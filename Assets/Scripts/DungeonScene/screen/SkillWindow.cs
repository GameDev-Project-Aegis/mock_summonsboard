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
    public GameObject cancel;


    // ally1のウィンドウを押されたらスキルウィンドウを表示
    public void Click1()
    {
        ally1_skillwindow.GetComponent<Animator>().SetTrigger("Appear");
        ally2_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally3_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally4_window.GetComponent<Animator>().SetTrigger("MoveDown");
        // ally1のウィンドウのボタン機能を無効化
        ally1_window.GetComponent<Button>().interactable = false;
        // cancelボタンをactive化
        cancel.SetActive(true);
    }

    // ally2のウィンドウを押されたらスキルウィンドウを表示
    public void Click2()
    {
        ally2_window.GetComponent<Animator>().SetTrigger("MoveLeft");
        ally2_skillwindow.GetComponent<Animator>().SetTrigger("Appear");

        ally1_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally3_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally4_window.GetComponent<Animator>().SetTrigger("MoveDown");
        // ally2のウィンドウのボタン機能を無効化
        ally2_window.GetComponent<Button>().interactable = false;
        // cancelボタンをactive化
        cancel.SetActive(true);
    }

    // ally3のウィンドウを押されたらスキルウィンドウを表示
    public void Click3()
    {
        ally3_window.GetComponent<Animator>().SetTrigger("MoveLeft");
        ally3_skillwindow.GetComponent<Animator>().SetTrigger("Appear");

        ally1_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally2_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally4_window.GetComponent<Animator>().SetTrigger("MoveDown");
        // ally3のウィンドウのボタン機能を無効化
        ally3_window.GetComponent<Button>().interactable = false;
        // cancelボタンをactive化
        cancel.SetActive(true);
    }

    // ally4のウィンドウを押されたらスキルウィンドウを表示
    public void Click4()
    {
        ally4_window.GetComponent<Animator>().SetTrigger("MoveLeft");
        ally4_skillwindow.GetComponent<Animator>().SetTrigger("Appear");

        ally1_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally2_window.GetComponent<Animator>().SetTrigger("MoveDown");
        ally3_window.GetComponent<Animator>().SetTrigger("MoveDown");
        // ally4のウィンドウのボタン機能を無効化
        ally4_window.GetComponent<Button>().interactable = false;
        // cancelボタンをactive化
        cancel.SetActive(true);
    }
}