using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject fade;//インスペクタからPrefab化したCanvasを入れる
    public GameObject fadeCanvas;//操作するCanvas、タグで探す
    public GameObject ResultEffect;

    void Start()
    {
        Invoke("findFadeObject", 0.02f);
        //sceneChange("MainScene");
        Invoke("Result", 3.0f);
    }

    void Result(){
        ResultEffect.SetActive(true);
    }

    void findFadeObject()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("Fade");//Canvasをみつける
        fadeCanvas.GetComponent<FadeManager>().fadeIn();//フェードインフラグを立てる
    }

    public async void sceneChange()//ボタン操作などで呼び出す
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("Fade");
        fadeCanvas.GetComponent<FadeManager>().fadeOut();//フェードアウトフラグを立てる
        await Task.Delay(2000);//暗転するまで待つ
        SceneManager.LoadScene("MainScene");//シーンチェンジ
    }
}
