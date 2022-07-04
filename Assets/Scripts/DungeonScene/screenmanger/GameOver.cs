using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject mainCamera;

    public GameObject Gameover;
    Animator GameoverAnimator;
    public GameObject Continue;
    Animator ContinueAnimator;
    public GameObject Retire1;
    Animator Retire1Animator;
    public GameObject Retire2;
    Animator Retire2Animator;

    void Start()
    {
        GameoverAnimator = Gameover.GetComponent<Animator>();
        ContinueAnimator = Continue.GetComponent<Animator>();
        Retire1Animator = Retire1.GetComponent<Animator>();
        Retire2Animator = Retire2.GetComponent<Animator>();

        StartCoroutine(AppearGameover());
    }

    IEnumerator AppearGameover()
    {
        yield return new WaitForSeconds(1.5f);
        Gameover.SetActive(true);
        Gameover.transform.SetAsLastSibling();
    }

    public void TapOkGameover()
    {
        StartCoroutine(ShrinkGameover());
        StartCoroutine(SwellContinue());
    }

    public void TapNoGameover()
    {
        StartCoroutine(ShrinkGameover());
        StartCoroutine(SwellRetire1());
    }

    public void TapOkContinue()
    {
        //
    }

    public void TapNoContinue()
    {
        StartCoroutine(ShrinkContinue());
        StartCoroutine(SwellGameover());
    }

    public void TapOkRetire1()
    {
        StartCoroutine(ShrinkRetire1());
        StartCoroutine(SwellRetire2());
    }

    public void TapNoRetire1()
    {
        StartCoroutine(ShrinkRetire1());
        StartCoroutine(SwellGameover());
    }

    public void TapRetire2()
    {
        StartCoroutine(ShrinkRetire2());
        StartCoroutine(SceneChange());
    }

    IEnumerator SwellGameover()
    {
        yield return new WaitForSeconds(0.5f);
        Gameover.SetActive(true);
        Gameover.transform.SetAsLastSibling();
    }

    IEnumerator ShrinkGameover()
    {
        GameoverAnimator.SetTrigger("shrink");
        yield return new WaitForSeconds(0.5f);
        Gameover.SetActive(false);
    }

    IEnumerator SwellContinue()
    {
        yield return new WaitForSeconds(0.5f);
        Continue.SetActive(true);
        Continue.transform.SetAsLastSibling();
    }

    IEnumerator ShrinkContinue()
    {
        ContinueAnimator.SetTrigger("shrink");
        yield return new WaitForSeconds(0.5f);
        Continue.SetActive(false);
    }

    IEnumerator SwellRetire1()
    {
        yield return new WaitForSeconds(0.5f);
        Retire1.SetActive(true);
        Retire1.transform.SetAsLastSibling();
    }

    IEnumerator ShrinkRetire1()
    {
        Retire1Animator.SetTrigger("shrink");
        yield return new WaitForSeconds(0.5f);
        Retire1.SetActive(false);
    }

    IEnumerator SwellRetire2()
    {
        yield return new WaitForSeconds(0.5f);
        Retire2.SetActive(true);
        Retire2.transform.SetAsLastSibling();
    }

    IEnumerator ShrinkRetire2()
    {
        Retire2Animator.SetTrigger("shrink");
        yield return new WaitForSeconds(0.5f);
        Retire2.SetActive(false);
    }

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetTrigger("crumble");
        mainCamera.GetComponent<SceneController>().sceneChange();
    }
}
