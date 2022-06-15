using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartEffect : MonoBehaviour
{
    public GameObject backShadow;
    public GameObject imageGame;
    public GameObject imageStart;
    public GameObject window;
    public GameObject ally1;
    public GameObject ally2;
    public GameObject ally3;
    public GameObject ally4;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject smoke_target1;
    public GameObject smoke_target2;

    // オブジェクト参照（？）
    public GameObject TurnStart;
    TurnStart TurnStartClass;

    // Start is called before the first frame update
    void Start()
    {
        // モンスターを非表示
        ally1.SetActive(false);
        ally2.SetActive(false);
        ally3.SetActive(false);
        ally4.SetActive(false);
        // enemy1.GetComponent<Image>().enabled = false;
        // enemy2.GetComponent<Image>().enabled = false;
        enemy1.SetActive(false);
        enemy2.SetActive(false);        

        // windowを非表示
        window.GetComponent<Renderer>().enabled = false;

        StartCoroutine(GameStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GameStart()
    {
        // 画面を明るくする
        backShadow.GetComponent<Animator>().SetTrigger("FadeIn");
        
        yield return new WaitForSeconds(0.5f);

        // AllySlideInコルーチンの呼び出し
        StartCoroutine(AllySlideIn());

        yield return new WaitForSeconds(1.5f);

        // EnemyAppearコルーチンの呼び出し
        StartCoroutine(EnemyAppear());

        yield return new WaitForSeconds(1.0f);

        // LogoSlideInメソッドの呼び出し
        LogoSlideIn();

        yield return new WaitForSeconds(1.5f);

        TurnStartClass = TurnStart.GetComponent<TurnStart>();

        // // TurnStartクラス内のStartPlayerメソッドの呼び出し
        // TurnStartClass.StartPlayer();

        // TurnStartクラス内のStartEnemyメソッドの呼び出し
        TurnStartClass.StartEnemy();
    }

    // 味方を画面内に登場させるコルーチン
    IEnumerator AllySlideIn()
    {
        // 味方を画面外に移動
        ally1.transform.localPosition = new Vector3(160, 0, 0);
        ally2.transform.localPosition = new Vector3(160, 0, 0);
        ally3.transform.localPosition = new Vector3(160, 0, 0);
        ally4.transform.localPosition = new Vector3(160, 0, 0);

        // 味方を表示
        ally1.SetActive(true);
        ally2.SetActive(true);
        ally3.SetActive(true);
        ally4.SetActive(true);

        // 味方を画面内に登場させる
        ally2.GetComponent<Animator>().SetTrigger("SlideIn");

        yield return new WaitForSeconds(0.1f);

        ally3.GetComponent<Animator>().SetTrigger("SlideIn");

        yield return new WaitForSeconds(0.1f);

        ally4.GetComponent<Animator>().SetTrigger("SlideIn");

        yield return new WaitForSeconds(0.5f);

        ally1.GetComponent<Animator>().SetTrigger("SlideIn");
    }

    // 敵を画面内に登場させるコルーチン
    IEnumerator EnemyAppear()
    {
        // 敵を表示
        // enemy1.GetComponent<Image>().enabled = true;
        enemy1.SetActive(true);
        smoke_target1.GetComponent<ParticleSystem>().Play();  // 砂埃のパーティクルエフェクトを再生

        yield return new WaitForSeconds(0.2f);

        // enemy2.GetComponent<Image>().enabled = true;
        enemy2.SetActive(true);
        smoke_target2.GetComponent<ParticleSystem>().Play();
    }

    // ゲーム開始時の文字、windowを画面内に登場させる
    void LogoSlideIn()
    {
        // windowを下に移動した後に表示
        window.transform.position = new Vector3(0, -230, -5400);
        window.GetComponent<Renderer>().enabled = true;

        // 各アニメーションの呼び出し
        window.GetComponent<Animator>().SetTrigger("PopUp");
        imageGame.GetComponent<Animator>().SetTrigger("SlideIn");
        imageStart.GetComponent<Animator>().SetTrigger("SlideIn");
    }
}