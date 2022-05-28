using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardSurface : MonoBehaviour
{
    //app層
    // Start is called before the first frame update
    void Start()
    {
        //マウスポイントから座標を取得するための準備
        mainCamera = Camera.main;
        PosZ = ally1.transform.position.z;

        //1.駒を初期配置に置く
        PlaceMonsterInitially();

        //アニメーション作成のために一時的にattack()を開始時に呼び出し
        //ally1.GetComponent<ally1>().Attack();

        // //Focusを配列に入れる
        // FocusBox[0] = FocusLeftUp;
        // FocusBox[1] = FocusLeft;
        // FocusBox[2] = FocusLeftDown;
        // FocusBox[3] = FocusUp;
        // FocusBox[4] = FocusDown;
        // FocusBox[5] = FocusRightUp;
        // FocusBox[6] = FocusRight;
        // FocusBox[7] = FocusRightDown;
    }
    
    int[,] AvailableSquares;

    // クリック時
    void OnMouseDown()
    {
        //1.タップした先のモンスターを返す関数（いない場合はnullを返す）
        allyDrag = MonsterOnTaped();

        if (allyDrag != null){
            //③味方駒のcanvasを半透明にする
            Ally.alpha = 0.6f;
            //④初期配置の場所にHighLight(originally)を表示する
            HighLight_O.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
            HighLight_O.GetComponent<Animator>().SetTrigger("setOriginally");
            //HighLight(distination)を表示する
            BeforePointX = InitialPointX;
            BeforePointY = InitialPointY;
            HighLight_D.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
            HighLight_D.GetComponent<Animator>().SetTrigger("setDistination");

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
            }
            //動けるマスの範囲の二次元配列を作成して{PointX,PointY}の組み合わせが存在すれば下の処理に進める
            AvailableSquares = new int[24,2];
            //LeftUp
            if (moveAlly[0] != 0){
                if (InitialPointY-1 >= 0 && InitialPointX-1 >= 0 && arrayBoard[InitialPointY-1,InitialPointX-1] == 0){
                    FocusLeftUp.transform.GetChild(0).GetComponent<focus>().FocusMove();
                    AvailableSquares[0,0] = InitialPointX-1;
                    AvailableSquares[0,1] = InitialPointY-1;
                }
            }
            if (moveAlly[0] == 2){
                if (InitialPointY >= 2 && InitialPointX >= 2 && arrayBoard[InitialPointY-2,InitialPointX-2] == 0){
                    FocusLeftUp.transform.GetChild(1).GetComponent<focus>().FocusMove();
                    AvailableSquares[1,0] = InitialPointX-1;
                    AvailableSquares[1,1] = InitialPointY-1;
                }
                if (InitialPointY >= 3 && InitialPointX >= 3 && arrayBoard[InitialPointY-3,InitialPointX-3] == 0){
                    FocusLeftUp.transform.GetChild(2).GetComponent<focus>().FocusMove();
                    AvailableSquares[3,0] = InitialPointX-3;
                    AvailableSquares[3,1] = InitialPointY-3;
                }
            }
            //Left
            if (moveAlly[1] != 0){
                if (InitialPointX >= 1 && arrayBoard[InitialPointY,InitialPointX-1] == 0){
                    FocusLeft.transform.GetChild(0).GetComponent<focus>().FocusMove();
                    AvailableSquares[3,0] = InitialPointX-1;
                    AvailableSquares[3,1] = InitialPointY;
                }
            }
            if (moveAlly[1] == 2){
                if (InitialPointX >= 2 && arrayBoard[InitialPointY,InitialPointX-2] == 0){
                    FocusLeft.transform.GetChild(1).GetComponent<focus>().FocusMove();
                    AvailableSquares[4,0] = InitialPointX-2;
                    AvailableSquares[4,1] = InitialPointY;
                }
                if (InitialPointX >= 3 && arrayBoard[InitialPointY,InitialPointX-3] == 0){
                    FocusLeft.transform.GetChild(2).GetComponent<focus>().FocusMove();
                    AvailableSquares[5,0] = InitialPointX-3;
                    AvailableSquares[5,1] = InitialPointY;
                }
            }
            //LeftDown
            if (moveAlly[2] != 0){
                if (InitialPointY <= 2 && InitialPointX >= 1 && arrayBoard[InitialPointY+1,InitialPointX-1] == 0){
                    FocusLeftDown.transform.GetChild(0).GetComponent<focus>().FocusMove();
                    AvailableSquares[6,0] = InitialPointX-1;
                    AvailableSquares[6,1] = InitialPointY+1;
                }
            }
            if (moveAlly[2] == 2){
                if (InitialPointY <= 1 && InitialPointX >= 2 && arrayBoard[InitialPointY+2,InitialPointX-2] == 0){
                    FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                    AvailableSquares[7,0] = InitialPointX-2;
                    AvailableSquares[7,1] = InitialPointY+2;
                }
                if (InitialPointY <= 0 && InitialPointX >= 3 && arrayBoard[InitialPointY+3,InitialPointX-3] == 0){
                    FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                    AvailableSquares[8,0] = InitialPointX-3;
                    AvailableSquares[8,1] = InitialPointY+3;
                }
            }
            //Up
            if (moveAlly[3] != 0){
                if (InitialPointY >= 1 && arrayBoard[InitialPointY-1,InitialPointX] == 0){
                    FocusUp.transform.GetChild(0).GetComponent<focus>().FocusMove();
                    AvailableSquares[9,0] = InitialPointX;
                    AvailableSquares[9,1] = InitialPointY-1;
                }
            }
            if (moveAlly[3] == 2){
                if (InitialPointY >= 2 && arrayBoard[InitialPointY-2,InitialPointX] == 0){
                    FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                    AvailableSquares[10,0] = InitialPointX;
                    AvailableSquares[10,1] = InitialPointY-2;
                }
                if (InitialPointY >= 3 && arrayBoard[InitialPointY-3,InitialPointX] == 0){
                    FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                    AvailableSquares[11,0] = InitialPointX;
                    AvailableSquares[11,1] = InitialPointY-3;
                }
            }
            //Down
            if (moveAlly[4] != 0){
                if (InitialPointY <= 2 && arrayBoard[InitialPointY+1,InitialPointX] == 0){
                    FocusDown.transform.GetChild(0).GetComponent<focus>().FocusMove();
                    AvailableSquares[12,0] = InitialPointX;
                    AvailableSquares[12,1] = InitialPointY+1;
                }
            }
            if (moveAlly[4] == 2){
                if (InitialPointY <= 1 && arrayBoard[InitialPointY+2,InitialPointX] == 0){
                    FocusDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                    AvailableSquares[13,0] = InitialPointX;
                    AvailableSquares[13,1] = InitialPointY+2;
                }
                if (InitialPointY <= 0 && arrayBoard[InitialPointY+3,InitialPointX] == 0){
                    FocusDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                    AvailableSquares[14,0] = InitialPointX;
                    AvailableSquares[14,1] = InitialPointY+3;
                }
            }
            //RightUp
            if (moveAlly[5] != 0){
                if (InitialPointY >= 1 && InitialPointX <= 2 && arrayBoard[InitialPointY-1,InitialPointX+1] == 0){
                    FocusRightUp.transform.GetChild(0).GetComponent<focus>().FocusMove();
                    AvailableSquares[15,0] = InitialPointX+1;
                    AvailableSquares[15,1] = InitialPointY-1;
                }
            }
            if (moveAlly[5] == 2){
                if (InitialPointY >= 2 && InitialPointX <= 1 && arrayBoard[InitialPointY-2,InitialPointX+2] == 0){
                    FocusRightUp.transform.GetChild(1).GetComponent<focus>().FocusMove();
                    AvailableSquares[16,0] = InitialPointX+2;
                    AvailableSquares[16,1] = InitialPointY-2;
                }
                if (InitialPointY >= 3 && InitialPointX <= 0 && arrayBoard[InitialPointY-3,InitialPointX+3] == 0){
                    FocusRightUp.transform.GetChild(2).GetComponent<focus>().FocusMove();
                    AvailableSquares[17,0] = InitialPointX+3;
                    AvailableSquares[17,1] = InitialPointY-3;
                }
            }
            //Right
            if (moveAlly[6] != 0){
                if (InitialPointX <= 2 && arrayBoard[InitialPointY,InitialPointX+1] == 0){
                    FocusRight.transform.GetChild(0).GetComponent<focus>().FocusMove();
                    AvailableSquares[18,0] = InitialPointX+1;
                    AvailableSquares[18,1] = InitialPointY;
                }
            }
            if (moveAlly[6] == 2){
                if (InitialPointX <= 1 && arrayBoard[InitialPointY,InitialPointX+2] == 0){
                    FocusRight.transform.GetChild(1).GetComponent<focus>().FocusMove();
                    AvailableSquares[19,0] = InitialPointX+2;
                    AvailableSquares[19,1] = InitialPointY;
                }
                if (InitialPointX <= 0 && arrayBoard[InitialPointY,InitialPointX+3] == 0){
                    FocusRight.transform.GetChild(2).GetComponent<focus>().FocusMove();
                    AvailableSquares[20,0] = InitialPointX+3;
                    AvailableSquares[20,1] = InitialPointY;
                }
            }
            //RightDown
            if (moveAlly[7] != 0){
                if (InitialPointY <= 2 && InitialPointX <= 2 && arrayBoard[InitialPointY+1,InitialPointX+1] == 0){
                    FocusRightDown.transform.GetChild(0).GetComponent<focus>().FocusMove();
                    AvailableSquares[21,0] = InitialPointX+1;
                    AvailableSquares[21,1] = InitialPointY+1;
                }
            }
            if (moveAlly[7] == 2){
                if (InitialPointY <= 1 && InitialPointX <= 1 && arrayBoard[InitialPointY+2,InitialPointX+2] == 0){
                    FocusRightDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                    AvailableSquares[22,0] = InitialPointX+2;
                    AvailableSquares[22,1] = InitialPointY+2;
                }
                if (InitialPointY <= 0 && InitialPointX <= 0 && arrayBoard[InitialPointY+3,InitialPointX+3] == 0){
                    FocusRightDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                    AvailableSquares[23,0] = InitialPointX+3;
                    AvailableSquares[23,1] = InitialPointY+3;
                }
            }
        }
    }

    // ドラッグ時
    void OnMouseDrag()
    {
        //allyDragに駒オブジェクトがセットされてる場合のみ実行
        if (allyDrag != null) {
            //1.モンスターをドラッグに合わせて移動させる関数
            DragMonster();
            //2.ドラッグした先のHighLightをdistinationにする
            //座標からマス目を取得
            // mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            PointArray = GetBoardPoint(mousePos.x, mousePos.y);
            //HighLghtの移動
            PointX = PointArray[0];
            PointY = PointArray[1];
            if (PointX != BeforePointX || PointY != BeforePointY){
                for(int i = 0; i < AvailableSquares.GetLength(0); i++){
                    if (AvailableSquares[i,0] == PointX && AvailableSquares[i,1] == PointY){
                        Debug.Log("移動可能マス");
                        BeforePointX = PointX;
                        BeforePointY = PointY;
                        HighLight_D.transform.localPosition = new Vector3(PointValueXArray[PointX], PointValueYArray[PointY], 0);
                        break;
                    }
                }
                
            }

            //座標が盤面から外に出た時に駒を初期位置に戻す
            if (mousePos.x < -2.2 || mousePos.x > 2.2 || mousePos.y < -2.2 || mousePos.y > 2.2){
                allyDrag.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);

                HighLight_O.GetComponent<Animator>().SetTrigger("outOriginally");
                HighLight_D.GetComponent<Animator>().SetTrigger("outDistination");

                //2.focusの解除
                FocusParent.SetActive(false);

                //③allyDragを空に戻して置く
                allyDrag = null;

                //④alluDragがセットされた場合に味方駒のcanvasを半透明にする
                Ally.alpha = 1.0f;
            }
        }
    }

    // ドロップ時
    void OnMouseUp()
    {
        //allyDragに駒オブジェクトがセットされてる場合のみ実行
        if (allyDrag != null) {
            //1.モンスターをドロップしたときの処理を行う関数
            // DropMonster();
            //①座標からマスを導出
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            PointArray = GetBoardPoint(mousePos.x, mousePos.y);
            PointX = PointArray[0];
            PointY = PointArray[1];

            //動けるマスの整理
            bool immovable = false;
            for(int i = 0; i < AvailableSquares.GetLength(0); i++){
                if (AvailableSquares[i,0] == PointX && AvailableSquares[i,1] == PointY){
                    //②マスに駒が置かれていないかを判別
                    if (arrayBoard[PointY,PointX] == 0) {
                        //駒オブジェクトのpositionの書き換え
                        //ドロップ時のマウスの位置するマスに対応したローカル座標を配列PointValueX(Y)Arrayで取得している
                        mousePos.z = PosZ;
                        allyDrag.transform.localPosition = new Vector3(PointValueXArray[PointX], PointValueYArray[PointY], 0);
                        //配列arrayBoardの書き換え
                        arrayBoard[InitialPointY,InitialPointX] = 0;    //元いたマスのステータスを0にする
                        arrayBoard[PointY,PointX] = DragObjectNum;      //移動した先のマスのステータスを駒の番号にする

                        immovable = true;
                    }
                    break;
                }
            }
            if (!immovable){
                //駒を初期位置に戻す
                allyDrag.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0); 
            }

            //③allyDragを空に戻して置く
            allyDrag = null;

            //④alluDragがセットされた場合に味方駒のcanvasを半透明にする
            Ally.alpha = 1.0f;
            
            //2.HighLightの解除
            HighLight_O.GetComponent<Animator>().SetTrigger("outOriginally");
            HighLight_D.GetComponent<Animator>().SetTrigger("outDistination");

            //2.focusの解除
            FocusParent.SetActive(false);
        }
    }

    //model層
    
    //1.味方駒をドラッグ&ドロップする機能
    //1.1.変数
    //駒オブジェクトをuGUIでアタッチする
    public GameObject ally1;
    public GameObject ally2;
    public GameObject ally3;
    public GameObject ally4;
    public GameObject enemy1;
    public GameObject enemy2;

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
    private int BeforePointX;
    private int BeforePointY;

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

    //1.2.関数
    //駒を初期配置に置く関数
    void PlaceMonsterInitially(){
        //①配列の設定（駒が初期配置されるマス目のステータスを変更）
        arrayBoard[1,3] = 1;   // ally1
        arrayBoard[2,3] = 2;  // ally2
        arrayBoard[3,3] = 3;  // ally3
        arrayBoard[3,2] = 4;  // ally4
        arrayBoard[1,0] = 5;  // enemy1
        arrayBoard[0,1] = 6;  // enemy2
        //②駒オブジェクトに座標を指定
        ally1.transform.localPosition = new Vector3(105, 35, 0);
        ally2.transform.localPosition = new Vector3(105, -35, 0);
        ally3.transform.localPosition = new Vector3(105, -105, 0);
        ally4.transform.localPosition = new Vector3(35, -105, 0);
        enemy1.transform.localPosition = new Vector3(-105, 35, 0);
        enemy2.transform.localPosition = new Vector3(-35, 105, 0);
    }
    
    //タップした先のモンスターを返す関数
    GameObject? MonsterOnTaped(){
        //①タップしたマスに味方駒があるか判定
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        PointArray = GetBoardPoint(mousePos.x, mousePos.y); //GetBoardPoint()は下で定義してるよ！
        InitialPointX = PointArray[0];
        InitialPointY = PointArray[1];

        //②味方駒がいた場合にallyDragにその駒のオブジェクトをセットする
        //transform.SetAsLastSibling()でヒエラルキー内の順序を変更し，一番手前に表示
        if (arrayBoard[InitialPointY,InitialPointX] == 1) {
            // allyDrag = ally1;
            DragObjectNum = 1;
            ally1.transform.SetAsLastSibling();
            return ally1;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 2) {
            // allyDrag = ally2;
            DragObjectNum = 2;
            ally2.transform.SetAsLastSibling();
            return ally2;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 3) {
            // allyDrag = ally3;
            DragObjectNum = 3;
            ally3.transform.SetAsLastSibling();
            return ally3;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 4) {
            // allyDrag = ally4;
            DragObjectNum = 4;
            ally4.transform.SetAsLastSibling();
            return ally4;
        }else{
            return null;
        }
    }
    
    //関数GetBoardPoint()
    //float型の座標を引数で与えるとどのマス目かを返す関数
    //返り値の型は長さ2の配列（int）
    int[] GetBoardPoint(float mPosX, float mPosY) {
        
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

    //モンスターをドラッグに合わせて移動させる関数
    void DragMonster(){
        //マウスから取得した座標をallyDragオブジェクトに入れる
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = PosZ;
        allyDrag.transform.position = mousePos;
    }

    //モンスターをドロップしたときの処理を行う関数
    void DropMonster(){
    }

    //2.モンスターをドラッグさせている間の味方駒の表示エフェクト
    //2.1.変数
    //盤面に番号を振る
    int[,] arrayBoardNum = new int[4, 4]{
        {1,2,3,4},
        {5,6,7,8},
        {9,10,11,12},
        {13,14,15,16}
    };
    //盤面の番号を格納するための変数
    private int boardNum;
    //プレハブSquareをセットするための空オブジェクト
    // private GameObject? Square;
    //子オブジェクトとしてプレハブを取得するためのBoardオブジェクトをアタッチ
    // public GameObject Board;

    //プレハブを全て予めアタッチしておく
    public GameObject FocusParent;
    public GameObject FocusLeftUp;
    public GameObject FocusLeft;
    public GameObject FocusLeftDown;
    public GameObject FocusUp;
    public GameObject FocusDown;
    public GameObject FocusRightUp;
    public GameObject FocusRight;
    public GameObject FocusRightDown;

    //アタッチしたプレハブを配列とする
    // GameObject[] FocusBox = new GameObject[16];

    //各モンスターの動く範囲のマスを配列としておく
    int[] moveAlly1 = {1,2,1,0,0,2,2,2};
    int[] moveAlly2 = {1,2,1,1,0,1,2,1};
    int[] moveAlly3 = {1,1,1,0,0,1,1,1};
    int[] moveAlly4 = {1,2,0,1,1,0,2,0};
    //上記をセットするための空配列
    int[] moveAlly = new int[8];

    //ドラッグ時に味方駒を半透明とするためのAllyをアタッチしておく
    public CanvasGroup Ally;
    
    //HighLight(originally)をアタッチ
    public GameObject HighLight_O;
    //HighLight(distination)をアタッチ
    public GameObject HighLight_D;
}
