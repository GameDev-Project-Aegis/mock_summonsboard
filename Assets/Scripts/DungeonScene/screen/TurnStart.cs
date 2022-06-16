using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnStart : MonoBehaviour
{
    public GameObject turn_circle1;
    public GameObject turn_circle2;
    public GameObject turn_circle3;
    public GameObject imagePlayer;
    public GameObject imageEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 青い円陣と"Player"の文字を表示する
    public void StartPlayer()
    {
        // 各アニメーションの呼び出し
        turn_circle1.GetComponent<Animator>().SetTrigger("RotatePlayer");
        turn_circle2.GetComponent<Animator>().SetTrigger("RotatePlayer");
        turn_circle3.GetComponent<Animator>().SetTrigger("RotatePlayer");
        imagePlayer.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    // 赤い円陣と"Enemy"の文字を表示する
    public void StartEnemy()
    {
        // 各アニメーションの呼び出し
        turn_circle1.GetComponent<Animator>().SetTrigger("RotateEnemy");
        turn_circle2.GetComponent<Animator>().SetTrigger("RotateEnemy");
        turn_circle3.GetComponent<Animator>().SetTrigger("RotateEnemy");
        imageEnemy.GetComponent<Animator>().SetTrigger("FadeIn");
    }
}