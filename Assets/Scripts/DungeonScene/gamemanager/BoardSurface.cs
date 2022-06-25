using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardSurface : MonoBehaviour
{
    //ゲームマネージャーオブジェクトをアタッチする
    public GameObject ActionPlayer;
    ActionPlayer ActionPlayerClass;
    public GameObject ActionEnemy;
    ActionEnemy ActionEnemyClass;

    //アクションマネージャーオブジェクトをアタッチする
    public GameObject DirectAttack;
    DirectAttack DirectAttackClass;

    //スクリーンマネージャーオブジェクトをアタッチする
    public GameObject TurnStart;
    TurnStart TurnStartClass;

    //駒オブジェクトをアタッチする
    public GameObject ally1;
    ally Ally1Class;
    public GameObject ally2;
    ally Ally2Class;
    public GameObject ally3;
    ally Ally3Class;
    public GameObject ally4;
    ally Ally4Class;

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
    private int[] lastPointArray = new int[2];
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
    //モンスター毎に座標を記憶させる
    int[] PointAlly1 = new int[2];
    int[] PointAlly2 = new int[2];
    int[] PointAlly3 = new int[2];
    int[] PointAlly4 = new int[2];
    int[] PointEnemy1 = new int[2];
    int[] PointEnemy2 = new int[2];

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
    public int[,] AvailableSquares;

    //Squareプレハブを盤面状に格納するための配列
    GameObject[,] SquareBox = new GameObject[4,4];

    //ターン判定のためのbool値（プレイヤーターン -> true, 敵ターン -> false）
    private bool PlayerTurn;

    //ダブルタップ判定に必要な変数
    private bool isDoubleTapStart; //タップ認識中のフラグ
    private float doubleTapTime; //タップ開始からの累積時間
    
    // Start is called before the first frame update
    void Start()
    {
        Ally1Class = ally1.GetComponent<ally>();
        Ally2Class = ally2.GetComponent<ally>();
        Ally3Class = ally3.GetComponent<ally>();
        Ally4Class = ally4.GetComponent<ally>();

        ActionPlayerClass = ActionPlayer.GetComponent<ActionPlayer>();
        ActionEnemyClass = ActionEnemy.GetComponent<ActionEnemy>();
        DirectAttackClass = DirectAttack.GetComponent<DirectAttack>();
        TurnStartClass = TurnStart.GetComponent<TurnStart>();

        //マウスポイントから座標を取得するための準備
        mainCamera = Camera.main;
        PosZ = ally1.transform.position.z;

        //駒を初期配置に置く
        PlaceMonsterInitially();

        //SquareBoxにプレハブをセットする
        PrepareSquareBox();

        //プレイヤーターンをセットする
        //ゲーム開始エフェクトが終了するまではfalseとする
        PlayerTurn = false;
    }

    void Update () {
        //ダブルタップの判定
        if (isDoubleTapStart){
            doubleTapTime += Time.deltaTime;
            if (doubleTapTime < 0.2f) {
                if (Input.GetMouseButtonDown (0)) {
                    isDoubleTapStart = false;
                    doubleTapTime = 0.0f;
                    Ally1Class.SwordEffectStop();
                    Ally2Class.SwordEffectStop();
                    Ally3Class.SwordEffectStop();
                    Ally4Class.SwordEffectStop();
                    Ally.alpha = 1.0f;   
                    HighLight_O.GetComponent<Animator>().SetTrigger("outOriginally");
                    HighLight_D.GetComponent<Animator>().SetTrigger("outDistination");
                    InitializationEffect();
                    StartCoroutine(ProcessAfterDrop(arrayBoard));
                    isDoubleTapStart = false;
                    doubleTapTime = 0.0f;
                }
            }else{
                isDoubleTapStart = false;
                doubleTapTime = 0.0f;
            }
        } else {
            //ダブルタップの開始時
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x > -2 && mousePos.x < 2 && mousePos.y > -2 && mousePos.y < 2 && PlayerTurn == true && Input.GetMouseButtonDown (0)) {
                isDoubleTapStart = true;
            }
        }
    }

    // タップ時
    void OnMouseDown()
    {
        //プレイヤーターンの場合のみ実行
        if (PlayerTurn){
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            lastPointArray = GetBoardPoint(mousePos.x, mousePos.y); //GetBoardPoint()は下で定義してるよ！
            InitialPointX = lastPointArray[0];
            InitialPointY = lastPointArray[1];

            //タップした先のモンスターを返す関数（いない場合はnullを返す）
            //InitialPointX,InitialPointYもセットされる
            allyDrag = ActionPlayerClass.MonsterOnTaped(mousePos,InitialPointX,InitialPointY,arrayBoard);

            //プレイヤー駒をタップした場合の処理
            if (allyDrag == ally1 || allyDrag == ally2 || allyDrag == ally3 || allyDrag == ally4){
                //味方駒のcanvasを半透明にする
                Ally.alpha = 0.6f;

                //ハイライトを表示
                ActionPlayerClass.SetHighLight(InitialPointX,InitialPointY);

                //動けるマスの範囲の二次元配列を作成・Squareプレハブの非表示・Focusプレハブの表示
                AvailableSquares = GenerateAvailableSquares(allyDrag,InitialPointX,InitialPointY,true);

                //攻撃矢印アニメーションを呼び出す
                Ally1Class.SwordEffectAction(PointAlly1,arrayBoard,moveAlly1);
                Ally2Class.SwordEffectAction(PointAlly2,arrayBoard,moveAlly2);
                Ally3Class.SwordEffectAction(PointAlly3,arrayBoard,moveAlly3);
                Ally4Class.SwordEffectAction(PointAlly4,arrayBoard,moveAlly4);
            }
            //敵駒をタップした場合の処理
            else {
                AvailableSquares = GenerateAvailableSquares(allyDrag,InitialPointX,InitialPointY,false);
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
            ActionPlayerClass.DragMonster(mousePos,allyDrag,PosZ);

            //現在のマウスポジションからマスの座標を取得
            PointArray = GetBoardPoint(mousePos.x, mousePos.y);
            //前フレームとマスの座標を比較し現在のマウスポジションが変更された場合のみ以下の処理を呼び出す
            if(PointArray[0]!=lastPointArray[0]||PointArray[1]!=lastPointArray[1]){
                //HighLghtの移動
                HighLight_D.transform.localPosition = new Vector3(PointValueXArray[PointArray[0]], PointValueYArray[PointArray[1]], 0);
            
                //攻撃矢印アニメーションを呼び出す
                allyDrag.GetComponent<ally>().SwordEffectAction(PointArray,arrayBoard,moveAlly);
                
                //lastPointArrayの更新
                lastPointArray[0] = PointArray[0];
                lastPointArray[1] = PointArray[1];
            }
            
            //座標が盤面から外に出た時に駒を初期位置に戻す
            if (mousePos.x < -2 || mousePos.x > 2 || mousePos.y < -2 || mousePos.y > 2){
                ActionPlayerClass.InitializationDrag(allyDrag,Ally,false,InitialPointX,InitialPointY);
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
                //ドロップ可能か判定のためのbool値
                bool immovable = false;

                //座標からマスを導出
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                PointArray = GetBoardPoint(mousePos.x, mousePos.y);
                PointX = PointArray[0];
                PointY = PointArray[1];

                //モンスターをドロップしたときの処理を行う関数
                immovable = ActionPlayerClass.DropMonster(AvailableSquares,PointX,PointY,InitialPointX,InitialPointY,allyDrag,DragObjectNum);
                ActionPlayerClass.InitializationDrag(allyDrag,Ally,immovable,InitialPointX,InitialPointY);
                InitializationEffect();

                //攻撃矢印アニメーションを終了する
                Ally1Class.SwordEffectStop();
                Ally2Class.SwordEffectStop();
                Ally3Class.SwordEffectStop();
                Ally4Class.SwordEffectStop();
                
                //ドロップが成功した場合の処理
                if (immovable){
                    StartCoroutine(ProcessAfterDrop(arrayBoard));
                }
            }
            //敵駒がセットされていた場合の処理
            else {
                //エフェクトのリセットを行う
                InitializationEffect();
            }
        }
        allyDrag = null;
    }

    //ドロップが終了した後の処理（コルーチン）
    IEnumerator ProcessAfterDrop(int[,] arrayBoard)
    {
        //プレイターターンを終了させる
        PlayerTurn = false;

        //プレイヤーモンスターの攻撃アクション
        yield return StartCoroutine(DirectAttackClass.AllyDirectAttack(arrayBoard));

        //敵ターン開始のエフェクトの発動
        TurnStartClass.StartEnemy();
        yield return new WaitForSeconds(1.5f);

        //敵ターンのギミックを実行
        yield return StartCoroutine(ActionEnemyClass.ActionEnemyTurn(arrayBoard));

        //敵モンスターの攻撃アクション
        Debug.Log("enemy attack!!");

        //プレイヤーターンに切り替えるギミックの実行
        TurnStartClass.StartPlayer();
        yield return new WaitForSeconds(1.5f);

        // yield return null;
    }

    //関数：駒を初期配置に置く
    void PlaceMonsterInitially()
    {
        //①配列の設定（駒が初期配置されるマス目のステータスを変更）
        arrayBoard[1,3] = 1;   // ally1
        PointAlly1[0] = 3;
        PointAlly1[1] = 1;
        arrayBoard[2,3] = 2;  // ally2
        PointAlly2[0] = 3;
        PointAlly2[1] = 2;
        arrayBoard[3,3] = 3;  // ally3
        PointAlly3[0] = 3;
        PointAlly3[1] = 3;
        arrayBoard[3,2] = 4;  // ally4
        PointAlly4[0] = 2;
        PointAlly4[1] = 3;
        arrayBoard[1,0] = 11;  // enemy1
        PointEnemy1[0] = 0;
        PointEnemy1[1] = 1;
        arrayBoard[0,1] = 12;  // enemy2
        PointEnemy2[0] = 1;
        PointEnemy2[1] = 0;
        //②駒オブジェクトに座標を指定
        ally1.transform.localPosition = new Vector3(105, 35, 0);
        ally2.transform.localPosition = new Vector3(105, -35, 0);
        ally3.transform.localPosition = new Vector3(105, -105, 0);
        ally4.transform.localPosition = new Vector3(35, -105, 0);
        enemy1.transform.localPosition = new Vector3(-105, 35, 0);
        enemy2.transform.localPosition = new Vector3(-35, 105, 0);
    }

    //関数：SquareBoxにプレハブをセットする関数
    void PrepareSquareBox()
    {
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
    
    //関数：float型の座標を引数で与えるとどのマス目かを返す関数
    //返り値の型は長さ2の配列（int）
    int[] GetBoardPoint(float mPosX, float mPosY)
    {
        
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

    //関数：動けるマスの範囲の二次元配列を作成とプレハブの非表示
    public int[,] GenerateAvailableSquares(GameObject? allyDrag, int InitialPointX, int InitialPointY, bool Player)
    {
        //飛行可能か判定するbool値
        bool flying = false;

        //Focusを座標にセットする
        FocusParent.SetActive(true);
        FocusParent.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
        //対象のFocusプレハブをアクションさせる
        if (allyDrag == ally1){
            moveAlly = moveAlly1;
            flying = true;
        } else if (allyDrag == ally2){
            moveAlly = moveAlly2;
            flying = false;
        } else if (allyDrag == ally3){
            moveAlly = moveAlly3;
            flying = false;
        } else if (allyDrag == ally4){
            moveAlly = moveAlly4;
            flying = false;
        } else if (allyDrag == enemy1){
            moveAlly = moveEnemy1;
            flying = true;
        } else if (allyDrag == enemy2){
            moveAlly = moveEnemy2;
            flying = false;
        }

        int[,] AvailableSquares = new int[24,2]{
            {10,10},{10,10},{10,10},{10,10},{10,10},{10,10},
            {10,10},{10,10},{10,10},{10,10},{10,10},{10,10},
            {10,10},{10,10},{10,10},{10,10},{10,10},{10,10},
            {10,10},{10,10},{10,10},{10,10},{10,10},{10,10}
        };
        SquareBox[InitialPointY,InitialPointX].SetActive(false);
        //LeftUp
        if (InitialPointY-1 >= 0 && InitialPointX-1 >= 0 && moveAlly[0] != 0){  //そもそも左上矢印のステータスが0だった場合は処理は行われない
            //左上ひとマス目が空か判定しfocusをオンにする
            if (arrayBoard[InitialPointY-1,InitialPointX-1] == 0){
                if(Player){
                    FocusLeftUp.transform.GetChild(0).GetComponent<focus>().FocusMove();
                }else{
                    FocusLeftUp.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                }
                AvailableSquares[0,0] = InitialPointX-1;
                AvailableSquares[0,1] = InitialPointY-1;
                SquareBox[InitialPointY-1,InitialPointX-1].SetActive(false);
            }
            //左上矢印が二重矢印だった場合に呼ばれる処理
            if (moveAlly[0] == 2){
                int squareLeftUp = arrayBoard[InitialPointY-1,InitialPointX-1];
                //左上ひとマス目が空か味方（つまり敵以外）であったかまたは飛行可能であるかの場合のみfocusがオンになる
                if (squareLeftUp==0 || squareLeftUp==1 || squareLeftUp==2 || squareLeftUp==3 || squareLeftUp==4 || flying){
                    if (InitialPointY >= 2 && InitialPointX >= 2 && arrayBoard[InitialPointY-2,InitialPointX-2] == 0){
                        if(Player){
                            FocusLeftUp.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        }else{
                            FocusLeftUp.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                        }
                        AvailableSquares[1,0] = InitialPointX-2;
                        AvailableSquares[1,1] = InitialPointY-2;
                        SquareBox[InitialPointY-2,InitialPointX-2].SetActive(false);
                    }
                    if (InitialPointY >= 3 && InitialPointX >= 3 && arrayBoard[InitialPointY-3,InitialPointX-3] == 0){
                        squareLeftUp = arrayBoard[InitialPointY-2,InitialPointX-2];
                        //左上ふたマス目が空か味方（つまり敵以外）であったかまたは飛行可能であるかの場合のみfocusがオンになる
                        if (squareLeftUp==0 || squareLeftUp==1 || squareLeftUp==2 || squareLeftUp==3 || squareLeftUp==4 || flying){
                            if(Player){
                                FocusLeftUp.transform.GetChild(2).GetComponent<focus>().FocusMove();
                            }else{
                                FocusLeftUp.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                            }
                            AvailableSquares[2,0] = InitialPointX-3;
                            AvailableSquares[2,1] = InitialPointY-3;
                            SquareBox[InitialPointY-3,InitialPointX-3].SetActive(false);
                        }
                    }
                }
            }
        }
        //Left
        if (InitialPointX >= 1 && moveAlly[1] != 0){
            if (arrayBoard[InitialPointY,InitialPointX-1] == 0){
                if(Player){
                    FocusLeft.transform.GetChild(0).GetComponent<focus>().FocusMove();
                }else{
                    FocusLeft.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                }
                AvailableSquares[3,0] = InitialPointX-1;
                AvailableSquares[3,1] = InitialPointY;
                SquareBox[InitialPointY,InitialPointX-1].SetActive(false);
            } 
            if (moveAlly[1] == 2){
                int squareLeft = arrayBoard[InitialPointY,InitialPointX-1];
                if (squareLeft==0 || squareLeft==1 || squareLeft==2 || squareLeft==3 || squareLeft==4 ||flying){
                    if (InitialPointX >= 2 && arrayBoard[InitialPointY,InitialPointX-2] == 0){
                        if(Player){
                            FocusLeft.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        }else{
                            FocusLeft.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                        }
                        AvailableSquares[4,0] = InitialPointX-2;
                        AvailableSquares[4,1] = InitialPointY;
                        SquareBox[InitialPointY,InitialPointX-2].SetActive(false);
                    }
                    if (InitialPointX >= 3 && arrayBoard[InitialPointY,InitialPointX-3] == 0){
                        squareLeft = arrayBoard[InitialPointY,InitialPointX-2];
                        if (squareLeft==0 || squareLeft==1 || squareLeft==2 || squareLeft==3 || squareLeft==4 || flying){
                            if(Player){
                                FocusLeft.transform.GetChild(2).GetComponent<focus>().FocusMove();
                            }else{
                                FocusLeft.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                            }
                            AvailableSquares[5,0] = InitialPointX-3;
                            AvailableSquares[5,1] = InitialPointY;
                            SquareBox[InitialPointY,InitialPointX-3].SetActive(false);
                        }
                    }
                }
            }
        }
        //LeftDown
        if (InitialPointY <= 2 && InitialPointX >= 1 && moveAlly[2] != 0){
            if (arrayBoard[InitialPointY+1,InitialPointX-1] == 0){
                if(Player){
                    FocusLeftDown.transform.GetChild(0).GetComponent<focus>().FocusMove();
                }else{
                    FocusLeftDown.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                }
                AvailableSquares[6,0] = InitialPointX-1;
                AvailableSquares[6,1] = InitialPointY+1;
                SquareBox[InitialPointY+1,InitialPointX-1].SetActive(false);
            }
            if (moveAlly[2] == 2){
                int squareLeftDown = arrayBoard[InitialPointY+1,InitialPointX-1];
                if (squareLeftDown==0 || squareLeftDown==1 || squareLeftDown==2 || squareLeftDown==3 || squareLeftDown==4 || flying){
                    if (InitialPointY <= 1 && InitialPointX >= 2 && arrayBoard[InitialPointY+2,InitialPointX-2] == 0){
                        if(Player){
                            FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        }else{
                            FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                        }
                        AvailableSquares[7,0] = InitialPointX-2;
                        AvailableSquares[7,1] = InitialPointY+2;
                        SquareBox[InitialPointY+2,InitialPointX-2].SetActive(false);
                    }
                    if (InitialPointY <= 0 && InitialPointX >= 3 && arrayBoard[InitialPointY+3,InitialPointX-3] == 0){
                        squareLeftDown = arrayBoard[InitialPointY+2,InitialPointX-2];
                        if (squareLeftDown==0 || squareLeftDown==1 || squareLeftDown==2 || squareLeftDown==3 || squareLeftDown==4 || flying){
                            if(Player){
                                FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                            }else{
                                FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                            }
                            AvailableSquares[8,0] = InitialPointX-3;
                            AvailableSquares[8,1] = InitialPointY+3;
                            SquareBox[InitialPointY+3,InitialPointX-3].SetActive(false);
                        }
                    }
                }
            }
        }
        //Up
        if (InitialPointY >= 1 && moveAlly[3] != 0){
            if (arrayBoard[InitialPointY-1,InitialPointX] == 0){
                if(Player){
                    FocusUp.transform.GetChild(0).GetComponent<focus>().FocusMove();
                }else{
                    FocusUp.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                }
                AvailableSquares[9,0] = InitialPointX;
                AvailableSquares[9,1] = InitialPointY-1;
                SquareBox[InitialPointY-1,InitialPointX].SetActive(false);
            }
            if (moveAlly[3] == 2){
                int squareUp = arrayBoard[InitialPointY-1,InitialPointX];
                if (squareUp==0 || squareUp==1 || squareUp==2 || squareUp==3 || squareUp==4 || flying){
                    if (InitialPointY >= 2 && arrayBoard[InitialPointY-2,InitialPointX] == 0){
                        if(Player){
                            FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        }else{
                            FocusLeftDown.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                        }
                        AvailableSquares[10,0] = InitialPointX;
                        AvailableSquares[10,1] = InitialPointY-2;
                        SquareBox[InitialPointY-2,InitialPointX].SetActive(false);
                    }
                    if (InitialPointY >= 3 && arrayBoard[InitialPointY-3,InitialPointX] == 0){
                        squareUp = arrayBoard[InitialPointY-2,InitialPointX];
                        if (squareUp==0 || squareUp==1 || squareUp==2 || squareUp==3 || squareUp==4 || flying){
                            if(Player){
                                FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                            }else{
                                FocusLeftDown.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                            }
                            AvailableSquares[11,0] = InitialPointX;
                            AvailableSquares[11,1] = InitialPointY-3;
                            SquareBox[InitialPointY-3,InitialPointX].SetActive(false);
                        }
                    }
                }
            }
        }
        //Down
        if (InitialPointY <= 2 && moveAlly[4] != 0){
            if (arrayBoard[InitialPointY+1,InitialPointX] == 0){
                if(Player){
                    FocusDown.transform.GetChild(0).GetComponent<focus>().FocusMove();
                }else{
                    FocusDown.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                }
                AvailableSquares[12,0] = InitialPointX;
                AvailableSquares[12,1] = InitialPointY+1;
                SquareBox[InitialPointY+1,InitialPointX].SetActive(false);
            }
            if (moveAlly[4] == 2){
                int squareDown = arrayBoard[InitialPointY+1,InitialPointX];
                if (squareDown==0 || squareDown==1 || squareDown==2 || squareDown==3 || squareDown==4 || flying){
                    if (InitialPointY <= 1 && arrayBoard[InitialPointY+2,InitialPointX] == 0){
                        if(Player){
                            FocusDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        }else{
                            FocusDown.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                        }
                        AvailableSquares[13,0] = InitialPointX;
                        AvailableSquares[13,1] = InitialPointY+2;
                        SquareBox[InitialPointY+2,InitialPointX].SetActive(false);
                    }
                    if (InitialPointY <= 0 && arrayBoard[InitialPointY+3,InitialPointX] == 0){
                        squareDown = arrayBoard[InitialPointY+2,InitialPointX];
                        if (squareDown==0 || squareDown==1 || squareDown==2 || squareDown==3 || squareDown==4 || flying){
                            if(Player){
                                FocusDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                            }else{
                                FocusDown.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                            }
                            AvailableSquares[14,0] = InitialPointX;
                            AvailableSquares[14,1] = InitialPointY+3;
                            SquareBox[InitialPointY+3,InitialPointX].SetActive(false);
                        }
                    }
                }
            }
        }
        //RightUp
        if (InitialPointY >= 1 && InitialPointX <= 2 && moveAlly[5] != 0){
            if (arrayBoard[InitialPointY-1,InitialPointX+1] == 0){
                if(Player){
                    FocusRightUp.transform.GetChild(0).GetComponent<focus>().FocusMove();
                }else{
                    FocusRightUp.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                }
                AvailableSquares[15,0] = InitialPointX+1;
                AvailableSquares[15,1] = InitialPointY-1;
                SquareBox[InitialPointY-1,InitialPointX+1].SetActive(false);
            }
            if (moveAlly[5] == 2){
                int squareRightUp = arrayBoard[InitialPointY-1,InitialPointX+1];
                if (squareRightUp==0 || squareRightUp==1 || squareRightUp==2 || squareRightUp==3 || squareRightUp==4 || flying){
                    if (InitialPointY >= 2 && InitialPointX <= 1 && arrayBoard[InitialPointY-2,InitialPointX+2] == 0){
                        if(Player){
                            FocusRightUp.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        }else{
                            FocusRightUp.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                        }
                        AvailableSquares[16,0] = InitialPointX+2;
                        AvailableSquares[16,1] = InitialPointY-2;
                        SquareBox[InitialPointY-2,InitialPointX+2].SetActive(false);
                    }
                    if (InitialPointY >= 3 && InitialPointX <= 0 && arrayBoard[InitialPointY-3,InitialPointX+3] == 0){
                        squareRightUp = arrayBoard[InitialPointY-2,InitialPointX+2];
                        if (squareRightUp==0 || squareRightUp==1 || squareRightUp==2 || squareRightUp==3 || squareRightUp==4 || flying){
                            if(Player){
                                FocusRightUp.transform.GetChild(2).GetComponent<focus>().FocusMove();
                            }else{
                                FocusRightUp.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                            }
                            AvailableSquares[17,0] = InitialPointX+3;
                            AvailableSquares[17,1] = InitialPointY-3;
                            SquareBox[InitialPointY-3,InitialPointX+3].SetActive(false);
                        }
                    }
                }
            }
        }
        //Right
        if (InitialPointX <= 2 && moveAlly[6] != 0){
            if (arrayBoard[InitialPointY,InitialPointX+1] == 0){
                if(Player){
                    FocusRight.transform.GetChild(0).GetComponent<focus>().FocusMove();
                }else{
                    FocusRight.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                }
                AvailableSquares[18,0] = InitialPointX+1;
                AvailableSquares[18,1] = InitialPointY;
                SquareBox[InitialPointY,InitialPointX+1].SetActive(false);
            }
            if (moveAlly[6] == 2){
                int squareRight = arrayBoard[InitialPointY,InitialPointX+1];
                if (squareRight==0 || squareRight==1 || squareRight==2 || squareRight==3 || squareRight==4 || flying){
                    if (InitialPointX <= 1 && arrayBoard[InitialPointY,InitialPointX+2] == 0){
                        if(Player){
                            FocusRight.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        }else{
                            FocusRight.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                        }
                        AvailableSquares[19,0] = InitialPointX+2;
                        AvailableSquares[19,1] = InitialPointY;
                        SquareBox[InitialPointY,InitialPointX+2].SetActive(false);
                    }
                    if (InitialPointX <= 0 && arrayBoard[InitialPointY,InitialPointX+3] == 0){
                        squareRight = arrayBoard[InitialPointY,InitialPointX+2];
                        if (squareRight==0 || squareRight==1 || squareRight==2 || squareRight==3 || squareRight==4 || flying){
                        if(Player){
                                FocusRight.transform.GetChild(2).GetComponent<focus>().FocusMove();
                            }else{
                                FocusRight.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                            }
                            AvailableSquares[20,0] = InitialPointX+3;
                            AvailableSquares[20,1] = InitialPointY;
                            SquareBox[InitialPointY,InitialPointX+3].SetActive(false);
                        }
                    }
                }
            }
        }
        //RightDown
        if (InitialPointY <= 2 && InitialPointX <= 2 && moveAlly[7] != 0){
            if (arrayBoard[InitialPointY+1,InitialPointX+1] == 0){
                if(Player){
                    FocusRightDown.transform.GetChild(0).GetComponent<focus>().FocusMove();
                }else{
                    FocusRightDown.transform.GetChild(0).GetComponent<focus>().FocusEnemyMove();
                }
                AvailableSquares[21,0] = InitialPointX+1;
                AvailableSquares[21,1] = InitialPointY+1;
                SquareBox[InitialPointY+1,InitialPointX+1].SetActive(false);
            }
            if (moveAlly[7] == 2){
                int squareRightDown = arrayBoard[InitialPointY+1,InitialPointX+1];
                if (squareRightDown==0 || squareRightDown==1 || squareRightDown==2 || squareRightDown==3 || squareRightDown==4 || flying){
                    if (InitialPointY <= 1 && InitialPointX <= 1 && arrayBoard[InitialPointY+2,InitialPointX+2] == 0){
                        if(Player){
                            FocusRightDown.transform.GetChild(1).GetComponent<focus>().FocusMove();
                        }else{
                            FocusRightDown.transform.GetChild(1).GetComponent<focus>().FocusEnemyMove();
                        }
                        AvailableSquares[22,0] = InitialPointX+2;
                        AvailableSquares[22,1] = InitialPointY+2;
                        SquareBox[InitialPointY+2,InitialPointX+2].SetActive(false);
                    }
                    if (InitialPointY <= 0 && InitialPointX <= 0 && arrayBoard[InitialPointY+3,InitialPointX+3] == 0){
                        squareRightDown = arrayBoard[InitialPointY+2,InitialPointX+2];
                        if (squareRightDown==0 || squareRightDown==1 || squareRightDown==2 || squareRightDown==3 || squareRightDown==4 || flying){
                            if(Player){
                                FocusRightDown.transform.GetChild(2).GetComponent<focus>().FocusMove();
                            }else{
                                FocusRightDown.transform.GetChild(2).GetComponent<focus>().FocusEnemyMove();
                            }
                            AvailableSquares[23,0] = InitialPointX+3;
                            AvailableSquares[23,1] = InitialPointY+3;
                            SquareBox[InitialPointY+3,InitialPointX+3].SetActive(false);
                        }
                    }
                }
            }
        }
        return AvailableSquares;
    }

    //DragObjectNumを書き換える関数
    public void UpdateDragObjectNum(int Num)
    {
        DragObjectNum = Num;
    }

    //arrayBoardを書き換える関数
    public void UpdateArrayBoard(int PointX, int PointY, int ObjectNum)
    {
        arrayBoard[PointY,PointX] = ObjectNum;
    }

    //モンスターの座標を書き換える関数
    public void UpdateMonsterPoint(int PointX, int PointY, int Num)
    {
        switch (Num) {
            case 1:
                PointAlly1[0] = PointX;
                PointAlly1[1] = PointY;
                break;
            case 2:
                PointAlly2[0] = PointX;
                PointAlly2[1] = PointY;
                break;
            case 3:
                PointAlly3[0] = PointX;
                PointAlly3[1] = PointY;
                break;
            case 4:
                PointAlly4[0] = PointX;
                PointAlly4[1] = PointY;
                break;
            case 11:
                PointEnemy1[0] = PointX;
                PointEnemy1[1] = PointY;
                break;
            case 12:
                PointEnemy2[0] = PointX;
                PointEnemy2[1] = PointY;
                break;
        }
    }

    //関数：フォーカスとスクエアのエフェクトを初期化する
    public void InitializationEffect()
    {
        //focusの解除
        FocusParent.SetActive(false);

        //Squareを元に戻す
        for (int i = 0; i < 4; i++){
            for (int j = 0; j < 4; j++){
                SquareBox[i,j].SetActive(true);
            }
        }
    }

    //関数：プレイヤーターンを切り替える
    public void TurnPlayerTurn()
    {
        PlayerTurn = true;
    }

    //関数：モンスターのHPが0になった時の処理
    public void ExtinguishMonster(int ObjectNum)
    {
        switch(ObjectNum){
            case 1:
                ally1.SetActive(false);
                arrayBoard[PointAlly1[1],PointAlly1[0]] = 0;
                break;
            case 2:
                ally2.SetActive(false);
                arrayBoard[PointAlly2[1],PointAlly2[0]] = 0;
                break;
            case 3:
                ally3.SetActive(false);
                arrayBoard[PointAlly3[1],PointAlly3[0]] = 0;
                break;
            case 4:
                ally4.SetActive(false);
                arrayBoard[PointAlly4[1],PointAlly4[0]] = 0;
                break;
            case 11:
                enemy1.SetActive(false);
                arrayBoard[PointEnemy1[1],PointEnemy1[0]] = 0;
                break;
            case 12:
                enemy2.SetActive(false);
                arrayBoard[PointEnemy2[1],PointEnemy2[0]] = 0;
                break;
        }
    }
}
