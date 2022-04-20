using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject objectNomalDungeon;
    public GameObject objectFireDungeon;
    public GameObject objectfireDungeon01;
    public GameObject selectDungeon;
    public GameObject canvasGame;

    // Start is called before the first frame update
    void Start()
    {
        objectNomalDungeon.GetComponent<Animator>().SetTrigger("slideIn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TapNomalDungeon(){
        //nomalの消去
        objectNomalDungeon.GetComponent<Animator>().SetTrigger("slideOut");
        //fireの消去
        objectFireDungeon.GetComponent<Animator>().SetTrigger("slideIn");
    }

    public void TapFireDungeon(){
        //nomalの消去
        objectFireDungeon.GetComponent<Animator>().SetTrigger("slideOut");
        //fireの消去
        objectfireDungeon01.GetComponent<Animator>().SetTrigger("slideIn");
    }
    
    public void TapfireDungeon01(){
        selectDungeon.SetActive(true);
    }
}
