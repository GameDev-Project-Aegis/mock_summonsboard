using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGameoverEffect : MonoBehaviour
{
    public GameObject ObjectGameover;
    public GameObject ObjectContinue;
    public GameObject ObjectRetire1;
    public GameObject ObjectRetire2;

    public GameObject Gameover1;
    public GameObject Gameover2;
    public GameObject Gameover3;
    public GameObject Gameover4;
    public GameObject Gameover5;
    public GameObject Gameover6;
    public GameObject Gameover7;
    public GameObject Gameover8;

    public GameObject mainCamera;

    public void TapYesGameover(){
        ObjectGameover.GetComponent<Animator>().SetTrigger("scaleOut");
        ObjectContinue.GetComponent<Animator>().SetTrigger("scaleIn");
    }
    public void TapNoGameover(){
        ObjectGameover.GetComponent<Animator>().SetTrigger("scaleOut");
        ObjectRetire1.GetComponent<Animator>().SetTrigger("scaleIn");
    }

    public void TapYesContinue(){
        ObjectContinue.GetComponent<Animator>().SetTrigger("scaleOut");
        ObjectGameover.GetComponent<Animator>().SetTrigger("scaleIn");
    }
    public void TapNoContinue(){
        ObjectContinue.GetComponent<Animator>().SetTrigger("scaleOut");
        ObjectGameover.GetComponent<Animator>().SetTrigger("scaleIn");
    }

    public void TapYesRetire1(){
        ObjectRetire1.GetComponent<Animator>().SetTrigger("scaleOut");
        Gameover8.GetComponent<Animator>().SetTrigger("crumble");
        Gameover7.GetComponent<Animator>().SetTrigger("crumble");
        Gameover6.GetComponent<Animator>().SetTrigger("crumble");
        Gameover5.GetComponent<Animator>().SetTrigger("crumble");
        Gameover4.GetComponent<Animator>().SetTrigger("crumble");
        Gameover3.GetComponent<Animator>().SetTrigger("crumble");
        Gameover2.GetComponent<Animator>().SetTrigger("crumble");
        Gameover1.GetComponent<Animator>().SetTrigger("crumble");
        ObjectRetire2.GetComponent<Animator>().SetTrigger("scaleIn");
    }
    public void TapNoRetire1(){
        ObjectRetire1.GetComponent<Animator>().SetTrigger("scaleOut");
        ObjectGameover.GetComponent<Animator>().SetTrigger("scaleIn");
    }

    public void TapRetire2(){
        ObjectRetire2.GetComponent<Animator>().SetTrigger("scaleOut");
        mainCamera.GetComponent<SceneController>().sceneChange();
    }
}
