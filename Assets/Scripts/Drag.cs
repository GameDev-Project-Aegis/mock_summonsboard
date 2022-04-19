using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public GameObject allyParent;              //味方Parent
    public GameObject SetField;                  //盤面フィールド

    private float constantZ;                     //ドラッグ時のZ座標
    private Vector3 pos;                         //味方の座標


    //盤面フィールドデータ
    private const int pitch = 35;               //フィールドのマス目のピッチ
    private const int Xmax = 105;                //フィールドのX座標上限
    private const int Xmin = -105;               //フィールドのX座標下限
    private const int Ymax = 105;                //フィールドのY座標上限
    private const int Ymin = -105;               //フィールドのY座標下限

    // Start is called before the first frame update
    void Start()
    {
        constantZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ドラッグ時の処理
    private void OnMouseDrag()
    {
        //味方Parentの子なら盤面フィールドに移行する
        if (transform.parent.gameObject == allyParent)
            transform.SetParent(SetField.transform, false);

        //マウスポイントの取得と座標代入
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = constantZ;
        transform.position = mousePos;
        transform.SetSiblingIndex(99);
    }

    //ドロップ時の処理
    private void OnMouseUp()
    {
        //盤面フィールド内のマス目座標に修正する
        pos.x = Mathf.Clamp(Rounding(transform.localPosition.x), Xmin, Xmax);
        pos.y = Mathf.Clamp(Rounding(transform.localPosition.y), Ymin, Ymax);

        //座標代入
        this.transform.localPosition = pos;
        this.transform.SetSiblingIndex(0);
    }

    //ドロップ時の座標
    private int Rounding(float axis)
    {
        float dec = axis / pitch;
        int sign = (int)(axis / Mathf.Abs(axis));
        int conf = 0;

        if  (dec <= 2 && dec >= -2) {
            conf = sign * pitch;
        }
        else {
            conf = sign * pitch * 3;
        }
        return conf;
    }
}
