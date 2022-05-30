using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardSurface : MonoBehaviour
{
    //駒オブジェクトをアタッチする
    public GameObject ally1;
    public GameObject ally2;
    public GameObject ally3;
    public GameObject ally4;
    public GameObject enemy1;
    public GameObject enemy2;

    //ドラッグ時に味方駒を半透明とするためのAllyをアタッチしておく
    public CanvasGroup Ally;
    
    //HighLight(originally)をアタッチ
    public GameObject HighLight_O;
    //HighLight(distination)をアタッチ
    public GameObject HighLight_D;

    //Squareプレハブをアタッチする
    public GameObject Square01;
    public GameObject Square02;
    public GameObject Square03;
    public GameObject Square04;
    public GameObject Square05;
    public GameObject Square06;
    public GameObject Square07;
    public GameObject Square08;
    public GameObject Square09;
    public GameObject Square10;
    public GameObject Square11;
    public GameObject Square12;
    public GameObject Square13;
    public GameObject Square14;
    public GameObject Square15;
    public GameObject Square16;

    //Focusプレハブをアタッチする
    public GameObject FocusParent;
    public GameObject FocusLeftUp;
    public GameObject FocusLeft;
    public GameObject FocusLeftDown;
    public GameObject FocusUp;
    public GameObject FocusDown;
    public GameObject FocusRightUp;
    public GameObject FocusRight;
    public GameObject FocusRightDown;

    //ドラッグ時のみドラッグする駒をセットするための空オブジェクト
    //「?」の記載はallyDragをnullと置けるようにするため（null許容型）
    private GameObject? allyDrag;

    //駒をタッチ操作する処理に必要な変数の定義
    private Camera mainCamera;
    private float PosZ;
    private Vector3 mousePos;
    private int[] PointArray = new int[2];
    private int DragObjectNum;
    private int InitialPointX;
    private int InitialPointY;
    private int PointX;
    private int PointY;

    //マス目に対応するlocal座標を配列としたもの
    float[] PointValueXArray = {-105, -35, 35, 105};
    float[] PointValueYArray = {105, 35, -35, -105};

    //盤面用の配列
    //4行4列の配列を定義
    //行->Y位置、列->X位置
    //値はマスのステータスを表す（0->空, 1->ally1, 2->ally2, 3->ally3, 4->ally4, 5->enemy1, 6->enemy2,）
    int[,] arrayBoard = new int[4, 4]{
        {0,0,0,0},
        {0,0,0,0},
        {0,0,0,0},
        {0,0,0,0}
    };

    //各モンスターの動く範囲のマスを配列としておく
    // 0 -> 矢印なし, 1 -> 一重矢印, 2 -> 二重矢印
    //{左上, 左, 左下, 上, 下, 右上, 右, 右下}
    int[] moveAlly1 = {1,2,1,0,0,2,2,2};
    int[] moveAlly2 = {1,2,1,1,0,1,2,1};
    int[] moveAlly3 = {1,1,1,0,0,1,1,1};
    int[] moveAlly4 = {1,2,0,1,1,0,2,0};
    int[] moveEnemy1 = {0,1,0,0,0,2,0,2};
    int[] moveEnemy2 = {0,1,0,1,1,0,1,0};
    //上記をセットするための空配列
    int[] moveAlly = new int[8];
    //モンスターが動ける範囲のマスを配列とする
    int[,] AvailableSquares;

    //Squareプレハブを盤面状に格納するための配列
    GameObject[,] SquareBox = new GameObject[4,4];

    //ドロップ可能か判定のためのbool値
    bool immovable = false;

    //ターン判定のためのbool値（プレイヤーターン -> true, 敵ターン -> false）
    bool PlayerTurn;
    
    // Start is called before the first frame update
    void Start()
    {
        //マウスポイントから座標を取得するための準備
        mainCamera = Camera.main;
        PosZ = ally1.transform.position.z;

        //駒を初期配置に置く
        PlaceMonsterInitially();

        //アニメーション作成のために一時的にattack()を開始時に呼び出し
        //ally1.GetComponent<ally1>().Attack();

        //SquareBoxにプレハブをセットする
        PrepareSquareBox();

        //プレイヤーターンをセットする
        PlayerTurn = true;
    }

    // タップ時
    void OnMouseDown()
    {
        //プレイヤーターンの場合のみ実行
        if (PlayerTurn){
            //タップした先のモンスターを返す関数（いない場合はnullを返す）
            //InitialPointX,InitialPointYもセットされる
            allyDrag = MonsterOnTaped();

            //プレイヤー駒をタップした場合の処理
            if (allyDrag == ally1 || allyDrag == ally2 || allyDrag == ally3 || allyDrag == ally4){
                //味方駒のcanvasを半透明にする
                Ally.alpha = 0.6f;

                //ハイライトを表示
                SetHighLight();

                //動けるマスの範囲の二次元配列を作成・Squareプレハブの非表示・Focusプレハブの表示
                GenerateAvailableSquares(InitialPointX,InitialPointY,true);
            }
            //敵駒をタップした場合の処理
            else {
                GenerateAvailableSquares(InitialPointX,InitialPointY,false);
            }
        }
    }

    // ドラッグ時
    void OnMouseDrag()
    {
        //プレイヤーターンかつallyDragにプレイヤー駒オブジェクトがセットされてる場合のみ実行
        if (PlayerTurn && allyDrag != null && allyDrag != enemy1 && allyDrag != enemy2) {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            //モンスターをドラッグに合わせて移動させる
            DragMonster();

            //ハイライトをドラッグに合わせて移動させる
            DragHighLight();
            
            //座標が盤面から外に出た時に駒を初期位置に戻す
            if (mousePos.x < -2 || mousePos.x > 2 || mousePos.y < -2 || mousePos.y > 2){
                InitializationDrag(false);
            }
        }
    }

    // ドロップ時
    void OnMouseUp()
    {
        //プレイヤーターンかつallyDragに駒オブジェクトがセットされてる場合のみ実行
        if (PlayerTurn && allyDrag != null) {
            //プレイヤー駒がセットされていた場合の処理
            if (allyDrag == ally1 || allyDrag == ally2 || allyDrag == ally3 || allyDrag == ally4){
                //モンスターをドロップしたときの処理を行う関数
                immovable = DropMonster();
                InitializationDrag(immovable);
                if (immovable){
                    //プレイターターンを終了させる処理
                    PlayerTurn = false;
                    ActionEnemyTurn();
                }
            }
            //敵駒がセットされていた場合の処理
            else {
                //エフェクトのリセットを行う
                InitializationEffect();
            }
        }
    }

    //関数：駒を初期配置に置く
    void PlaceMonsterInitially(){
        //①配列の設定（駒が初期配置されるマス目のステータスを変更）
        arrayBoard[1,3] = 1;   // ally1
        arrayBoard[2,3] = 2;  // ally2
        arrayBoard[3,3] = 3;  // ally3
        arrayBoard[3,2] = 4;  // ally4
        arrayBoard[1,0] = 11;  // enemy1
        arrayBoard[0,1] = 12;  // enemy2
        //②駒オブジェクトに座標を指定
        ally1.transform.localPosition = new Vector3(105, 35, 0);
        ally2.transform.localPosition = new Vector3(105, -35, 0);
        ally3.transform.localPosition = new Vector3(105, -105, 0);
        ally4.transform.localPosition = new Vector3(35, -105, 0);
        enemy1.transform.localPosition = new Vector3(-105, 35, 0);
        enemy2.transform.localPosition = new Vector3(-35, 105, 0);
    }

    //関数：SquareBoxにプレハブをセットする関数
    void PrepareSquareBox(){
        SquareBox[0,0] = Square01;
        SquareBox[0,1] = Square02;
        SquareBox[0,2] = Square03;
        SquareBox[0,3] = Square04;
        SquareBox[1,0] = Square05;
        SquareBox[1,1] = Square06;
        SquareBox[1,2] = Square07;
        SquareBox[1,3] = Square08;
        SquareBox[2,0] = Square09;
        SquareBox[2,1] = Square10;
        SquareBox[2,2] = Square11;
        SquareBox[2,3] = Square12;
        SquareBox[3,0] = Square13;
        SquareBox[3,1] = Square14;
        SquareBox[3,2] = Square15;
        SquareBox[3,3] = Square16;
    }
    
    //関数：タップした先のモンスターを返す
    GameObject? MonsterOnTaped(){
        //①タップしたマスに味方駒があるか判定
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        PointArray = GetBoardPoint(mousePos.x, mousePos.y); //GetBoardPoint()は下で定義してるよ！
        InitialPointX = PointArray[0];
        InitialPointY = PointArray[1];

        //②味方駒がいた場合にallyDragにその駒のオブジェクトをセットする
        //transform.SetAsLastSibling()でヒエラルキー内の順序を変更し，一番手前に表示
        if (arrayBoard[InitialPointY,InitialPointX] == 1) {
            DragObjectNum = 1;
            ally1.transform.SetAsLastSibling();
            return ally1;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 2) {
            DragObjectNum = 2;
            ally2.transform.SetAsLastSibling();
            return ally2;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 3) {
            DragObjectNum = 3;
            ally3.transform.SetAsLastSibling();
            return ally3;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 4) {
            DragObjectNum = 4;
            ally4.transform.SetAsLastSibling();
            return ally4;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 11) {
            DragObjectNum = 11;
            return enemy1;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 12) {
            DragObjectNum = 12;
            return enemy2;
        }else{
            return null;
        }
    }
    
    //関数：float型の座標を引数で与えるとどのマス目かを返す関数
    //返り値の型は長さ2の配列（int）
    int[] GetBoardPoint(float mPosX, float mPosY){
        
        if (mPosX > -2.2 && mPosX < -1.1) {
            PointX = 0;
        }else if (mPosX > -1.1 && mPosX < 0) {
            PointX = 1;
        }else if (mPosX > 0 && mPosX < 1.1) {
            PointX = 2;
        }else if (mPosX > 1.1 && mPosX < 2.2) {
            PointX = 3;
        }

        if (mPosY < 2.2 && mPosY > 1.1) {
            PointY = 0;
        }else if (mPosY < 1.1 && mPosY > 0) {
            PointY = 1;
        }else if (mPosY < 0 && mPosY > -1.1) {
            PointY = 2;
        }else if (mPosY < -1.1 && mPosY > -2.2) {
            PointY = 3;
        }

        int[] arr = {PointX, PointY};

        return arr;
    }

    //関数：タップ時にハイライトを表示する関数
    void SetHighLight(){
        //初期配置の場所にHighLight(originally)を表示する
        HighLight_O.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
        HighLight_O.GetComponent<Animator>().SetTrigger("setOriginally");
        //HighLight(distination)を表示する
        HighLight_D.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
        HighLight_D.GetComponent<Animator>().SetTrigger("setDistination");
    }

    //関数：動けるマスの範囲の二次元配列を作成とプレハブの非表示
    void GenerateAvailableSquares(int InitialPointX, int InitialPointY, bool Player){
            //Focusを座標にセットする
            FocusParent.SetActive(true);
            FocusParent.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
            //対象のFocusプレハブをアクションさせる
            if (allyDrag == ally1){
                moveAlly = moveAlly1;
            } else if (allyDrag == ally2){
                moveAlly = moveAlly2;
            } else if (allyDrag == ally3){
                moveAlly = moveAlly3;
            } else if (allyDrag == ally4){
                moveAlly = moveAlly4;
            } else if (allyDrag == enemy1){
                moveAlly = moveEnemy1;
            } else if (allyDrag == enemy2){
                moveAlly = moveEnemy2;
            }

            AvailableSquares = new int[24,2]{
                {10,10},{10,10},{10,10},{10,10},{10,10},{10,10},
                {10,10},{10,10},{10,10},{10,10},{10,10},{10,10},
                {10,10},{10,10},{10,10},{10,10},{10,10},{10,10},
                {10,10},{10,10},{10,10},{10,10},{10,10},{10,10}
            };
            SquareBox[InitialPointY,InitialPointX].SetActive(false);
            //LeftUp
            if (moveAlly[0] != 0){
                if (InitialPointY-1 >= 0 && InitialPointX-1 >= 0 && arrayBoard[InitialPointY-1,InitialPointX-1] == 0){
                    if(Player){
                        FocusLeftUp.transform.GetChild(0).GetComponent<focus>().FocusMove();
                        AvailableSquares[0,0] = InitialPointX-1;
                        AvailableSquares[0,1] = InitialPointY-1;
                    }else{
                        FocusLeftUp.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY-1,InitialPointX-1].SetActive(false);
                }
            }
            if (moveAlly[0] == 2){
                if (InitialPointY >= 2 && InitialPointX >= 2 && arrayBoard[InitialPointY-2,InitialPointX-2] == 0){
                    if(Player){
                        FocusLeftUp.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        AvailableSquares[1,0] = InitialPointX-1;
                        AvailableSquares[1,1] = InitialPointY-1;
                    }else{
                        FocusLeftUp.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY-2,InitialPointX-2].SetActive(false);
                }
                if (InitialPointY >= 3 && InitialPointX >= 3 && arrayBoard[InitialPointY-3,InitialPointX-3] == 0){
                    if(Player){
                        FocusLeftUp.transform.GetChild(2).GetComponent<focus>().FocusMove();
                        AvailableSquares[3,0] = InitialPointX-3;
                        AvailableSquares[3,1] = InitialPointY-3;
                    }else{
                        FocusLeftUp.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY-3,InitialPointX-3].SetActive(false);
                }
            }
            //Left
            if (moveAlly[1] != 0){
                if (InitialPointX >= 1 && arrayBoard[InitialPointY,InitialPointX-1] == 0){
                    if(Player){
                        FocusLeft.transform.GetChild(0).GetComponent<focus>().FocusMove();
                        AvailableSquares[3,0] = InitialPointX-1;
                        AvailableSquares[3,1] = InitialPointY;
                    }else{
                        FocusLeft.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY,InitialPointX-1].SetActive(false);
                }
            }
            if (moveAlly[1] == 2){
                if (InitialPointX >= 2 && arrayBoard[InitialPointY,InitialPointX-2] == 0){
                    if(Player){
                        FocusLeft.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        AvailableSquares[4,0] = InitialPointX-2;
                        AvailableSquares[4,1] = InitialPointY;
                    }else{
                        FocusLeft.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY,InitialPointX-2].SetActive(false);
                }
                if (InitialPointX >= 3 && arrayBoard[InitialPointY,InitialPointX-3] == 0){
                    if(Player){
                        FocusLeft.transform.GetChild(2).GetComponent<focus>().FocusMove();
                        AvailableSquares[5,0] = InitialPointX-3;
                        AvailableSquares[5,1] = InitialPointY;
                    }else{
                        FocusLeft.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY,InitialPointX-3].SetActive(false);
                }
            }
            //LeftDown
            if (moveAlly[2] != 0){
                if (InitialPointY <= 2 && InitialPointX >= 1 && arrayBoard[InitialPointY+1,InitialPointX-1] == 0){
                    if(Player){
                        FocusLeftDown.transform.GetChild(0).GetComponent<focus>().FocusMove();
                        AvailableSquares[6,0] = InitialPointX-1;
                        AvailableSquares[6,1] = InitialPointY+1;
                    }else{
                        FocusLeftDown.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY+1,InitialPointX-1].SetActive(false);
                }
            }
            if (moveAlly[2] == 2){
                if (InitialPointY <= 1 && InitialPointX >= 2 && arrayBoard[InitialPointY+2,InitialPointX-2] == 0){
                    if(Player){
                        FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        AvailableSquares[7,0] = InitialPointX-2;
                        AvailableSquares[7,1] = InitialPointY+2;
                    }else{
                        FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY+2,InitialPointX-2].SetActive(false);
                }
                if (InitialPointY <= 0 && InitialPointX >= 3 && arrayBoard[InitialPointY+3,InitialPointX-3] == 0){
                    if(Player){
                        FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                        AvailableSquares[8,0] = InitialPointX-3;
                        AvailableSquares[8,1] = InitialPointY+3;
                    }else{
                        FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY+3,InitialPointX-3].SetActive(false);
                }
            }
            //Up
            if (moveAlly[3] != 0){
                if (InitialPointY >= 1 && arrayBoard[InitialPointY-1,InitialPointX] == 0){
                    if(Player){
                        FocusUp.transform.GetChild(0).GetComponent<focus>().FocusMove();
                        AvailableSquares[9,0] = InitialPointX;
                        AvailableSquares[9,1] = InitialPointY-1;
                    }else{
                        FocusUp.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY-1,InitialPointX].SetActive(false);
                }
            }
            if (moveAlly[3] == 2){
                if (InitialPointY >= 2 && arrayBoard[InitialPointY-2,InitialPointX] == 0){
                    if(Player){
                        FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        AvailableSquares[10,0] = InitialPointX;
                        AvailableSquares[10,1] = InitialPointY-2;
                    }else{
                        FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY-2,InitialPointX].SetActive(false);
                }
                if (InitialPointY >= 3 && arrayBoard[InitialPointY-3,InitialPointX] == 0){
                    if(Player){
                        FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                        AvailableSquares[11,0] = InitialPointX;
                        AvailableSquares[11,1] = InitialPointY-3;
                    }else{
                        FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY-3,InitialPointX].SetActive(false);
                }
            }
            //Down
            if (moveAlly[4] != 0){
                if (InitialPointY <= 2 && arrayBoard[InitialPointY+1,InitialPointX] == 0){
                    if(Player){
                        FocusDown.transform.GetChild(0).GetComponent<focus>().FocusMove();
                        AvailableSquares[12,0] = InitialPointX;
                        AvailableSquares[12,1] = InitialPointY+1;
                    }else{
                        FocusDown.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY+1,InitialPointX].SetActive(false);
                }
            }
            if (moveAlly[4] == 2){
                if (InitialPointY <= 1 && arrayBoard[InitialPointY+2,InitialPointX] == 0){
                    if(Player){
                        FocusDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        AvailableSquares[13,0] = InitialPointX;
                        AvailableSquares[13,1] = InitialPointY+2;
                    }else{
                        FocusDown.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY+2,InitialPointX].SetActive(false);
                }
                if (InitialPointY <= 0 && arrayBoard[InitialPointY+3,InitialPointX] == 0){
                    if(Player){
                        FocusDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                        AvailableSquares[14,0] = InitialPointX;
                        AvailableSquares[14,1] = InitialPointY+3;
                    }else{
                        FocusDown.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY+3,InitialPointX].SetActive(false);
                }
            }
            //RightUp
            if (moveAlly[5] != 0){
                if (InitialPointY >= 1 && InitialPointX <= 2 && arrayBoard[InitialPointY-1,InitialPointX+1] == 0){
                    if(Player){
                        FocusRightUp.transform.GetChild(0).GetComponent<focus>().FocusMove();
                        AvailableSquares[15,0] = InitialPointX+1;
                        AvailableSquares[15,1] = InitialPointY-1;
                    }else{
                        FocusRightUp.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY-1,InitialPointX+1].SetActive(false);
                }
            }
            if (moveAlly[5] == 2){
                if (InitialPointY >= 2 && InitialPointX <= 1 && arrayBoard[InitialPointY-2,InitialPointX+2] == 0){
                    if(Player){
                        FocusRightUp.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        AvailableSquares[16,0] = InitialPointX+2;
                        AvailableSquares[16,1] = InitialPointY-2;
                    }else{
                        FocusRightUp.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY-2,InitialPointX+2].SetActive(false);
                }
                if (InitialPointY >= 3 && InitialPointX <= 0 && arrayBoard[InitialPointY-3,InitialPointX+3] == 0){
                    if(Player){
                        FocusRightUp.transform.GetChild(2).GetComponent<focus>().FocusMove();
                        AvailableSquares[17,0] = InitialPointX+3;
                        AvailableSquares[17,1] = InitialPointY-3;
                    }else{
                        FocusRightUp.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY-3,InitialPointX+3].SetActive(false);
                }
            }
            //Right
            if (moveAlly[6] != 0){
                if (InitialPointX <= 2 && arrayBoard[InitialPointY,InitialPointX+1] == 0){
                    if(Player){
                        FocusRight.transform.GetChild(0).GetComponent<focus>().FocusMove();
                        AvailableSquares[18,0] = InitialPointX+1;
                        AvailableSquares[18,1] = InitialPointY;
                    }else{
                        FocusRight.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY,InitialPointX+1].SetActive(false);
                }
            }
            if (moveAlly[6] == 2){
                if (InitialPointX <= 1 && arrayBoard[InitialPointY,InitialPointX+2] == 0){
                    if(Player){
                        FocusRight.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        AvailableSquares[19,0] = InitialPointX+2;
                        AvailableSquares[19,1] = InitialPointY;
                    }else{
                        FocusRight.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY,InitialPointX+2].SetActive(false);
                }
                if (InitialPointX <= 0 && arrayBoard[InitialPointY,InitialPointX+3] == 0){
                    if(Player){
                        FocusRight.transform.GetChild(2).GetComponent<focus>().FocusMove();
                        AvailableSquares[20,0] = InitialPointX+3;
                        AvailableSquares[20,1] = InitialPointY;
                    }else{
                        FocusRight.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY,InitialPointX+3].SetActive(false);
                }
            }
            //RightDown
            if (moveAlly[7] != 0){
                if (InitialPointY <= 2 && InitialPointX <= 2 && arrayBoard[InitialPointY+1,InitialPointX+1] == 0){
                    if(Player){
                        FocusRightDown.transform.GetChild(0).GetComponent<focus>().FocusMove();
                        AvailableSquares[21,0] = InitialPointX+1;
                        AvailableSquares[21,1] = InitialPointY+1;
                    }else{
                        FocusRightDown.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY+1,InitialPointX+1].SetActive(false);
                }
            }
            if (moveAlly[7] == 2){
                if (InitialPointY <= 1 && InitialPointX <= 1 && arrayBoard[InitialPointY+2,InitialPointX+2] == 0){
                    if(Player){
                        FocusRightDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        AvailableSquares[22,0] = InitialPointX+2;
                        AvailableSquares[22,1] = InitialPointY+2;
                    }else{
                        FocusRightDown.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY+2,InitialPointX+2].SetActive(false);
                }
                if (InitialPointY <= 0 && InitialPointX <= 0 && arrayBoard[InitialPointY+3,InitialPointX+3] == 0){
                    if(Player){
                        FocusRightDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                        AvailableSquares[23,0] = InitialPointX+3;
                        AvailableSquares[23,1] = InitialPointY+3;
                    }else{
                        FocusRightDown.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                    }
                    SquareBox[InitialPointY+3,InitialPointX+3].SetActive(false);
                }
            }
    }

    //関数：モンスターをドラッグに合わせて移動させる
    void DragMonster(){
        mousePos.z = PosZ;
        allyDrag.transform.position = mousePos;
    }

    //関数：ハイライトをドラッグに合わせて移動させる
    void DragHighLight(){
        //座標からマス目を取得
        PointArray = GetBoardPoint(mousePos.x, mousePos.y);
        //HighLghtの移動
        HighLight_D.transform.localPosition = new Vector3(PointValueXArray[PointArray[0]], PointValueYArray[PointArray[1]], 0);
    }
            
    //関数：駒の位置とエフェクトを初期化する
    void InitializationDrag(bool immovable){
        if (!immovable){
            allyDrag.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
        }

        //allyDragを空に戻して置く
        allyDrag = null;

        //alluDragがセットされた場合に味方駒のcanvasを半透明にする
        Ally.alpha = 1.0f;
            
        //HighLightの解除
        HighLight_O.GetComponent<Animator>().SetTrigger("outOriginally");
        HighLight_D.GetComponent<Animator>().SetTrigger("outDistination");

        InitializationEffect();
    }
            
    //関数：フォーカスとスクエアのエフェクトを初期化する
    void InitializationEffect(){
        //focusの解除
        FocusParent.SetActive(false);

        //Squareを元に戻す
        for (int i = 0; i < 4; i++){
            for (int j = 0; j < 4; j++){
                SquareBox[i,j].SetActive(true);
            }
        }
    }

    //関数：モンスターをドロップしたときの処理を行う
    bool DropMonster(){
        //座標からマスを導出
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        PointArray = GetBoardPoint(mousePos.x, mousePos.y);
        PointX = PointArray[0];
        PointY = PointArray[1];

        //動けるマスの整理
        bool immovable = false;
        for(int i = 0; i < AvailableSquares.GetLength(0); i++){
            if (AvailableSquares[i,0] == PointX && AvailableSquares[i,1] == PointY){
                //駒オブジェクトのpositionの書き換え
                //ドロップ時のマウスの位置するマスに対応したローカル座標を配列PointValueX(Y)Arrayで取得している
                mousePos.z = PosZ;
                    allyDrag.transform.localPosition = new Vector3(PointValueXArray[PointX], PointValueYArray[PointY], 0);
                //配列arrayBoardの書き換え
                arrayBoard[InitialPointY,InitialPointX] = 0;    //元いたマスのステータスを0にする
                arrayBoard[PointY,PointX] = DragObjectNum;      //移動した先のマスのステータスを駒の番号にする

                immovable = true;
                break;
            }
        }

        return immovable;
    }

    //関数：相手ターンの一連の処理を行う関数
    void ActionEnemyTurn(){

        int? EnemyNum = null;
        int[] moveEnemy = new int[8];

        int EnemyPointX;
        int EnemyPointY;

        int[,] MovableSquare = {
            {10,10},{10,10},{10,10},
            {10,10},{10,10},{10,10},
            {10,10},{10,10},{10,10}
        };
        
        //Enemyの中で一番X座標が小さい駒を取得
        for(int i=0; i<4; i++){
            for(int j=0; j<4; j++){
                if (arrayBoard[j,i]==11 || arrayBoard[j,i]==12){
                    EnemyNum = arrayBoard[j,i];
                    EnemyPointX = i;
                    EnemyPointY = j;
                    break;
                }
            }
            if(EnemyNum != null){
                break;
            }
        }
        Debug.Log(EnemyNum);
        
        //取得した駒にRight側で動けるマスがあるか判定
        GetEnemyMovableSquare(EnemyNum);
        
        //動けるマスの配列の中からランダムにひとつ取得   

        //存在しない場合違う駒を取得しなおす

        //取得した駒のフォーカスエフェクトを呼び出す

        //取得した駒のアニメーションを実行する

        //フォーカスアニメーションの解除

        //プレイヤーターンに移行
        PlayerTurn = true;
    }

    //関数：ActionEnemyTurn()内部で取得した駒にRight側で動けるマスがあるか判定
    void GetEnemyMovableSquare(int EnemyNum){
        if (EnemyNum == 11){
            moveEnemy = moveEnemy1;
        } else if (EnemyNum == 12){
            moveEnemy = moveEnemy2;
        }
        //RightUp
        if (moveEnemy[5] != 0){
            if (EnemyPointY >= 1 && EnemyPointX <= 2 && arrayBoard[EnemyPointY-1,EnemyPointX+1] == 0){
                MovableSquare[0,0] = EnemyPointX+1;
                MovableSquare[0,1] = EnemyPointY-1;
            }
            if (moveAlly[5] == 2){
                if (EnemyPointY >= 2 && EnemyPointX <= 1 && arrayBoard[EnemyPointY-2,EnemyPointX+2] == 0){
                    MovableSquare[1,0] = EnemyPointX+2;
                    MovableSquare[1,1] = EnemyPointY-2;
                }
                if (EnemyPointY >= 3 && EnemyPointX <= 0 && arrayBoard[EnemyPointY-3,EnemyPointX+3] == 0){
                    MovableSquare[2,0] = EnemyPointX+3;
                    MovableSquare[2,1] = EnemyPointY-3;
                }
            }
        }
        //Right
        if (moveEnemy[6] != 0){
            if (EnemyPointX <= 2 && arrayBoard[EnemyPointY,EnemyPointX+1] == 0){
                MovableSquare[3,0] = EnemyPointX+1;
                MovableSquare[3,1] = EnemyPointY-1;
            }
            if (moveAlly[6] == 2){
                if (EnemyPointX <= 1 && arrayBoard[EnemyPointY,EnemyPointX+2] == 0){
                    MovableSquare[4,0] = EnemyPointX+2;
                    MovableSquare[4,1] = EnemyPointY-2;
                }
                if (EnemyPointX <= 0 && arrayBoard[EnemyPointY,EnemyPointX+3] == 0){
                    MovableSquare[5,0] = EnemyPointX+3;
                    MovableSquare[5,1] = EnemyPointY-3;
                }
            }
        }
        //RightDown
        if (moveEnemy[7] != 0){
            if (EnemyPointY <= 2 && EnemyPointX <= 2 && arrayBoard[EnemyPointY+1,EnemyPointX+1] == 0){
                MovableSquare[6,0] = EnemyPointX+1;
                MovableSquare[6,1] = EnemyPointY-1;
            }
            if (moveAlly[7] == 2){
                if (EnemyPointY <= 1 && EnemyPointX <= 1 && arrayBoard[EnemyPointY+2,EnemyPointX+2] == 0){
                    MovableSquare[7,0] = EnemyPointX+2;
                    MovableSquare[7,1] = EnemyPointY+2;
                }
                if (EnemyPointY <= 0 && EnemyPointX <= 0 && arrayBoard[EnemyPointY+3,EnemyPointX+3] == 0){
                    MovableSquare[8,0] = EnemyPointX+3;
                    MovableSquare[8,1] = EnemyPointY+3;
                }
            }
        }
    }
}
