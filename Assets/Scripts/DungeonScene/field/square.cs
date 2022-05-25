using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class square : MonoBehaviour
{
    public GameObject focus1;
    public GameObject focus2;
    public GameObject focus3;

    public void FocusMove(){
        focus1.GetComponent<Animator>().SetTrigger("focus");
        focus2.GetComponent<Animator>().SetTrigger("focus");
        focus3.GetComponent<Animator>().SetTrigger("focus");
    }

    public void UnFocusMove(){
        focus1.GetComponent<Animator>().SetTrigger("unfocus");
        focus2.GetComponent<Animator>().SetTrigger("unfocus");
        focus3.GetComponent<Animator>().SetTrigger("unfocus");
    }
}
