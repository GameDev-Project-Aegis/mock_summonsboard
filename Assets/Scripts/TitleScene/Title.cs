using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // ボタンを押した時の処理
    public void Tap()
    {
        // MainSceneの追加
        SceneManager.LoadScene ("MainScene");

	    // TitleSceneの削除
        SceneManager.UnloadSceneAsync("TitleScene");
    }
}
