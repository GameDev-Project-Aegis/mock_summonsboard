using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ボタンを押した時の処理
    public void Tap()
    {
        // MainSceneの追加
        SceneManager.LoadScene ("MainScene");

	// TitleSceneの削除
        SceneManager.UnloadSceneAsync("TitleScene");
	
	Debug.Log("デバッグ");
    }
}
