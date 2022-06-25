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
    public GameObject enemy1;
    Animator Enemy1Class;
    public GameObject enemy2;
    Animator Enemy2Class;

    //攻撃矢印があるかを判定するための配列
    int[] moveAlly1 = {1,2,1,0,0,2,2,2};
    int[] moveAlly2 = {1,2,1,1,0,1,2,1};
    int[] moveAlly3 = {1,1,1,0,0,1,1,1};
    int[] moveAlly4 = {1,2,0,1,1,0,2,0};

    int[] singleAttack11 = {0,10};
    int[] singleAttack12 = {0,10};

    void Start()
    {
        Ally1Class = ally1.GetComponent<Animator>();
        Ally2Class = ally2.GetComponent<Animator>();
        Ally3Class = ally3.GetComponent<Animator>();
        Ally4Class = ally4.GetComponent<Animator>();
        Enemy1Class = enemy1.GetComponent<Animator>();
        Enemy2Class = enemy2.GetComponent<Animator>();
    }

    public IEnumerator AllyDirectAttack(int[,] arrayBoard)
    {
        singleAttack11[0] = 0;
        singleAttack11[1] = 10;
        singleAttack12[0] = 0;
        singleAttack12[1] = 10;

        //周囲8マスのモンスターを格納する配列
        int[] arrayAround8 = new int[8];

        //コンボ攻撃を順に行う
        yield return StartCoroutine(AllyComboAttack(arrayBoard, arrayAround8, 11));
        yield return StartCoroutine(AllyComboAttack(arrayBoard, arrayAround8, 12));

        //単体攻撃の判定を行う
        //単体攻撃がない場合
        if(singleAttack11[0]==0&&singleAttack12[0]==0){
            // yield break;
        }
        //同一モンスターが２体同時攻撃を行う場合
        else if(singleAttack11[0]==singleAttack12[0]){
            SetSingleAttackAnimationTrigger(singleAttack11[0], 8);
            yield return new WaitForSeconds(0.9f);
            Enemy1Class.SetTrigger("defence");
            Enemy2Class.SetTrigger("defence");
            yield return new WaitForSeconds(0.5f);
        }
        //単体->単体の攻撃が行われる場合
        else{
            if(singleAttack11[0]!=0){
                SetSingleAttackAnimationTrigger(singleAttack11[0], singleAttack11[1]);
                yield return new WaitForSeconds(0.8f);
                Enemy1Class.SetTrigger("defence");
                yield return new WaitForSeconds(0.5f);
            }
            if(singleAttack12[0]!=0){
                SetSingleAttackAnimationTrigger(singleAttack12[0], singleAttack12[1]);
                yield return new WaitForSeconds(0.8f);
                Enemy2Class.SetTrigger("defence");
                yield return new WaitForSeconds(0.5f);
            }
        }
        
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator AllyComboAttack(int[,] arrayBoard, int[] arrayAround8, int EnemyNum)
    {
        for(int i=0; i<4; i++){
            for(int j=0; j<4; j++){
                if(arrayBoard[j,i]==EnemyNum){
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
                    }
                    //複数体によるコンボ攻撃が行われる場合
                    else if(combo>1){
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
                        yield return new WaitForSeconds(0.8f);
                        switch (EnemyNum) {
                            case 11:
                                Enemy1Class.SetTrigger("defence");
                                break;
                            case 12:
                                Enemy2Class.SetTrigger("defence");
                                break;
                        }
                        yield return new WaitForSeconds(1);
                    }
                    else if(combo==1){
                        //単体の攻撃が行われる場合
                        switch (EnemyNum) {
                            case 11:
                                if(ally1_index!=-1){
                                    singleAttack11[0] = 1;
                                    singleAttack11[1] = ally1_index;
                                }
                                if(ally2_index!=-1){
                                    singleAttack11[0] = 2;
                                    singleAttack11[1] = ally2_index;
                                }
                                if(ally3_index!=-1){
                                    singleAttack11[0] = 3;
                                    singleAttack11[1] = ally3_index;
                                }
                                if(ally4_index!=-1){
                                    singleAttack11[0] = 4;
                                    singleAttack11[1] = ally4_index;
                                }
                                break;
                            case 12:
                                if(ally1_index!=-1){
                                    singleAttack12[0] = 1;
                                    singleAttack12[1] = ally1_index;
                                }
                                if(ally2_index!=-1){
                                    singleAttack12[0] = 2;
                                    singleAttack12[1] = ally2_index;
                                }
                                if(ally3_index!=-1){
                                    singleAttack12[0] = 3;
                                    singleAttack12[1] = ally3_index;
                                }
                                if(ally4_index!=-1){
                                    singleAttack12[0] = 4;
                                    singleAttack12[1] = ally4_index;
                                }
                                break;
                        }
                    }
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
    void SetAttackAnimationTrigger(Animator AllyClass, int combo, int ally_index)
    {
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

    //単体攻撃のアニメーションを呼び出す関数
    void SetSingleAttackAnimationTrigger(int singleAttack, int ally_index)
    {
        switch(singleAttack){
            case 1:
                Ally1Class.SetTrigger("attack");
                switch(ally_index){
                    case 0:
                        Ally1Class.SetTrigger("RightDown");
                        return;
                    case 1:
                        Ally1Class.SetTrigger("Right");
                        return;
                    case 2:
                        Ally1Class.SetTrigger("RightUp");
                        return;
                    case 3:
                        Ally1Class.SetTrigger("Down");
                        return;
                    case 4:
                        Ally1Class.SetTrigger("Up");
                        return;
                    case 5:
                        Ally1Class.SetTrigger("LeftDown");
                        return;
                    case 6:
                        Ally1Class.SetTrigger("Left");
                        return;
                    case 7:
                        Ally1Class.SetTrigger("LeftUp");
                        return;
                    case 8:
                        Ally1Class.SetTrigger("MultiDirection");
                        return;
                }
                return;
            case 2:
                Ally2Class.SetTrigger("attack");
                switch(ally_index){
                    case 0:
                        Ally2Class.SetTrigger("RightDown");
                        return;
                    case 1:
                        Ally2Class.SetTrigger("Right");
                        return;
                    case 2:
                        Ally2Class.SetTrigger("RightUp");
                        return;
                    case 3:
                        Ally2Class.SetTrigger("Down");
                        return;
                    case 4:
                        Ally2Class.SetTrigger("Up");
                        return;
                    case 5:
                        Ally2Class.SetTrigger("LeftDown");
                        return;
                    case 6:
                        Ally2Class.SetTrigger("Left");
                        return;
                    case 7:
                        Ally2Class.SetTrigger("LeftUp");
                        return;
                    case 8:
                        Ally2Class.SetTrigger("MultiDirection");
                        return;
                }
                return;
            case 3:
                Ally3Class.SetTrigger("attack");
                switch(ally_index){
                    case 0:
                        Ally3Class.SetTrigger("RightDown");
                        return;
                    case 1:
                        Ally3Class.SetTrigger("Right");
                        return;
                    case 2:
                        Ally3Class.SetTrigger("RightUp");
                        return;
                    case 3:
                        Ally3Class.SetTrigger("Down");
                        return;
                    case 4:
                        Ally3Class.SetTrigger("Up");
                        return;
                    case 5:
                        Ally3Class.SetTrigger("LeftDown");
                        return;
                    case 6:
                        Ally3Class.SetTrigger("Left");
                        return;
                    case 7:
                        Ally3Class.SetTrigger("LeftUp");
                        return;
                    case 8:
                        Ally3Class.SetTrigger("MultiDirection");
                        return;
                }
                return;
            case 4:
                Ally4Class.SetTrigger("attack");
                switch(ally_index){
                    case 0:
                        Ally4Class.SetTrigger("RightDown");
                        return;
                    case 1:
                        Ally4Class.SetTrigger("Right");
                        return;
                    case 2:
                        Ally4Class.SetTrigger("RightUp");
                        return;
                    case 3:
                        Ally4Class.SetTrigger("Down");
                        return;
                    case 4:
                        Ally4Class.SetTrigger("Up");
                        return;
                    case 5:
                        Ally4Class.SetTrigger("LeftDown");
                        return;
                    case 6:
                        Ally4Class.SetTrigger("Left");
                        return;
                    case 7:
                        Ally4Class.SetTrigger("LeftUp");
                        return;
                    case 8:
                        Ally4Class.SetTrigger("MultiDirection");
                        return;
                }
                return;
        }
    }
}
