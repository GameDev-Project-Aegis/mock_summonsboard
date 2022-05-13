using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSurface : MonoBehaviour
{
    public GameObject ally1;
    private Camera mainCamera;
    private float PosZ;        
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        PosZ = ally1.transform.position.z;
    }

    // クリック時
    void OnMouseDown()
    {
        Debug.Log("Mouse Down");
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
