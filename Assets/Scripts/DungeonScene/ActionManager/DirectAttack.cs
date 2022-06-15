using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    //攻撃矢印があるかを判定するための配列
    int[] moveAlly1 = {1,2,1,0,0,2,2,2};
    int[] moveAlly2 = {1,2,1,1,0,1,2,1};
    int[] moveAlly3 = {1,1,1,0,0,1,1,1};
    int[] moveAlly4 = {1,2,0,1,1,0,2,0};

    void Start()
    {
        Ally1Class = ally1.GetComponent<Animator>();
        Ally2Class = ally2.GetComponent<Animator>();
        Ally3Class = ally3.GetComponent<Animator>();
        Ally4Class = ally4.GetComponent<Animator>();
    }

    public IEnumerator AllyDirectAttack(int[,] arrayBoard)
    {
        //敵モンスター2体に関してまずコンボ攻撃を行う
        yield return StartCoroutine(ComboDirectAttack(arrayBoard));
        // yield return null;
    }

    IEnumerator ComboDirectAttack(int[,] arrayBoard)
    {
        //周囲8マスのモンスターを格納する配列
        int[] arrayAround8 = new int[8];

        for(int i=0; i<4; i++){
            for(int j=0; j<4; j++){
                if(arrayBoard[j,i]==11){
                    //八方のマスを確認する
                    arrayAround8[0] = ReturnSquareStatus(i-1,j-1,arrayBoard,7);
                    arrayAround8[1] = ReturnSquareStatus(i-1,j,arrayBoard,6);
                    arrayAround8[2] = ReturnSquareStatus(i-1,j+1,arrayBoard,5);
                    arrayAround8[3] = ReturnSquareStatus(i,j-1,arrayBoard,4);
                    arrayAround8[4] = ReturnSquareStatus(i,j+1,arrayBoard,3);
                    arrayAround8[5] = ReturnSquareStatus(i+1,j-1,arrayBoard,2);
                    arrayAround8[6] = ReturnSquareStatus(i+1,j,arrayBoard,1);
                    arrayAround8[7] = ReturnSquareStatus(i+1,j+1,arrayBoard,0);

                    // //八方マスに攻撃モンスターが存在するか判定する
                    int ally1_index = Array.IndexOf(arrayAround8, 1);
                    int ally2_index = Array.IndexOf(arrayAround8, 2);
                    int ally3_index = Array.IndexOf(arrayAround8, 3);
                    int ally4_index = Array.IndexOf(arrayAround8, 4);

                    //何コンボが入るのかを計算
                    int combo = 0;

                    if(ally1_index!=-1){combo += 1;}
                    if(ally2_index!=-1){combo += 1;}
                    if(ally3_index!=-1){combo += 1;}
                    if(ally4_index!=-1){combo += 1;}

                    if(combo==0){
                        //攻撃は行われない
                    }else if(combo>1){
                        foreach(int I in arrayAround8){
                            switch(I){
                                case 1:
                                    SetAttackAnimationTrigger(Ally1Class,combo,ally1_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    break;
                                case 2:
                                    SetAttackAnimationTrigger(Ally2Class,combo,ally2_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    break;
                                case 3:
                                    SetAttackAnimationTrigger(Ally3Class,combo,ally3_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    break;
                                case 4:
                                    SetAttackAnimationTrigger(Ally4Class,combo,ally4_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    break;
                            }
                        }
                        yield return new WaitForSeconds(1.5f);
                    }
                    //敵一体を複数で囲むパターン

                    //敵一体を単体で攻撃するパターン
                        //敵複数体を単体モンスターで攻撃するパターン
                }
                if(arrayBoard[j,i]==12){
                    //八方のマスを確認する
                    arrayAround8[0] = ReturnSquareStatus(i-1,j-1,arrayBoard,7);
                    arrayAround8[1] = ReturnSquareStatus(i-1,j,arrayBoard,6);
                    arrayAround8[2] = ReturnSquareStatus(i-1,j+1,arrayBoard,5);
                    arrayAround8[3] = ReturnSquareStatus(i,j-1,arrayBoard,4);
                    arrayAround8[4] = ReturnSquareStatus(i,j+1,arrayBoard,3);
                    arrayAround8[5] = ReturnSquareStatus(i+1,j-1,arrayBoard,2);
                    arrayAround8[6] = ReturnSquareStatus(i+1,j,arrayBoard,1);
                    arrayAround8[7] = ReturnSquareStatus(i+1,j+1,arrayBoard,0);

                    // //八方マスに攻撃モンスターが存在するか判定する
                    int ally1_index = Array.IndexOf(arrayAround8, 1);
                    int ally2_index = Array.IndexOf(arrayAround8, 2);
                    int ally3_index = Array.IndexOf(arrayAround8, 3);
                    int ally4_index = Array.IndexOf(arrayAround8, 4);

                    //何コンボが入るのかを計算
                    int combo = 0;

                    if(ally1_index!=-1){combo += 1;}
                    if(ally2_index!=-1){combo += 1;}
                    if(ally3_index!=-1){combo += 1;}
                    if(ally4_index!=-1){combo += 1;}

                    if(combo==0){
                        //攻撃は行われない
                    }else if(combo>1){
                        foreach(int I in arrayAround8){
                            switch(I){
                                case 1:
                                    SetAttackAnimationTrigger(Ally1Class,combo,ally1_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    break;
                                case 2:
                                    SetAttackAnimationTrigger(Ally2Class,combo,ally2_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    break;
                                case 3:
                                    SetAttackAnimationTrigger(Ally3Class,combo,ally3_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    break;
                                case 4:
                                    SetAttackAnimationTrigger(Ally4Class,combo,ally4_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    break;
                            }
                        }
                        yield return new WaitForSeconds(1.5f);
                    }
                    //敵一体を複数で囲むパターン

                    //敵一体を単体で攻撃するパターン
                        //敵複数体を単体モンスターで攻撃するパターン
                }
            }
        }
    }

    //座標のステータスを確認する関数
    int ReturnSquareStatus(int PointX, int PointY, int[,] arrayBoard, int attackIndex)
    {
        if(PointX >= 0 && PointX < 4 && PointY >= 0 && PointY < 4){
            int ObjectNum = arrayBoard[PointY,PointX];
            switch(ObjectNum){
                case 0:
                    return 0;
                case 1:
                    if(moveAlly1[attackIndex]!=0){
                        return 1;
                    }else{
                        return 0;
                    }
                case 2:
                    if(moveAlly2[attackIndex]!=0){
                        return 2;
                    }else{
                        return 0;
                    }
                case 3:
                    if(moveAlly3[attackIndex]!=0){
                        return 3;
                    }else{
                        return 0;
                    }
                case 4:
                    if(moveAlly4[attackIndex]!=0){
                        return 4;
                    }else{
                        return 0;
                    }
                case 11:
                    return 0;
                case 12:
                    return 0;
                default:
                    return 0;
            }
        }else{
            return 0;
        }
    }

    //コンボ数とオブジェクトからアニメーションを呼び出す関数
    void SetAttackAnimationTrigger(Animator AllyClass, int combo, int ally_index){
        switch(combo){
            case 1:
                AllyClass.SetTrigger("combo1");
                break;
            case 2:
                AllyClass.SetTrigger("combo2");
                break;
            case 3:
                AllyClass.SetTrigger("combo3");
                break;
            case 4:
                AllyClass.SetTrigger("combo4");
                break;
        }
        switch(ally_index){
            case 0:
                AllyClass.SetTrigger("RightDown");
                break;
            case 1:
                AllyClass.SetTrigger("Right");
                break;
            case 2:
                AllyClass.SetTrigger("RightUp");
                break;
            case 3:
                AllyClass.SetTrigger("Down");
                break;
            case 4:
                AllyClass.SetTrigger("Up");
                break;
            case 5:
                AllyClass.SetTrigger("LeftDown");
                break;
            case 6:
                AllyClass.SetTrigger("Left");
                break;
            case 7:
                AllyClass.SetTrigger("LeftUp");
                break;
        }
    }
}
