using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindow : MonoBehaviour
{
    public GameObject ally1_skillwindow;
    public GameObject ally2_skillwindow;
    public GameObject ally3_skillwindow;
    public GameObject ally4_skillwindow;
    public GameObject ally1_window;
    public GameObject ally2_window;
    public GameObject ally3_window;
    public GameObject ally4_window;
    // スキル説明文を表示するときに一番左に移動するウィンドウを「サブウィンドウ」と名付ける
    public GameObject ally2_subwindow;
    public GameObject ally3_subwindow;
    public GameObject ally4_subwindow;
    public GameObject cancel_a;
    public GameObject cancel_b;


    // ally1のウィンドウを押された時の処理
    public void Click1()
    {
        // ally1のスキル説明文を表示
        ally1_skillwindow.GetComponent<Animator>().SetTrigger("Appear");
        // その他のallyのウィンドウを画面外に動かす
        ally2_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally3_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally4_window.GetComponent<Animator>().SetTrigger("MoveDown");
        // 全てのallyのウィンドウのボタン機能を無効化
        ally1_window.GetComponent<Button>().interactable = false;
        ally2_window.GetComponent<Button>().interactable = false;
        ally3_window.GetComponent<Button>().interactable = false;
        ally4_window.GetComponent<Button>().interactable = false;
        // cancelボタンを有効化
        cancel_a.SetActive(true);
        cancel_b.SetActive(true);
    }

    // ally2のウィンドウを押された時の処理
    public void Click2()
    {
        // ウィンドウを非表示，サブウィンドウを表示
        ally2_window.GetComponent<Image>().enabled = false;
        ally2_subwindow.GetComponent<Image>().enabled = true;
        // ally2のスキル説明文を表示，サブウィンドウを一番左に移動
        ally2_skillwindow.GetComponent<Animator>().SetTrigger("Appear");
        ally2_subwindow.GetComponent<Animator>().SetTrigger("MoveLeft");
        // その他のallyのウィンドウを画面外に動かす
        ally1_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally3_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally4_window.GetComponent<Animator>().SetTrigger("MoveDown");
        // 全てのallyのウィンドウのボタン機能を無効化
        ally1_window.GetComponent<Button>().interactable = false;
        ally2_window.GetComponent<Button>().interactable = false;
        ally3_window.GetComponent<Button>().interactable = false;
        ally4_window.GetComponent<Button>().interactable = false;
        // cancelボタンを有効化
        cancel_a.SetActive(true);
        cancel_b.SetActive(true);
    }

    // ally3のウィンドウを押された時の処理
    public void Click3()
    {
        // ウィンドウを非表示，サブウィンドウを表示
        ally3_window.GetComponent<Image>().enabled = false;
        ally3_subwindow.GetComponent<Image>().enabled = true;
        // ally3のスキル説明文を表示，サブウィンドウを一番左に移動
        ally3_skillwindow.GetComponent<Animator>().SetTrigger("Appear");
        ally3_subwindow.GetComponent<Animator>().SetTrigger("MoveLeft");
        // その他のallyのウィンドウを画面外に動かす
        ally1_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally2_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally4_window.GetComponent<Animator>().SetTrigger("MoveDown");
        // 全てのallyのウィンドウのボタン機能を無効化
        ally1_window.GetComponent<Button>().interactable = false;
        ally2_window.GetComponent<Button>().interactable = false;
        ally3_window.GetComponent<Button>().interactable = false;
        ally4_window.GetComponent<Button>().interactable = false;
        // cancelボタンを有効化
        cancel_a.SetActive(true);
        cancel_b.SetActive(true);
    }

    // ally4のウィンドウを押された時の処理
    public void Click4()
    {
        // ウィンドウを非表示，サブウィンドウを表示
        ally4_window.GetComponent<Image>().enabled = false;
        ally4_subwindow.GetComponent<Image>().enabled = true;
        // ally4のスキル説明文を表示，サブウィンドウを一番左に移動
        ally4_skillwindow.GetComponent<Animator>().SetTrigger("Appear");
        ally4_subwindow.GetComponent<Animator>().SetTrigger("MoveLeft");
        // その他のallyのウィンドウを画面外に動かす
        ally1_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally2_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally3_window.GetComponent<Animator>().SetTrigger("MoveDown");
        // 全てのallyのウィンドウのボタン機能を無効化
        ally1_window.GetComponent<Button>().interactable = false;
        ally2_window.GetComponent<Button>().interactable = false;
        ally3_window.GetComponent<Button>().interactable = false;
        ally4_window.GetComponent<Button>().interactable = false;
        // cancelボタンを有効化
        cancel_a.SetActive(true);
        cancel_b.SetActive(true);
    }

    // cancelを押された時の処理
    public void ClickCancel()
    {
        // cancelボタンを無効化
        cancel_a.SetActive(false);
        cancel_b.SetActive(false);
        // 全てのallyのウィンドウのボタン機能を有効化
        ally1_window.GetComponent<Button>().interactable = true;
        ally2_window.GetComponent<Button>().interactable = true;
        ally3_window.GetComponent<Button>().interactable = true;
        ally4_window.GetComponent<Button>().interactable = true;

        // ally1のスキル説明文が表示されている時の処理
        if (ally1_skillwindow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("active"))
        {
            // ally1のスキル説明文を非表示
            ally1_skillwindow.GetComponent<Animator>().SetTrigger("Disappear");
            // その他のallyのウィンドウを画面内に動かす
            ally2_window.GetComponent<Animator>().SetTrigger("MoveUp");
            ally3_window.GetComponent<Animator>().SetTrigger("MoveUp");
            ally4_window.GetComponent<Animator>().SetTrigger("MoveUp");
        }

        // ally2のスキル説明文が表示されている時の処理
        if (ally2_skillwindow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("active"))
        {
            StartCoroutine(Inactive2());
        }

        // ally3のスキル説明文が表示されている時の処理
        if (ally3_skillwindow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("active"))
        {
            StartCoroutine(Inactive3());
        }

        // ally4のスキル説明文が表示されている時の処理
        if (ally4_skillwindow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("active"))
        {
            StartCoroutine(Inactive4());
        }
    }

    IEnumerator Inactive2()
    {
        // ally2のスキル説明文を非表示，サブウィンドウを元の位置に移動
        ally2_skillwindow.GetComponent<Animator>().SetTrigger("Disappear");
        ally2_subwindow.GetComponent<Animator>().SetTrigger("MoveRight");
        // その他のallyのウィンドウを画面内に動かす
        ally1_window.GetComponent<Animator>().SetTrigger("MoveUp");
        ally3_window.GetComponent<Animator>().SetTrigger("MoveUp");
        ally4_window.GetComponent<Animator>().SetTrigger("MoveUp");

        yield return new WaitForSeconds(0.3f);

        // ウィンドウを表示，サブウィンドウを非表示
        ally2_window.GetComponent<Image>().enabled = true;
        ally2_subwindow.GetComponent<Image>().enabled = false;
    }

    IEnumerator Inactive3()
    {
        // ally3のスキル説明文を非表示，サブウィンドウを元の位置に移動
        ally3_skillwindow.GetComponent<Animator>().SetTrigger("Disappear");
        ally3_subwindow.GetComponent<Animator>().SetTrigger("MoveRight");
        // その他のallyのウィンドウを画面内に動かす
        ally1_window.GetComponent<Animator>().SetTrigger("MoveUp");
        ally2_window.GetComponent<Animator>().SetTrigger("MoveUp");
        ally4_window.GetComponent<Animator>().SetTrigger("MoveUp");

        yield return new WaitForSeconds(0.3f);

        // ウィンドウを表示，サブウィンドウを非表示
        ally3_window.GetComponent<Image>().enabled = true;
        ally3_subwindow.GetComponent<Image>().enabled = false;
    }
    IEnumerator Inactive4()
    {
        // ally4のスキル説明文を非表示，サブウィンドウを元の位置に移動
        ally4_skillwindow.GetComponent<Animator>().SetTrigger("Disappear");
        ally4_subwindow.GetComponent<Animator>().SetTrigger("MoveRight");
        // その他のallyのウィンドウを画面内に動かす
        ally1_window.GetComponent<Animator>().SetTrigger("MoveUp");
        ally2_window.GetComponent<Animator>().SetTrigger("MoveUp");
        ally3_window.GetComponent<Animator>().SetTrigger("MoveUp");

        yield return new WaitForSeconds(0.3f);

        // ウィンドウを表示，サブウィンドウを非表示
        ally4_window.GetComponent<Image>().enabled = true;
        ally4_subwindow.GetComponent<Image>().enabled = false;
    }
}