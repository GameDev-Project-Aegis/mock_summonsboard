using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject ResultEffect;
    public GameObject BlackOut;

    void Start()
    {
        //Invoke("Result", 3.0f);
    }

    void Result(){
        ResultEffect.SetActive(true);
    }

    public async void sceneChange()//ボタン操作などで呼び出す
    {
        await Task.Delay(500);
        BlackOut.SetActive(true);
        await Task.Delay(500);
        SceneManager.LoadScene ("MainScene");
        SceneManager.UnloadSceneAsync("DungeonScene");
    }
}
