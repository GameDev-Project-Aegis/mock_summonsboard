using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag1 : MonoBehaviour
{
    private float constantZ;                     //ドラッグ時のZ座標
    private Vector3 pos;                         //味方の座標


    private const int pitch = 35;               //フィールドのマス目のピッチ

    //属性
    enum MONSTER {
        EMPTY,  //空欄 = 0
        ALLY,   //味方 = 1
        ENEMY   //敵 = 2
    }
    
    [SerializeField]
    GameObject emptyParent = null;              //空欄
    [SerializeField]
    GameObject allyParent = null;               //味方Parent
    [SerializeField]
    GameObject enemyParent = null;              //敵Parent
    [SerializeField]
    GameObject boardDisplay = null;             //盤面フィールド
    
    const int WIDTH = 4;
    const int HEIGHT = 4;

    MONSTER[,] board = new MONSTER[WIDTH, HEIGHT];  //4*4配列を宣言


    // Start is called before the first frame update
    void Start()
    {
        constantZ = transform.position.z;

        Initialize(); // 盤面の初期値を設定
        ShowBoard();  // 盤面を表示
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 盤面の初期値を設定
    void Initialize()
    {
        // オブジェクトを探す
        GameObject ally1 = allyParent.transform.Find("ally (1)").gameObject;
        // GameObject ally2 = GameObject.Find ("ally (2)");
        // GameObject ally3 = GameObject.Find ("ally (3)");
        // GameObject ally4 = GameObject.Find ("ally (4)");
        // GameObject enemy1 = GameObject.Find ("enemy (1)");
        // GameObject enemy2 = GameObject.Find ("enemy (2)");

        //オブジェクトの初期位置を設定
        // ally1.transform.localPosition = new Vector3(105, 35, 0);

        pos.x = 105;
        pos.y = 35;

        //取得したマスの座標を配列に代入
        int[] a = PosNo(transform.localPosition.x, transform.localPosition.y);
        int h = a[0];
        int v = a[1];

        board[h, v] = MONSTER.ALLY;

        board[3, 2] = MONSTER.ALLY;   //ally(2)を配置
        board[3, 3] = MONSTER.ALLY;   //ally(3)を配置
        board[2, 3] = MONSTER.ALLY;   //ally(4)を配置
        board[1, 0] = MONSTER.ENEMY;  //enemy(1)を配置
        board[0, 1] = MONSTER.ENEMY;  //enemy(2)を配置
    }

    // 盤面を表示
    void ShowBoard() {
        // boardDisplayの全ての子オブジェクトを味方Parentに移行
        foreach (Transform child in boardDisplay.transform)
        {
            transform.SetParent(allyParent.transform, false);
        }
    }

    //クリック時の処理
    private void OnMouseDown()
    {
        //取得したマスの座標を配列に代入
        int[] a = PosNo(transform.localPosition.x, transform.localPosition.y);
        int h = a[0];
        int v = a[1];

        //元いたマスを空欄にする
        board[h, v] = MONSTER.EMPTY;
    }

    //ドラッグ時の処理
    private void OnMouseDrag()
    {
        //味方Parentの子なら盤面フィールドに移行する
        if (transform.parent.gameObject == allyParent) {
            transform.SetParent(boardDisplay.transform, false);
        }

        //マウスポイントの取得と座標代入
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = constantZ;
        transform.position = mousePos;
        transform.SetSiblingIndex(99);  //一番手前に表示
    }

    //ドロップ時の処理
    private void OnMouseUp()
    {
        //取得したマスの座標を配列に代入
        int[] a = PosNo(transform.localPosition.x, transform.localPosition.y);
        int h = a[0];
        int v = a[1];
        
        //空欄にのみ配置可能
        if (board[h, v] == MONSTER.EMPTY) {
            pos.x = h * 2 * pitch - 105;     //自身のx座標
            pos.y = -(v * 2 * pitch - 105);  //自身のy座標
        }

        //コマを配置
        this.transform.localPosition = pos;
        this.transform.SetSiblingIndex(0);

        //今いるマスを味方にする（機能していない？）
        board[h, v] = MONSTER.ALLY;
    }

    //ドロップ時のマスの座標を取得
    private int[] PosNo(float xaxis, float yaxis)
    {
        float xdec = xaxis / pitch;
        float ydec = yaxis / pitch;
        int xsign = (int)(Mathf.Abs(xaxis) / xaxis);  //-1か+1を返す
        int ysign = (int)(-Mathf.Abs(yaxis) / yaxis);  //+1か-1を返す
        int xconf = 0;
        int yconf = 0;
        
        if  (xdec <= 2 && xdec >= -2) {
            xconf = (xsign + 3) / 2 ;  //(-1,1) -> (1,2)
        }
        else {
            xconf = (xsign + 1) / 2 * 3 ;  //(-1,1) -> (0,3)
        }

        if  (ydec <= 2 && ydec >= -2) {
            yconf = (ysign + 3) / 2 ;  //(1,-1) -> (2,1)
        }
        else {
            yconf = (ysign + 1) / 2 * 3 ;  //(1,-1) -> (3,0)
        }
        
        int[] conf = new int[2];
        conf[0] = xconf;  //左から0, 1, 2, 3
        conf[1] = yconf;  //上から0, 1, 2, 3

        return conf;
    }
}
