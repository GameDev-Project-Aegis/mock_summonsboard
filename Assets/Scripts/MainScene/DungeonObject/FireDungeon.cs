using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDungeon : MonoBehaviour
{
    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    public void OnClick(){
        gameManager.GetComponent<GameManager>().TapFireDungeon();
    }
}
