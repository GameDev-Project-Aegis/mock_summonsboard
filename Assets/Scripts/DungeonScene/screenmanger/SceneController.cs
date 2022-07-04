using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject BlackOut;

    public async void sceneChange()//ボタン操作などで呼び出す
    {
        await Task.Delay(500);
        BlackOut.SetActive(true);
        await Task.Delay(500);
        SceneManager.LoadScene ("MainScene");
        SceneManager.UnloadSceneAsync("DungeonScene");
    }
}
