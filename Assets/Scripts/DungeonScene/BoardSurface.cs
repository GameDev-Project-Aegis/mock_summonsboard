using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSurface : MonoBehaviour
{
    public GameObject ally1;
    public GameObject ally2;
    public GameObject ally3;
    public GameObject ally4;
    public GameObject enemy1;
    public GameObject enemy2;

    private Camera mainCamera;
    private float PosZ;

    //盤面用の配列
    //16行3列の配列を定義
    //1列目->x位置(0-3)
    //2列目->y位置(0-3)
    //3列目->そのマス目のステータス(0:空, 1:ally1, 2:ally2, 3:ally3, 4:ally4, 5:enemy1, ... )
    int[,] arrayBoard = new int[16, 3]{
        {0,0,0},
        {0,1,0},
        {0,2,0},
        {0,3,0},
        {1,0,0},
        {1,1,0},
        {1,2,0},
        {1,3,0},
        {2,0,0},
        {2,1,0},
        {2,2,0},
        {2,3,0},
        {3,0,0},
        {3,1,0},
        {3,2,0},
        {3,3,0}
    };

    
    // Start is called before the first frame update
    void Start()
    {
        //マウスポイントから座標を取得するための準備
        mainCamera = Camera.main;
        PosZ = ally1.transform.position.z;

        //駒を初期配置に置く
        //①配列の設定（駒が初期配置されるマス目のステータスを変更）
        arrayBoard[7,2] = 1;   // ally1
        arrayBoard[11,2] = 2;  // ally2
        arrayBoard[15,2] = 3;  // ally3
        arrayBoard[14,2] = 4;  // ally4
        arrayBoard[2,2] = 5;  // enemy1
        arrayBoard[4,2] = 5;  // enemy2
        //②駒オブジェクトに座標を指定
        ally1.transform.localPosition = new Vector3(105, 35, 0);
        ally2.transform.localPosition = new Vector3(105, -35, 0);
        ally3.transform.localPosition = new Vector3(105, -105, 0);
        ally4.transform.localPosition = new Vector3(35, -105, 0);
        enemy1.transform.localPosition = new Vector3(-35, 105, 0);
        enemy2.transform.localPosition = new Vector3(-105, 35, 0);
    }

    // クリック時
    void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        Debug.Log(arrayBoard[6,2]);
        Debug.Log(arrayBoard[7,2]);
    }

    // ドラッグ時
    void OnMouseDrag()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = PosZ;
        ally1.transform.position = mousePos;
    }

    // ドロップ時
    void OnMouseUp()
    {
        //
    }
    
}
