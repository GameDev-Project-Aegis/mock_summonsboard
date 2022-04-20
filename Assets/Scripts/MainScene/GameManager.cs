using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject objectNomalDungeon;
    public GameObject objectFireDungeon;
    public GameObject objectfireDungeon01;
    public GameObject selectDungeon;
    public GameObject objectEntranceDungeon;
    public GameObject canvasGame;

    void Start()
    {
        objectNomalDungeon.GetComponent<Animator>().SetTrigger("slideIn");
    }

    //DungeonObjectディレクトのスクリプトから呼び出し

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

    //SelectDungeonディレクトのスクリプトから呼び出し

    public void TapEnterButton(){
        // DungeonSceneの追加
        SceneManager.LoadScene ("DungeonScene");
	    // MainSceneの削除
        SceneManager.UnloadSceneAsync("MainScene");
    }

    public void TapBackButton(){
        //shrinkアニメーション
        objectEntranceDungeon.GetComponent<Animator>().SetTrigger("shrink");
        //オブジェクト消去
        StartCoroutine(BackDungeonMenu());
    }

    IEnumerator BackDungeonMenu()
    {
        // 指定した秒数だけ処理を待つ(ここでは0.1秒)
        yield return new WaitForSeconds(0.5f);
        //オブジェクトを消去
        selectDungeon.SetActive(false);
    }
}
