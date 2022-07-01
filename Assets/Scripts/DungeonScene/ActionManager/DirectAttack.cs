using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class DirectAttack : MonoBehaviour
{
    public GameObject SurviveManager;
    SurviveManager SurviveManagerClass;

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
    public GameObject enemy3;
    Animator Enemy3Class;
    public GameObject enemy4;
    Animator Enemy4Class;

    //攻撃矢印があるかを判定するための配列
    int[] moveAlly1 = {1,2,1,0,0,2,2,2};
    int[] moveAlly2 = {1,2,1,1,0,1,2,1};
    int[] moveAlly3 = {1,1,1,0,0,1,1,1};
    int[] moveAlly4 = {1,2,0,1,1,0,2,0};
    int[] moveEnemy1 = {0,1,0,0,0,2,0,2};
    int[] moveEnemy2 = {0,1,0,1,1,0,1,0};
    int[] moveEnemy3 = {0,2,0,1,1,0,2,0};
    int[] moveEnemy4 = {0,2,0,1,1,0,2,0};

    int[] singleAttack1 = {0,10};
    int[] singleAttack2 = {0,10};
    int[] singleAttack3 = {0,10};
    int[] singleAttack4 = {0,10};
    int[] singleAttack11 = {0,10};
    int[] singleAttack12 = {0,10};
    int[] singleAttack13 = {0,10};
    int[] singleAttack14 = {0,10};

    //モンスターの攻撃力
    int PowerOffenceAlly1 = 200;
    int PowerOffenceAlly2 = 200;
    int PowerOffenceAlly3 = 200;
    int PowerOffenceAlly4 = 200;
    int PowerOffenceEnemy1 = 100;
    int PowerOffenceEnemy2 = 100;
    int PowerOffenceEnemy3 = 150;
    int PowerOffenceEnemy4 = 150;

    //モンスターの防御力
    int PowerDefenceAlly1 = 100;
    int PowerDefenceAlly2 = 100;
    int PowerDefenceAlly3 = 100;
    int PowerDefenceAlly4 = 100;
    int PowerDefenceEnemy1 = 100;
    int PowerDefenceEnemy2 = 500;
    int PowerDefenceEnemy3 = 50;
    int PowerDefenceEnemy4 = 50;

    void Start()
    {
        SurviveManagerClass = SurviveManager.GetComponent<SurviveManager>();

        Ally1Class = ally1.GetComponent<Animator>();
        Ally2Class = ally2.GetComponent<Animator>();
        Ally3Class = ally3.GetComponent<Animator>();
        Ally4Class = ally4.GetComponent<Animator>();
        Enemy1Class = enemy1.GetComponent<Animator>();
        Enemy2Class = enemy2.GetComponent<Animator>();
        Enemy3Class = enemy3.GetComponent<Animator>();
        Enemy4Class = enemy4.GetComponent<Animator>();
    }

    public IEnumerator AllyDirectAttack1stArea(int[,] arrayBoard)
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
        }else{
            //ダメージ計算関数に渡すパラメータ
            int AttackAlly1 = 0;
            int AttackAlly2 = 0;
            int AttackAlly3 = 0;
            int AttackAlly4 = 0;

            //同一モンスターが２体同時攻撃を行う場合
            if(singleAttack11[0]==singleAttack12[0]){
                SetSingleAttackAnimationTrigger(singleAttack11[0], 8);
                yield return new WaitForSeconds(0.9f);
                Enemy1Class.SetTrigger("defence");
                Enemy2Class.SetTrigger("defence");
                switch(singleAttack11[0]){
                    case 1:
                        AttackAlly1 = 1;
                        break;
                    case 2:
                        AttackAlly2 = 1;
                        break;
                    case 3:
                        AttackAlly3 = 1;
                        break;
                    case 4:
                        AttackAlly4 = 1;
                        break;
                }
                yield return new WaitForSeconds(0.1f);
                DamageCalculationEnemy(11,AttackAlly1,AttackAlly2,AttackAlly3,AttackAlly4);
                DamageCalculationEnemy(12,AttackAlly1,AttackAlly2,AttackAlly3,AttackAlly4);
                yield return new WaitForSeconds(0.4f);
            }
            //単体->単体の攻撃が行われる場合
            else{
                if(singleAttack11[0]!=0){
                    SetSingleAttackAnimationTrigger(singleAttack11[0], singleAttack11[1]);
                    yield return new WaitForSeconds(0.8f);
                    Enemy1Class.SetTrigger("defence");
                    switch(singleAttack11[0]){
                        case 1:
                            AttackAlly1 = 1;
                            break;
                        case 2:
                            AttackAlly2 = 1;
                            break;
                        case 3:
                            AttackAlly3 = 1;
                            break;
                        case 4:
                            AttackAlly4 = 1;
                            break;
                    }
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationEnemy(11,AttackAlly1,AttackAlly2,AttackAlly3,AttackAlly4);
                    yield return new WaitForSeconds(0.4f);
                }
                if(singleAttack12[0]!=0){
                    SetSingleAttackAnimationTrigger(singleAttack12[0], singleAttack12[1]);
                    yield return new WaitForSeconds(0.8f);
                    Enemy2Class.SetTrigger("defence");
                    switch(singleAttack12[0]){
                        case 1:
                            AttackAlly1 = 1;
                            break;
                        case 2:
                            AttackAlly2 = 1;
                            break;
                        case 3:
                            AttackAlly3 = 1;
                            break;
                        case 4:
                            AttackAlly4 = 1;
                            break;
                    }
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationEnemy(12,AttackAlly1,AttackAlly2,AttackAlly3,AttackAlly4);
                    yield return new WaitForSeconds(0.4f);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator AllyDirectAttack2ndArea(int[,] arrayBoard)
    {
        singleAttack13[0] = 0;
        singleAttack13[1] = 10;
        singleAttack14[0] = 0;
        singleAttack14[1] = 10;

        //周囲8マスのモンスターを格納する配列
        int[] arrayAround8 = new int[8];

        //コンボ攻撃を順に行う
        yield return StartCoroutine(AllyComboAttack(arrayBoard, arrayAround8, 13));
        yield return StartCoroutine(AllyComboAttack(arrayBoard, arrayAround8, 14));

        //単体攻撃の判定を行う
        //単体攻撃がない場合
        if(singleAttack13[0]==0&&singleAttack14[0]==0){
            // yield break;
        }else{
            //ダメージ計算関数に渡すパラメータ
            int AttackAlly1 = 0;
            int AttackAlly2 = 0;
            int AttackAlly3 = 0;
            int AttackAlly4 = 0;

            //同一モンスターが２体同時攻撃を行う場合
            if(singleAttack13[0]==singleAttack14[0]){
                SetSingleAttackAnimationTrigger(singleAttack13[0], 8);
                yield return new WaitForSeconds(0.9f);
                Enemy3Class.SetTrigger("defence");
                Enemy4Class.SetTrigger("defence");
                switch(singleAttack13[0]){
                    case 1:
                        AttackAlly1 = 1;
                        break;
                    case 2:
                        AttackAlly2 = 1;
                        break;
                    case 3:
                        AttackAlly3 = 1;
                        break;
                    case 4:
                        AttackAlly4 = 1;
                        break;
                }
                yield return new WaitForSeconds(0.1f);
                DamageCalculationEnemy(13,AttackAlly1,AttackAlly2,AttackAlly3,AttackAlly4);
                DamageCalculationEnemy(14,AttackAlly1,AttackAlly2,AttackAlly3,AttackAlly4);
                yield return new WaitForSeconds(0.4f);
            }
            //単体->単体の攻撃が行われる場合
            else{
                if(singleAttack13[0]!=0){
                    SetSingleAttackAnimationTrigger(singleAttack13[0], singleAttack13[1]);
                    yield return new WaitForSeconds(0.8f);
                    Enemy3Class.SetTrigger("defence");
                    switch(singleAttack13[0]){
                        case 1:
                            AttackAlly1 = 1;
                            break;
                        case 2:
                            AttackAlly2 = 1;
                            break;
                        case 3:
                            AttackAlly3 = 1;
                            break;
                        case 4:
                            AttackAlly4 = 1;
                            break;
                    }
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationEnemy(13,AttackAlly1,AttackAlly2,AttackAlly3,AttackAlly4);
                    yield return new WaitForSeconds(0.4f);
                }
                if(singleAttack14[0]!=0){
                    SetSingleAttackAnimationTrigger(singleAttack14[0], singleAttack14[1]);
                    yield return new WaitForSeconds(0.8f);
                    Enemy4Class.SetTrigger("defence");
                    switch(singleAttack14[0]){
                        case 1:
                            AttackAlly1 = 1;
                            break;
                        case 2:
                            AttackAlly2 = 1;
                            break;
                        case 3:
                            AttackAlly3 = 1;
                            break;
                        case 4:
                            AttackAlly4 = 1;
                            break;
                    }
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationEnemy(14,AttackAlly1,AttackAlly2,AttackAlly3,AttackAlly4);
                    yield return new WaitForSeconds(0.4f);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator EnemyDirectAttack(int[,] arrayBoard)
    {
        singleAttack1[0] = 0;
        singleAttack1[1] = 10;
        singleAttack2[0] = 0;
        singleAttack2[1] = 10;
        singleAttack3[0] = 0;
        singleAttack3[1] = 10;
        singleAttack4[0] = 0;
        singleAttack4[1] = 10;

        //周囲8マスのモンスターを格納する配列
        int[] arrayAround8 = new int[8];

        //コンボ攻撃を順に行う
        yield return StartCoroutine(EnemyComboAttack(arrayBoard, arrayAround8, 1));
        yield return StartCoroutine(EnemyComboAttack(arrayBoard, arrayAround8, 2));
        yield return StartCoroutine(EnemyComboAttack(arrayBoard, arrayAround8, 3));
        yield return StartCoroutine(EnemyComboAttack(arrayBoard, arrayAround8, 4));

        //単体攻撃の判定を行う
        //単体攻撃がない場合
        if(singleAttack1[0]==0&&singleAttack2[0]==0&&singleAttack3[0]==0&&singleAttack4[0]==0){
            // yield break;
        }else{
            //ダメージ計算関数に渡すパラメータ
            int AttackEnemy1 = 0;
            int AttackEnemy2 = 0;
            int AttackEnemy3 = 0;
            int AttackEnemy4 = 0;

            //同一モンスターが２体同時攻撃を行う場合

            //攻撃を受けるモンスターのリストを作成し重複を抽出
            List<int> SingleAttackCheck = new List<int>(){
                singleAttack1[0],
                singleAttack2[0],
                singleAttack3[0],
                singleAttack4[0]
            };

            int duplicates11 = SingleAttackCheck.Count(s => s == 11);
            int duplicates12 = SingleAttackCheck.Count(s => s == 12);
            int duplicates13 = SingleAttackCheck.Count(s => s == 13);
            int duplicates14 = SingleAttackCheck.Count(s => s == 14);


            //enemy1が攻撃を行う場合
            if(duplicates11 > 1){
                SetSingleAttackAnimationTrigger(11, 8);
                yield return new WaitForSeconds(0.9f);
                if(singleAttack1[0]==11){
                    Ally1Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(1,1,0,0,0);
                }
                if(singleAttack2[0]==11){
                    Ally2Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(2,1,0,0,0);
                }
                if(singleAttack3[0]==11){
                    Ally3Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(3,1,0,0,0);
                }
                if(singleAttack4[0]==11){
                    Ally4Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(4,1,0,0,0);
                }
                yield return new WaitForSeconds(0.4f);
            }
            //enemy1が単体に行う攻撃
            else{
                if(singleAttack1[0]==11){
                    SetSingleAttackAnimationTrigger(11, singleAttack1[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally1Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(1,1,0,0,0);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack2[0]==11){
                    SetSingleAttackAnimationTrigger(11, singleAttack2[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally2Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(2,1,0,0,0);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack3[0]==11){
                    SetSingleAttackAnimationTrigger(11, singleAttack3[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally3Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(3,1,0,0,0);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack4[0]==11){
                    SetSingleAttackAnimationTrigger(11, singleAttack4[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally4Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(4,1,0,0,0);
                    yield return new WaitForSeconds(0.4f);
                }
            }
            //enemy2
            if(duplicates12 > 1){
                SetSingleAttackAnimationTrigger(12, 8);
                yield return new WaitForSeconds(0.9f);
                if(singleAttack1[0]==12){
                    Ally1Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(1,0,1,0,0);
                }
                if(singleAttack2[0]==12){
                    Ally2Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(2,0,1,0,0);
                }
                if(singleAttack3[0]==12){
                    Ally3Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(3,0,1,0,0);
                }
                if(singleAttack4[0]==12){
                    Ally4Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(4,0,1,0,0);
                }
                yield return new WaitForSeconds(0.4f);
            }else{
                if(singleAttack1[0]==12){
                    SetSingleAttackAnimationTrigger(12, singleAttack1[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally1Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(1,0,1,0,0);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack2[0]==12){
                    SetSingleAttackAnimationTrigger(12, singleAttack2[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally2Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(2,0,1,0,0);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack3[0]==12){
                    SetSingleAttackAnimationTrigger(12, singleAttack3[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally3Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(3,0,1,0,0);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack4[0]==12){
                    SetSingleAttackAnimationTrigger(12, singleAttack4[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally4Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(4,0,1,0,0);
                    yield return new WaitForSeconds(0.4f);
                }
            }
            //enemy3
            if(duplicates13 > 1){
                SetSingleAttackAnimationTrigger(13, 8);
                yield return new WaitForSeconds(0.9f);
                if(singleAttack1[0]==13){
                    Ally1Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(1,0,0,1,0);
                }
                if(singleAttack2[0]==13){
                    Ally2Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(2,0,0,1,0);
                }
                if(singleAttack3[0]==13){
                    Ally3Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(3,0,0,1,0);
                }
                if(singleAttack4[0]==13){
                    Ally4Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(4,0,0,1,0);
                }
                yield return new WaitForSeconds(0.4f);
            }else{
                if(singleAttack1[0]==13){
                    SetSingleAttackAnimationTrigger(13, singleAttack1[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally1Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(1,0,0,1,0);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack2[0]==13){
                    SetSingleAttackAnimationTrigger(13, singleAttack2[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally2Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(2,0,0,1,0);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack3[0]==13){
                    SetSingleAttackAnimationTrigger(13, singleAttack3[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally3Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(3,0,0,1,0);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack4[0]==13){
                    SetSingleAttackAnimationTrigger(13, singleAttack4[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally4Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(4,0,0,1,0);
                    yield return new WaitForSeconds(0.4f);
                }
            }
            //enemy4
            if(duplicates14 > 1){
                SetSingleAttackAnimationTrigger(14, 8);
                yield return new WaitForSeconds(0.9f);
                if(singleAttack1[0]==14){
                    Ally1Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(1,0,0,0,1);
                }
                if(singleAttack2[0]==14){
                    Ally2Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(2,0,0,0,1);
                }
                if(singleAttack3[0]==14){
                    Ally3Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(3,0,0,0,1);
                }
                if(singleAttack4[0]==14){
                    Ally4Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(4,0,0,0,1);
                }
                yield return new WaitForSeconds(0.4f);
            }else{
                if(singleAttack1[0]==14){
                    SetSingleAttackAnimationTrigger(14, singleAttack1[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally1Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(1,0,0,0,1);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack2[0]==14){
                    SetSingleAttackAnimationTrigger(14, singleAttack2[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally2Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(2,0,0,0,1);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack3[0]==14){
                    SetSingleAttackAnimationTrigger(14, singleAttack3[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally3Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(3,0,0,0,1);
                    yield return new WaitForSeconds(0.4f);
                }else if(singleAttack4[0]==14){
                    SetSingleAttackAnimationTrigger(14, singleAttack4[1]);
                    yield return new WaitForSeconds(0.8f);
                    Ally4Class.SetTrigger("defence");
                    yield return new WaitForSeconds(0.1f);
                    DamageCalculationAlly(4,0,0,0,1);
                    yield return new WaitForSeconds(0.4f);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator AllyComboAttack(int[,] arrayBoard, int[] arrayAround8, int EnemyNum)
    {
        //ダメージ計算関数に渡すパラメータ
        int AttackAlly1 = 0;
        int AttackAlly2 = 0;
        int AttackAlly3 = 0;
        int AttackAlly4 = 0;

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
                                    AttackAlly1 = 1;
                                    break;
                                case 2:
                                    SetAttackAnimationTrigger(Ally2Class,combo,ally2_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    AttackAlly2 = 1;
                                    break;
                                case 3:
                                    SetAttackAnimationTrigger(Ally3Class,combo,ally3_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    AttackAlly3 = 1;
                                    break;
                                case 4:
                                    SetAttackAnimationTrigger(Ally4Class,combo,ally4_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    AttackAlly4 = 1;
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
                            case 13:
                                Enemy3Class.SetTrigger("defence");
                                break;
                            case 14:
                                Enemy4Class.SetTrigger("defence");
                                break;
                        }
                        yield return new WaitForSeconds(0.1f);
                        DamageCalculationEnemy(EnemyNum,AttackAlly1,AttackAlly2,AttackAlly3,AttackAlly4);
                        yield return new WaitForSeconds(0.9f);
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
                            case 13:
                                if(ally1_index!=-1){
                                    singleAttack13[0] = 1;
                                    singleAttack13[1] = ally1_index;
                                }
                                if(ally2_index!=-1){
                                    singleAttack13[0] = 2;
                                    singleAttack13[1] = ally2_index;
                                }
                                if(ally3_index!=-1){
                                    singleAttack13[0] = 3;
                                    singleAttack13[1] = ally3_index;
                                }
                                if(ally4_index!=-1){
                                    singleAttack13[0] = 4;
                                    singleAttack13[1] = ally4_index;
                                }
                                break;
                            case 14:
                                if(ally1_index!=-1){
                                    singleAttack14[0] = 1;
                                    singleAttack14[1] = ally1_index;
                                }
                                if(ally2_index!=-1){
                                    singleAttack14[0] = 2;
                                    singleAttack14[1] = ally2_index;
                                }
                                if(ally3_index!=-1){
                                    singleAttack14[0] = 3;
                                    singleAttack14[1] = ally3_index;
                                }
                                if(ally4_index!=-1){
                                    singleAttack14[0] = 4;
                                    singleAttack14[1] = ally4_index;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    IEnumerator EnemyComboAttack(int[,] arrayBoard, int[] arrayAround8, int EnemyNum)
    {
        //ダメージ計算関数に渡すパラメータ
        int AttackEnemy1 = 0;
        int AttackEnemy2 = 0;
        int AttackEnemy3 = 0;
        int AttackEnemy4 = 0;

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
                    int enemy1_index = Array.IndexOf(arrayAround8, 11);
                    int enemy2_index = Array.IndexOf(arrayAround8, 12);
                    int enemy3_index = Array.IndexOf(arrayAround8, 13);
                    int enemy4_index = Array.IndexOf(arrayAround8, 14);

                    //何コンボが入るのかを計算
                    int combo = 0;

                    if(enemy1_index!=-1){combo += 1;}
                    if(enemy2_index!=-1){combo += 1;}
                    if(enemy3_index!=-1){combo += 1;}
                    if(enemy4_index!=-1){combo += 1;}

                    if(combo==0){
                        //攻撃は行われない
                    }
                    //複数体によるコンボ攻撃が行われる場合
                    else if(combo>1){
                        foreach(int I in arrayAround8){
                            switch(I){
                                case 11:
                                    SetAttackAnimationTrigger(Enemy1Class,combo,enemy1_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    AttackEnemy1 = 1;
                                    break;
                                case 12:
                                    SetAttackAnimationTrigger(Enemy2Class,combo,enemy2_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    AttackEnemy2 = 1;
                                    break;
                                case 13:
                                    SetAttackAnimationTrigger(Enemy3Class,combo,enemy3_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    AttackEnemy3 = 1;
                                    break;
                                case 14:
                                    SetAttackAnimationTrigger(Enemy4Class,combo,enemy4_index);
                                    yield return new WaitForSeconds(0.33f);
                                    combo -= 1;
                                    AttackEnemy4 = 1;
                                    break;
                            }
                        }
                        yield return new WaitForSeconds(0.8f);
                        switch (EnemyNum) {
                            case 1:
                                Ally1Class.SetTrigger("defence");
                                break;
                            case 2:
                                Ally2Class.SetTrigger("defence");
                                break;
                            case 3:
                                Ally3Class.SetTrigger("defence");
                                break;
                            case 4:
                                Ally4Class.SetTrigger("defence");
                                break;
                        }
                        yield return new WaitForSeconds(0.1f);
                        DamageCalculationAlly(EnemyNum,AttackEnemy1,AttackEnemy2,AttackEnemy3,AttackEnemy4);
                        yield return new WaitForSeconds(0.9f);
                    }
                    else if(combo==1){
                        //単体の攻撃が行われる場合
                        switch (EnemyNum) {
                            case 1:
                                if(enemy1_index!=-1){
                                    singleAttack1[0] = 11;
                                    singleAttack1[1] = enemy1_index;
                                }
                                if(enemy2_index!=-1){
                                    singleAttack1[0] = 12;
                                    singleAttack1[1] = enemy2_index;
                                }
                                if(enemy3_index!=-1){
                                    singleAttack1[0] = 13;
                                    singleAttack1[1] = enemy3_index;
                                }
                                if(enemy4_index!=-1){
                                    singleAttack1[0] = 14;
                                    singleAttack1[1] = enemy4_index;
                                }
                                break;
                            case 2:
                                if(enemy1_index!=-1){
                                    singleAttack2[0] = 11;
                                    singleAttack2[1] = enemy1_index;
                                }
                                if(enemy2_index!=-1){
                                    singleAttack2[0] = 12;
                                    singleAttack2[1] = enemy2_index;
                                }
                                if(enemy3_index!=-1){
                                    singleAttack2[0] = 13;
                                    singleAttack2[1] = enemy3_index;
                                }
                                if(enemy4_index!=-1){
                                    singleAttack2[0] = 14;
                                    singleAttack2[1] = enemy4_index;
                                }
                                break;
                            case 3:
                                if(enemy1_index!=-1){
                                    singleAttack3[0] = 11;
                                    singleAttack3[1] = enemy1_index;
                                }
                                if(enemy2_index!=-1){
                                    singleAttack3[0] = 12;
                                    singleAttack3[1] = enemy2_index;
                                }
                                if(enemy3_index!=-1){
                                    singleAttack3[0] = 13;
                                    singleAttack3[1] = enemy3_index;
                                }
                                if(enemy4_index!=-1){
                                    singleAttack3[0] = 14;
                                    singleAttack3[1] = enemy4_index;
                                }
                                break;
                            case 4:
                                if(enemy1_index!=-1){
                                    singleAttack4[0] = 11;
                                    singleAttack4[1] = enemy1_index;
                                }
                                if(enemy2_index!=-1){
                                    singleAttack4[0] = 12;
                                    singleAttack4[1] = enemy2_index;
                                }
                                if(enemy3_index!=-1){
                                    singleAttack4[0] = 13;
                                    singleAttack4[1] = enemy3_index;
                                }
                                if(enemy4_index!=-1){
                                    singleAttack4[0] = 14;
                                    singleAttack4[1] = enemy4_index;
                                }
                                break;
                        }
                    }

                //ココアとで消します
                yield return null;
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
                    if(moveEnemy1[attackIndex]!=0){
                        return 11;
                    }else{
                        return 0;
                    }
                case 12:
                    if(moveEnemy2[attackIndex]!=0){
                        return 12;
                    }else{
                        return 0;
                    }
                case 13:
                    if(moveEnemy3[attackIndex]!=0){
                        return 13;
                    }else{
                        return 0;
                    }
                case 14:
                    if(moveEnemy4[attackIndex]!=0){
                        return 14;
                    }else{
                        return 0;
                    }
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
            case 11:
                Enemy1Class.SetTrigger("attack");
                switch(ally_index){
                    case 0:
                        Enemy1Class.SetTrigger("RightDown");
                        return;
                    case 1:
                        Enemy1Class.SetTrigger("Right");
                        return;
                    case 2:
                        Enemy1Class.SetTrigger("RightUp");
                        return;
                    case 3:
                        Enemy1Class.SetTrigger("Down");
                        return;
                    case 4:
                        Enemy1Class.SetTrigger("Up");
                        return;
                    case 5:
                        Enemy1Class.SetTrigger("LeftDown");
                        return;
                    case 6:
                        Enemy1Class.SetTrigger("Left");
                        return;
                    case 7:
                        Enemy1Class.SetTrigger("LeftUp");
                        return;
                    case 8:
                        Enemy1Class.SetTrigger("MultiDirection");
                        return;
                }
                return;
            case 12:
                Enemy2Class.SetTrigger("attack");
                switch(ally_index){
                    case 0:
                        Enemy2Class.SetTrigger("RightDown");
                        return;
                    case 1:
                        Enemy2Class.SetTrigger("Right");
                        return;
                    case 2:
                        Enemy2Class.SetTrigger("RightUp");
                        return;
                    case 3:
                        Enemy2Class.SetTrigger("Down");
                        return;
                    case 4:
                        Enemy2Class.SetTrigger("Up");
                        return;
                    case 5:
                        Enemy2Class.SetTrigger("LeftDown");
                        return;
                    case 6:
                        Enemy2Class.SetTrigger("Left");
                        return;
                    case 7:
                        Enemy2Class.SetTrigger("LeftUp");
                        return;
                    case 8:
                        Enemy2Class.SetTrigger("MultiDirection");
                        return;
                }
                return;
            case 13:
                Enemy3Class.SetTrigger("attack");
                switch(ally_index){
                    case 0:
                        Enemy3Class.SetTrigger("RightDown");
                        return;
                    case 1:
                        Enemy3Class.SetTrigger("Right");
                        return;
                    case 2:
                        Enemy3Class.SetTrigger("RightUp");
                        return;
                    case 3:
                        Enemy3Class.SetTrigger("Down");
                        return;
                    case 4:
                        Enemy3Class.SetTrigger("Up");
                        return;
                    case 5:
                        Enemy3Class.SetTrigger("LeftDown");
                        return;
                    case 6:
                        Enemy3Class.SetTrigger("Left");
                        return;
                    case 7:
                        Enemy3Class.SetTrigger("LeftUp");
                        return;
                    case 8:
                        Enemy3Class.SetTrigger("MultiDirection");
                        return;
                }
                return;
            case 14:
                Enemy4Class.SetTrigger("attack");
                switch(ally_index){
                    case 0:
                        Enemy4Class.SetTrigger("RightDown");
                        return;
                    case 1:
                        Enemy4Class.SetTrigger("Right");
                        return;
                    case 2:
                        Enemy4Class.SetTrigger("RightUp");
                        return;
                    case 3:
                        Enemy4Class.SetTrigger("Down");
                        return;
                    case 4:
                        Enemy4Class.SetTrigger("Up");
                        return;
                    case 5:
                        Enemy4Class.SetTrigger("LeftDown");
                        return;
                    case 6:
                        Enemy4Class.SetTrigger("Left");
                        return;
                    case 7:
                        Enemy4Class.SetTrigger("LeftUp");
                        return;
                    case 8:
                        Enemy4Class.SetTrigger("MultiDirection");
                        return;
                }
                return;
        }
    }

    //ダメージ計算関数
    //必要なパラメータは以下
    //  ダメージを受けるモンスター
    //  ダメージを与えるモンスター
    void DamageCalculationEnemy(int Wounded, int AttackAlly1, int AttackAlly2, int AttackAlly3, int AttackAlly4)
    {
        //3アタック->*1.00
        //6コンボ->*1.75
        //9コンボ->*2.13
        //12コンボ->*2.50
        int combo = (AttackAlly1+AttackAlly2+AttackAlly3+AttackAlly4)*3;
        Debug.Log(combo);

        double damage = PowerOffenceAlly1*AttackAlly1*3+PowerOffenceAlly2*AttackAlly2*3+PowerOffenceAlly3*AttackAlly3*3+PowerOffenceAlly4*AttackAlly4*3;
        damage = damage*((combo-3)*0.125+1);
        int Damage = Convert.ToInt32(damage);

        switch(Wounded){
            case 11:
                Damage -= PowerDefenceEnemy1;
                break;
            case 12:
                Damage -= PowerDefenceEnemy2;
                break;
            case 13:
                Damage -= PowerDefenceEnemy3;
                break;
            case 14:
                Damage -= PowerDefenceEnemy4;
                break;
        }
        if(Damage < 0){
            Damage = 0;
        }
        Debug.Log(Damage);

        SurviveManagerClass.HPdamage(Wounded,Damage);
    }

    void DamageCalculationAlly(int Wounded, int AttackEnemy1, int AttackEnemy2, int AttackEnemy3, int AttackEnemy4)
    {
        //3アタック->*1.00
        //6コンボ->*1.75
        //9コンボ->*2.13
        //12コンボ->*2.50
        int combo = (AttackEnemy1+AttackEnemy2+AttackEnemy3+AttackEnemy4)*3;
        Debug.Log(combo);

        double damage = PowerOffenceEnemy1*AttackEnemy1*3+PowerOffenceEnemy2*AttackEnemy2*3+PowerOffenceEnemy3*AttackEnemy3*3+PowerOffenceEnemy4*AttackEnemy4*3;
        damage = damage*((combo-3)*0.125+1);
        int Damage = Convert.ToInt32(damage);

        switch(Wounded){
            case 1:
                Damage -= PowerDefenceEnemy1;
                break;
            case 2:
                Damage -= PowerDefenceEnemy2;
                break;
            case 3:
                Damage -= PowerDefenceEnemy3;
                break;
            case 4:
                Damage -= PowerDefenceEnemy4;
                break;
        }
        if(Damage < 0){
            Damage = 0;
        }
        Debug.Log(Damage);

        SurviveManagerClass.HPdamage(Wounded,Damage);
    }
}
