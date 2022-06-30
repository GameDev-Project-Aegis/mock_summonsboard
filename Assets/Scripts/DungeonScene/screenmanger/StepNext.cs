using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepNext : MonoBehaviour
{
    Animator StepNextClass;
    public GameObject areaDisplay;

    public GameObject board;
    Animator boardClass;

    public GameObject ally1;
    Animator ally1Class;
    public GameObject ally2;
    Animator ally2Class;
    public GameObject ally3;
    Animator ally3Class;
    public GameObject ally4;
    Animator ally4Class;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject smoke_target3;
    public GameObject smoke_target4;

    void Start()
    {
        StepNextClass = GetComponent<Animator>();
        boardClass = board.GetComponent<Animator>();

        ally1Class = ally1.GetComponent<Animator>();
        ally2Class = ally2.GetComponent<Animator>();
        ally3Class = ally3.GetComponent<Animator>();
        ally4Class = ally4.GetComponent<Animator>();
    }

    public IEnumerator StepNextArea()
    {
        areaDisplay.SetActive(true);

        StepNextClass.SetTrigger("step2area");
        boardClass.SetTrigger("Step");

        ally1Class.SetTrigger("NextStep");
        ally2Class.SetTrigger("NextStep");
        ally3Class.SetTrigger("NextStep");
        ally4Class.SetTrigger("NextStep");

        yield return new WaitForSeconds(2);

        areaDisplay.SetActive(false);

        StartCoroutine(EnemyAppear());
    }

    // 敵を画面内に登場させるコルーチン
    IEnumerator EnemyAppear()
    {
        // 敵を表示
        enemy3.SetActive(true);
        smoke_target3.GetComponent<ParticleSystem>().Play();  // 砂埃のパーティクルエフェクトを再生

        yield return new WaitForSeconds(0.2f);

        enemy4.SetActive(true);
        smoke_target4.GetComponent<ParticleSystem>().Play();
    }
}
