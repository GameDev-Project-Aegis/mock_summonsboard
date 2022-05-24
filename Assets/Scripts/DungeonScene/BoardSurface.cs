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

        //Squareを配列に入れる
        SquareBox[0] = square01;
        SquareBox[1] = square02;
        SquareBox[2] = square03;
        SquareBox[3] = square04;
        SquareBox[4] = square05;
        SquareBox[5] = square06;
        SquareBox[6] = square07;
        SquareBox[7] = square08;
        SquareBox[8] = square09;
        SquareBox[9] = square10;
        SquareBox[10] = square11;
        SquareBox[11] = square12;
        SquareBox[12] = square13;
        SquareBox[13] = square14;
        SquareBox[14] = square15;
        SquareBox[15] = square16;
    }

    // クリック時
    void OnMouseDown()
    {
        //1.タップした先のモンスターを返す関数（いない場合はnullを返す）
        allyDrag = MonsterOnTaped();

        if (allyDrag != null){
            //③味方駒のcanvasを半透明にする
            Ally.alpha = 0.6f;
            //④初期配置の場所のSquareのHighLight(originally)を表示する
            boardNum = arrayBoardNum[InitialPointY,InitialPointX];
            Debug.Log(boardNum);
            // Square = Board.transform.GetChild(boardNum).gameObject;
            Square = SquareBox[boardNum - 1];
            if(Square != null){
                Square.transform.Find("HighLight").GetComponent<Animator>().SetBool("drag",true);
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
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            PointArray = GetBoardPoint(mousePos.x, mousePos.y);
            PointX = PointArray[0];
            PointY = PointArray[1];
            boardNum = arrayBoardNum[PointY,PointX];
            //Squareが前フレームと変わっている場合
            if (Square != SquareBox[boardNum - 1]){
                Square.transform.Find("HighLight").GetComponent<Animator>().SetTrigger("missDist");
                SquareBox[boardNum - 1].transform.Find("HighLight").GetComponent<Animator>().SetTrigger("enterDist");
                Square = SquareBox[boardNum - 1];
            }
        }
    }

    // ドロップ時
    void OnMouseUp()
    {
        //allyDragに駒オブジェクトがセットされてる場合のみ実行
        if (allyDrag != null) {
            //1.モンスターをドロップしたときの処理を行う関数
            DropMonster();
        }
        //HighLightを解除する
        if(Square != null){
            Debug.Log("解除");
            Square.transform.Find("HighLight").GetComponent<Animator>().SetBool("drag",false);
            Square.transform.Find("HighLight").GetComponent<Animator>().SetTrigger("drop");
            Square = null;
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
        //①座標からマスを導出
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        PointArray = GetBoardPoint(mousePos.x, mousePos.y);
        PointX = PointArray[0];
        PointY = PointArray[1];

        //②マスに駒が置かれていないかを判別
        if (arrayBoard[PointY,PointX] == 0) {
            //駒オブジェクトのpositionの書き換え
            //ドロップ時のマウスの位置するマスに対応したローカル座標を配列PointValueX(Y)Arrayで取得している
            mousePos.z = PosZ;
            allyDrag.transform.localPosition = new Vector3(PointValueXArray[PointX], PointValueYArray[PointY], 0);
            //配列arrayBoardの書き換え
            arrayBoard[InitialPointY,InitialPointX] = 0;    //元いたマスのステータスを0にする
            arrayBoard[PointY,PointX] = DragObjectNum;      //移動した先のマスのステータスを駒の番号にする
        }else { // ↓ドロップした先に他の駒がいた場合の処理
            //駒を初期位置に戻す
            allyDrag.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
        }

        //③allyDragを空に戻して置く
        allyDrag = null;

        //④alluDragがセットされた場合に味方駒のcanvasを半透明にする
        Ally.alpha = 1.0f;
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
    private GameObject? Square;
    //子オブジェクトとしてプレハブを取得するためのBoardオブジェクトをアタッチ
    public GameObject Board;

    //プレハブを全て予めアタッチしておく
    public GameObject square01;
    public GameObject square02;
    public GameObject square03;
    public GameObject square04;
    public GameObject square05;
    public GameObject square06;
    public GameObject square07;
    public GameObject square08;
    public GameObject square09;
    public GameObject square10;
    public GameObject square11;
    public GameObject square12;
    public GameObject square13;
    public GameObject square14;
    public GameObject square15;
    public GameObject square16;

    //アタッチしたオブジェクトを配列とする
    GameObject[] SquareBox = new GameObject[16];

    //ドラッグ時に味方駒を半透明とするためのAllyをアタッチしておく
    public CanvasGroup Ally;

    // //各モンスターの動く範囲のマスを配列としておく
    // int[,] arrayAlly1 = {
    //     //左上矢印
    //     {-1,1},
    //     //左矢印
    //     {-3,0},
    //     {-2,0},
    //     {-1,0},
    //     //左下矢印
    //     {-1,-1},
    //     //右上矢印
    //     {1,1},
    //     {2,2},
    //     {3,3},
    //     //右矢印
    //     {1,0},
    //     {2,0},
    //     {3,0},
    //     //右下矢印
    //     {1,-1},
    //     {2,-2},
    //     {3,-3}
    // };
    // int[,] arrayAlly2 = {
    //     //左上矢印
    //     {-1,1},
    //     //左矢印
    //     {-3,0},
    //     {-2,0},
    //     {-1,0},
    //     //左下矢印
    //     {-1,-1},
    //     //上矢印
    //     {0,1},
    //     //右上矢印
    //     {1,1},
    //     //右矢印
    //     {1,0},
    //     {2,0},
    //     {3,0},
    //     //右下矢印
    //     {1,-1}
    // };
    // int[,] arrayAlly3 = {
    //     //左上矢印
    //     {-1,1},
    //     //左矢印
    //     {-1,0},
    //     //左下矢印
    //     {-1,-1},
    //     //右上矢印
    //     {1,1},
    //     //右矢印
    //     {1,0},
    //     //右下矢印
    //     {1,-1}
    // };

    // int[,] arrayAlly4 = {
    //     //左上矢印
    //     {-1,1},
    //     //左矢印
    //     {-3,0},
    //     {-2,0},
    //     {-1,0},
    //     //上矢印
    //     {0,1},
    //     //下矢印
    //     {0,-1},
    //     //右矢印
    //     {1,0},
    //     {2,0},
    //     {3,0}
    // };

}
