using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 盤面の初期値を設定
        void Initialize()
        {
            // オブジェクトを探す
            GameObject ally1 = this.transform.Find("allyParent/ally (1)").gameObject;
            // GameObject ally2 = GameObject.Find ("ally (2)");
            // GameObject ally3 = GameObject.Find ("ally (3)");
            // GameObject ally4 = GameObject.Find ("ally (4)");
            // GameObject enemy1 = GameObject.Find ("enemy (1)");
            // GameObject enemy2 = GameObject.Find ("enemy (2)");

            //オブジェクトの初期位置を設定
            ally1.transform.localPosition = new Vector3(105, 35, 0);
        }
}
