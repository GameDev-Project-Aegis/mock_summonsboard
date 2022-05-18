using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSurface : MonoBehaviour
{
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

    
    // Start is called before the first frame update
    void Start()
    {
        //マウスポイントから座標を取得するための準備
        mainCamera = Camera.main;
        PosZ = ally1.transform.position.z;

        //駒を初期配置に置く
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

    // クリック時
    void OnMouseDown()
    {
        //①タップしたマスに味方駒があるか判定
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        PointArray = GetBoardPoint(mousePos.x, mousePos.y); //GetBoardPoint()は下で定義してるよ！
        InitialPointX = PointArray[0];
        InitialPointY = PointArray[1];

        //②味方駒がいた場合にallyDragにその駒のオブジェクトをセットする
        //transform.SetAsLastSibling()でヒエラルキー内の順序を変更し，一番手前に表示
        if (arrayBoard[InitialPointY,InitialPointX] == 1) {
            allyDrag = ally1;
            DragObjectNum = 1;
            ally1.transform.SetAsLastSibling();
        }else if (arrayBoard[InitialPointY,InitialPointX] == 2) {
            allyDrag = ally2;
            DragObjectNum = 2;
            ally2.transform.SetAsLastSibling();
        }else if (arrayBoard[InitialPointY,InitialPointX] == 3) {
            allyDrag = ally3;
            DragObjectNum = 3;
            ally3.transform.SetAsLastSibling();
        }else if (arrayBoard[InitialPointY,InitialPointX] == 4) {
            allyDrag = ally4;
            DragObjectNum = 4;
            ally4.transform.SetAsLastSibling();
        }
    }

    // ドラッグ時
    void OnMouseDrag()
    {
        //allyDragに駒オブジェクトがセットされてる場合のみ実行
        if (allyDrag != null) {
            //マウスから取得した座標をallyDragオブジェクトに入れる
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = PosZ;
            allyDrag.transform.position = mousePos;
        }
    }

    // ドロップ時
    void OnMouseUp()
    {
        //allyDragに駒オブジェクトがセットされてる場合のみ実行
        if (allyDrag != null) {
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

            //allyDragを空に戻して置く
            allyDrag = null;
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
    
}
