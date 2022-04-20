using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button_enter : MonoBehaviour
{
    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }
    public void OnTap(){
        gameManager.GetComponent<GameManager>().TapEnterDungeon();
    }
}
