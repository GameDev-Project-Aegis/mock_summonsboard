using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ally : MonoBehaviour
{
    //モンスターのアニメーション
    public GameObject head1;
    public GameObject head2;
    public GameObject head3;
    public GameObject neck1;
    public GameObject neck2;
    public GameObject neck3;
    public GameObject body;
    public GameObject leg;
    public GameObject tail;
    public GameObject wing1;
    public GameObject wing2;
    public GameObject effect1;
    public GameObject effect2;

    public void Attack()
    {
        head1.GetComponent<Animator>().SetTrigger("attack");
        head2.GetComponent<Animator>().SetTrigger("attack");
        head3.GetComponent<Animator>().SetTrigger("attack");
        neck1.GetComponent<Animator>().SetTrigger("attack");
        neck2.GetComponent<Animator>().SetTrigger("attack");
        neck3.GetComponent<Animator>().SetTrigger("attack");
        body.GetComponent<Animator>().SetTrigger("attack");
        leg.GetComponent<Animator>().SetTrigger("attack");
        tail.GetComponent<Animator>().SetTrigger("attack");
        wing1.GetComponent<Animator>().SetTrigger("attack");
        wing2.GetComponent<Animator>().SetTrigger("attack");
        effect1.GetComponent<Animator>().SetTrigger("attack");
        effect2.GetComponent<Animator>().SetTrigger("attack");
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
            if(arrayBoard[PointY-1,PointX-1]==11||arrayBoard[PointY-1,PointX-1]==12){
                arrow01.SetActive(true);
            }
        }
        if(PointX!=0&&moveAlly[1]!=0){
            if(arrayBoard[PointY,PointX-1]==11||arrayBoard[PointY,PointX-1]==12){
                arrow02.SetActive(true);
            }
        }
        if(PointX!=0&&PointY!=3&&moveAlly[2]!=0){
            if(arrayBoard[PointY+1,PointX-1]==11||arrayBoard[PointY+1,PointX-1]==12){
                arrow03.SetActive(true);
            }
        }
        if(PointY!=0&&moveAlly[3]!=0){
            if(arrayBoard[PointY-1,PointX]==11||arrayBoard[PointY-1,PointX]==12){
                arrow04.SetActive(true);
            }
        }
        if(PointY!=3&&moveAlly[4]!=0){
            if(arrayBoard[PointY+1,PointX]==11||arrayBoard[PointY+1,PointX]==12){
                arrow05.SetActive(true);
            }
        }
        if(PointX!=3&&PointY!=0&&moveAlly[5]!=0){
            if(arrayBoard[PointY-1,PointX+1]==11||arrayBoard[PointY-1,PointX+1]==12){
                arrow06.SetActive(true);
            }
        }
        if(PointX!=3&&moveAlly[6]!=0){
            if(arrayBoard[PointY,PointX+1]==11||arrayBoard[PointY,PointX+1]==12){
                arrow07.SetActive(true);
            }
        }
        if(PointX!=3&&PointY!=3&&moveAlly[7]!=0){
            if(arrayBoard[PointY+1,PointX+1]==11||arrayBoard[PointY+1,PointX+1]==12){
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

    //攻撃時のアニメーション
    public void PowerUp()
    {
        //モンスター自体がでっかくなる
        //strengthが出現する
    }

    public void PowerCharge()
    {
        //
    }
}
