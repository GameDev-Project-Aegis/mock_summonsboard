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

    //火のダンジョンレベルの時 => false
    //フロアセレクトレベルの時 => true
    int selectDungeonLevel = 0;

    void Start()
    {
        objectNomalDungeon.GetComponent<Animator>().SetTrigger("slideIn");
    }

    //DungeonObjectディレクトのスクリプトから呼び出し

    public void TapNomalDungeon(){
        objectNomalDungeon.GetComponent<Animator>().SetTrigger("slideOut");
        objectFireDungeon.GetComponent<Animator>().SetTrigger("slideIn");
        selectDungeonLevel = 1;
    }

    public void TapFireDungeon(){
        objectFireDungeon.GetComponent<Animator>().SetTrigger("slideOut");
        objectfireDungeon01.GetComponent<Animator>().SetTrigger("slideIn");
        selectDungeonLevel = 2;
    }
    
    public void TapfireDungeon01(){
        selectDungeon.SetActive(true);
    }

    public void TapBackButton(){
        switch (selectDungeonLevel)
        {
            //
            case 0:
                break;
            case 1:
                objectFireDungeon.GetComponent<Animator>().SetTrigger("slideOut");
                objectNomalDungeon.GetComponent<Animator>().SetTrigger("slideIn");
                selectDungeonLevel = 0;
                break;
            case 2:
                objectfireDungeon01.GetComponent<Animator>().SetTrigger("slideOut");
                objectFireDungeon.GetComponent<Animator>().SetTrigger("slideIn");
                selectDungeonLevel = 1;
                break;
        }
    }

    //SelectDungeonディレクトのスクリプトから呼び出し

    public void TapEnterDungeon(){
        // DungeonSceneの追加
        SceneManager.LoadScene ("DungeonScene");
	    // MainSceneの削除
        SceneManager.UnloadSceneAsync("MainScene");
    }

    public void TapBackDungeon(){
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
