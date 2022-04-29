using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ok_button : MonoBehaviour
{
    public GameObject mainCamera;

    public void OnTap(){
        mainCamera.GetComponent<SceneController>().sceneChange();
    }
}
