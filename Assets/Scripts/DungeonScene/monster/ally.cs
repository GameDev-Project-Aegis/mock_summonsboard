using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ally : MonoBehaviour
{
    //攻撃時のアニメーション
    public void Attack3()
    {
        GetComponent<Animator>().SetTrigger("attack3");
        // GetComponent<Animator>().SetTrigger("Left");
        GetComponent<Animator>().SetTrigger("MultiDirection");
    }
    public void Combo3()
    {
        // GetComponent<Animator>().SetTrigger("attack3");
        GetComponent<Animator>().SetTrigger("combo12");
        // GetComponent<Animator>().SetTrigger("Left");
        GetComponent<Animator>().SetTrigger("MultiDirection");
    }

    //ドラッグ時のアニメーション
    public GameObject arrow01;
    public GameObject arrow02;
    public GameObject arrow03;
    public GameObject arrow04;
    public GameObject arrow05;
    public GameObject arrow06;
    public GameObject arrow07;
    public GameObject arrow08;

    public void SwordEffectAction(int[] PointArray, int[,] arrayBoard, int[] moveAlly)
    {
        //一旦リセットする
        SwordEffectStop();

        //自分の座標の周囲に敵モンスターがいるかを検知する
        int PointX;
        int PointY;

        PointX = PointArray[0];
        PointY = PointArray[1];

        if(PointX!=0&&PointY!=0&&moveAlly[0]!=0){
            int Square01 = arrayBoard[PointY-1,PointX-1];
            if(Square01==11||Square01==12||Square01==13||Square01==14){
                arrow01.SetActive(true);
            }
        }
        if(PointX!=0&&moveAlly[1]!=0){
            int Square02 = arrayBoard[PointY,PointX-1];
            if(Square02==11||Square02==12||Square02==13||Square02==14){
                arrow02.SetActive(true);
            }
        }
        if(PointX!=0&&PointY!=3&&moveAlly[2]!=0){
            int Square03 = arrayBoard[PointY+1,PointX-1];
            if(Square03==11||Square03==12||Square03==13||Square03==14){
                arrow03.SetActive(true);
            }
        }
        if(PointY!=0&&moveAlly[3]!=0){
            int Square04 = arrayBoard[PointY-1,PointX];
            if(Square04==11||Square04==12||Square04==13||Square04==14){
                arrow04.SetActive(true);
            }
        }
        if(PointY!=3&&moveAlly[4]!=0){
            int Square05 = arrayBoard[PointY+1,PointX];
            if(Square05==11||Square05==12||Square05==13||Square05==14){
                arrow05.SetActive(true);
            }
        }
        if(PointX!=3&&PointY!=0&&moveAlly[5]!=0){
            int Square06 = arrayBoard[PointY-1,PointX+1];
            if(Square06==11||Square06==12||Square06==13||Square06==14){
                arrow06.SetActive(true);
            }
        }
        if(PointX!=3&&moveAlly[6]!=0){
            int Square07 = arrayBoard[PointY,PointX+1];
            if(Square07==11||Square07==12||Square07==13||Square07==14){
                arrow07.SetActive(true);
            }
        }
        if(PointX!=3&&PointY!=3&&moveAlly[7]!=0){
            int Square08 = arrayBoard[PointY+1,PointX+1];
            if(Square08==11||Square08==12||Square08==13||Square08==14){
                arrow08.SetActive(true);
            }
        }
    }

    public void SwordEffectStop()
    {
        arrow01.SetActive(false);
        arrow02.SetActive(false);
        arrow03.SetActive(false);
        arrow04.SetActive(false);
        arrow05.SetActive(false);
        arrow06.SetActive(false);
        arrow07.SetActive(false);
        arrow08.SetActive(false);
    }
}
