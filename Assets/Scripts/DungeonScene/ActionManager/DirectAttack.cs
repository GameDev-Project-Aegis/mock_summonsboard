using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttack : MonoBehaviour
{
    public GameObject BoardSurface;
    BoardSurface BoardSurfaceClass;

    public GameObject ally1;
    Animator Ally1Class;
    public GameObject ally2;
    Animator Ally2Class;
    public GameObject ally3;
    Animator Ally3Class;
    public GameObject ally4;
    Animator Ally4Class;

    void Start()
    {
        Ally1Class = ally1.GetComponent<Animator>();
        Ally2Class = ally2.GetComponent<Animator>();
        Ally3Class = ally3.GetComponent<Animator>();
        Ally4Class = ally4.GetComponent<Animator>();
    }

    public void AllyDirectAttack(int[,] arrayBoard)
    {
        //周囲8マスのモンスターを判別する関数
        int[] arrayAround8 = new int[8];

        //守備モンスターの配置を順に確認する
        for(int i=0; i<4; i++){
            for(int j=0; j<4; j++){
                if(arrayBoard[j,i]==11){
                    //八方のマスを確認する
                    arrayAround8[0] = ReturnSquareStatus(i-1,j-1,arrayBoard);
                    arrayAround8[1] = ReturnSquareStatus(i-1,j,arrayBoard);
                    arrayAround8[2] = ReturnSquareStatus(i-1,j+1,arrayBoard);
                    arrayAround8[3] = ReturnSquareStatus(i,j-1,arrayBoard);
                    arrayAround8[4] = ReturnSquareStatus(i,j+1,arrayBoard);
                    arrayAround8[5] = ReturnSquareStatus(i+1,j-1,arrayBoard);
                    arrayAround8[6] = ReturnSquareStatus(i+1,j,arrayBoard);
                    arrayAround8[7] = ReturnSquareStatus(i+1,j+1,arrayBoard);

                    // //八方マスに攻撃モンスターが存在するか判定する
                    int ally1_index = Array.IndexOf(arrayAround8, 1);
                    int ally2_index = Array.IndexOf(arrayAround8, 2);
                    int ally3_index = Array.IndexOf(arrayAround8, 3);
                    int ally4_index = Array.IndexOf(arrayAround8, 4);

                    int combo = 0;

                    if(ally1_index!=-1){combo += 1;}
                    if(ally2_index!=-1){combo += 1;}
                    if(ally3_index!=-1){combo += 1;}
                    if(ally4_index!=-1){combo += 1;}

                    if(combo==0){
                        //攻撃は行われない
                    }
                    else{
                        if(ally1_index!=-1){
                            switch (combo)
                            {
                                case 1:
                                    Ally1Class.SetTrigger("attack3");
                                    break;
                                case 2:
                                    Ally1Class.SetTrigger("combo6");
                                    break;
                                case 3:
                                    Ally1Class.SetTrigger("combo9");
                                    break;
                                case 4:
                                    Ally1Class.SetTrigger("combo12");
                                    break;
                            }
                            switch (ally1_index)
                            {
                                case 0:
                                    Ally1Class.SetTrigger("RightDown");
                                    break;
                                case 1:
                                    Ally1Class.SetTrigger("Right");
                                    break;
                                case 2:
                                    Ally1Class.SetTrigger("RightUp");
                                    break;
                                case 3:
                                    Ally1Class.SetTrigger("Down");
                                    break;
                                case 4:
                                    Ally1Class.SetTrigger("Up");
                                    break;
                                case 5:
                                    Ally1Class.SetTrigger("LeftDown");
                                    break;
                                case 6:
                                    Ally1Class.SetTrigger("Left");
                                    break;
                                case 7:
                                    Ally1Class.SetTrigger("LeftUp");
                                    break;
                            }
                        }
                    }
                    //敵一体を複数で囲むパターン

                    //敵一体を単体で攻撃するパターン
                        //敵複数体を単体モンスターで攻撃するパターン
                }
            }
        }

        // //接している攻撃モンスターの攻撃アニメーションを呼び出す
        // Ally1Class.Attack3();
    }
    //座標のステータスを確認する関数
    int ReturnSquareStatus(int PointX, int PointY, int[,] arrayBoard)
    {
        if(PointX >= 0 && PointX < 4 && PointY >= 0 && PointY < 4){
            return arrayBoard[PointY,PointX];
        }else{
            return 0;
        }
    }
}
