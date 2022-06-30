using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurviveManager : MonoBehaviour
{
    int ally1HP = 10000;
    int ally2HP = 10000;
    int ally3HP = 10000;
    int ally4HP = 10000;
    int enemy1HP = 3;
    int enemy2HP = 3;
    int enemy3HP = 2000;
    int enemy4HP = 2000;
    // int boss1HP = 10000;

    public GameObject BoardSurface;
    BoardSurface BoardSurfaceClass;

    // Start is called before the first frame update
    void Start()
    {
        BoardSurfaceClass = BoardSurface.GetComponent<BoardSurface>();
    }

    //ヒットポイントの減少
    public void HPdamage(int ObjectNum, int Damage)
    {
        switch(ObjectNum){
            case 1:
                ally1HP -= Damage;
                if(ally1HP < 0){
                    //HP0以下になったら死ぬ
                    BoardSurfaceClass.ExtinguishMonster(1);
                }
                break;
            case 2:
                ally2HP -= Damage;
                if(ally1HP < 0){
                    BoardSurfaceClass.ExtinguishMonster(2);
                }
                break;
            case 3:
                ally3HP -= Damage;
                if(ally1HP < 0){
                    BoardSurfaceClass.ExtinguishMonster(3);
                }
                break;
            case 4:
                ally4HP -= Damage;
                if(ally1HP < 0){
                    BoardSurfaceClass.ExtinguishMonster(4);
                }
                break;
            case 11:
                enemy1HP -= Damage;
                Debug.Log("enemy1 HP:"+enemy1HP);
                if(enemy1HP < 0){
                    BoardSurfaceClass.ExtinguishMonster(11);
                }
                break;
            case 12:
                enemy2HP -= Damage;
                Debug.Log("enemy2 HP:"+enemy2HP);
                if(enemy2HP < 0){
                    BoardSurfaceClass.ExtinguishMonster(12);
                }
                break;
            case 13:
                enemy3HP -= Damage;
                Debug.Log("enemy3 HP:"+enemy3HP);
                if(enemy3HP < 0){
                    BoardSurfaceClass.ExtinguishMonster(13);
                }
                break;
            case 14:
                enemy4HP -= Damage;
                Debug.Log("enemy4 HP:"+enemy4HP);
                if(enemy4HP < 0){
                    BoardSurfaceClass.ExtinguishMonster(14);
                }
                break;
        }
    }

    //ヒットポイントの回復
    void HPheal(){}
}
