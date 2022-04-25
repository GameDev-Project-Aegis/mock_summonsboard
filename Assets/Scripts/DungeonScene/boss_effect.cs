using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boss_effect : MonoBehaviour
{
    float fadeSpeed = 0.2f;        //透明度が変わるスピードを管理
	float red, green, blue, alfa;   //パネルの色、不透明度を管理
 
	public bool isFadeOut = true;  //フェードアウト処理の開始、完了を管理するフラグ
	public bool isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ

    public GameObject backShadow;
    public GameObject warning1;
    public GameObject warning2;
    public GameObject warning7a;
    public GameObject warning7b;
    
    Image fadeImage_backShadow;                //透明度を変更するパネルのイメージ

    void Start () {
		fadeImage_backShadow = backShadow.GetComponent<Image> ();
		red = fadeImage_backShadow.color.r;
		green = fadeImage_backShadow.color.g;
		blue = fadeImage_backShadow.color.b;
		alfa = fadeImage_backShadow.color.a;

        //後ろでwarning動かす
        warning7a.GetComponent<Animator>().SetTrigger("slideIn");
        warning7b.GetComponent<Animator>().SetTrigger("slideIn");
	}
 
	void Update () {
		if(isFadeIn){
			StartFadeIn ();
		}
 
		if (isFadeOut) {
			StartFadeOut ();
		}
	}
 
	void StartFadeIn(){
		alfa -= fadeSpeed;                //a)不透明度を徐々に下げる
		SetAlpha ();                      //b)変更した不透明度パネルに反映する
		if(alfa <= 0){                    //c)完全に透明になったら処理を抜ける
			isFadeIn = false;             
			fadeImage_backShadow.enabled = false;    //d)パネルの表示をオフにする
		}
	}
 
	void StartFadeOut(){
		//fadeImage_backShadow.enabled = true;  // a)パネルの表示をオンにする
		alfa += fadeSpeed;         // b)不透明度を徐々にあげる
		SetAlpha ();               // c)変更した透明度をパネルに反映する
		if(alfa >= 0.5){             // d)完全に不透明になったら処理を抜ける
			isFadeOut = false;
		}
	}
 
	void SetAlpha(){
		fadeImage_backShadow.color = new Color(red, green, blue, alfa);
	}
}
