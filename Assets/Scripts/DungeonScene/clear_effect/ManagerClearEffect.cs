using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerClearEffect : MonoBehaviour
{
    public GameObject ResultEffect;

    void Start()
    {
        Invoke("Result", 3.0f);
    }

    void Result(){
        ResultEffect.SetActive(true);
    }
}
