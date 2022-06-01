using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEnemy : MonoBehaviour
{
    public GameObject BoardSurface;
    public GameObject enemy1;
    public GameObject enemy2;

    //マス目に対応するlocal座標を配列としたもの
    float[] PointValueXArray = {-105, -35, 35, 105};
    float[] PointValueYArray = {105, 35, -35, -105};

    //関数：相手ターンの一連の処理を行う関数
    public bool ActionEnemyTurn()
    {

        int EnemyNum = 0;

        int EnemyPointX = 10;
        int EnemyPointY = 10;

        int[,] MovableSquare = new int[9,2];

        int[,] arrayBoard = BoardSurface.GetComponent<BoardSurface>().arrayBoard;
        GameObject? allyDrag = null;
        
        //Enemyの中で一番X座標が小さい駒を取得
        for(int i=0; i<4; i++){
            for(int j=0; j<4; j++){
                if (arrayBoard[j,i]==11 || arrayBoard[j,i]==12){
                    EnemyNum = arrayBoard[j,i];
                    EnemyPointX = i;
                    EnemyPointY = j;
                    //取得した駒のフォーカスエフェクトを呼び出す
                    if (EnemyNum==11){
                        allyDrag = enemy1;
                    } else if (EnemyNum==12){
                        allyDrag = enemy2;
                    }
                    BoardSurface.GetComponent<BoardSurface>().GenerateAvailableSquares(EnemyPointX,EnemyPointY,false);

                    //取得した駒のアニメーションを実行する
                    allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("right1");

                    //アニメーション終了後の処理
                    StartCoroutine(ProcessAfterAnimation(EnemyPointX,EnemyPointY,EnemyNum,allyDrag));

                    break;
                }
            }
            if(EnemyNum != 0){
                break;
            }
        }
        Debug.Log(EnemyNum);
        return true;
        
        // //取得した駒にRight側で動けるマスがあるか判定
        // MovableSquare = GetEnemyMovableSquare(EnemyNum,EnemyPointX,EnemyPointY);

        // //存在しない場合違う駒を取得しなおす
        // bool enemyMovable = false;
        // for (int i=0; i<9; i++){
        //     if (MovableSquare[i,0] != 10){
        //         enemyMovable = true;
        //     }
        // }
        // if (!enemyMovable){
        //     if (EnemyNum==11){
        //         MovableSquare = GetEnemyMovableSquare(12,EnemyPointX,EnemyPointY);
        //         enemyMovable = false;
        //         for (int i=0; i<9; i++){
        //             if (MovableSquare[i,0] != 10){
        //                 enemyMovable = true;
        //                 break;
        //             }
        //         }
        //     } else if (EnemyNum==12){
        //         MovableSquare = GetEnemyMovableSquare(11,EnemyPointX,EnemyPointY);
        //         enemyMovable = false;
        //         for (int i=0; i<9; i++){
        //             if (MovableSquare[i,0] != 10){
        //                 enemyMovable = true;
        //                 break;
        //             }
        //         }
        //     }
        // }
        
        //動けるマスの配列の中からランダムにひとつ取得
        // System.Random r1 = new System.Random();
        // int r2 = r1.Next(0, 4);
    }

    // コルーチン本体
    private IEnumerator ProcessAfterAnimation(int EnemyPointX,int EnemyPointY, int EnemyNum, GameObject? allyDrag)
    {
        // 1秒間待つ
        yield return new WaitForSeconds(1);

        Debug.Log(EnemyPointX);
        Debug.Log(EnemyPointY);

        //敵オブジェクトの座標の指定し直し
        allyDrag.transform.localPosition = new Vector3(PointValueXArray[EnemyPointX+1], PointValueYArray[EnemyPointY], 0);
        
        //配列arrayBoardの書き換え
        BoardSurface.GetComponent<BoardSurface>().arrayBoard[EnemyPointY,EnemyPointX] = 0;    //元いたマスのステータスを0にする
        BoardSurface.GetComponent<BoardSurface>().arrayBoard[EnemyPointY,EnemyPointX+1] = EnemyNum;
        // Debug.Log(arrayBoard);

        //フォーカスアニメーションの解除
        BoardSurface.GetComponent<BoardSurface>().FocusParent.SetActive(false);

        //プレイヤーターンに移行
        BoardSurface.GetComponent<BoardSurface>().allyDrag = null;
    }
    

    // //関数：ActionEnemyTurn()内部で取得した駒にRight側で動けるマスがあるか判定
    // int[,] GetEnemyMovableSquare(int? EnemyNum, int EnemyPointX, int EnemyPointY){
    //     int[] moveEnemy = new int[8];
    //     int[,] MovableSquare = {
    //         {10,10},{10,10},{10,10},
    //         {10,10},{10,10},{10,10},
    //         {10,10},{10,10},{10,10}
    //     };

    //     if (EnemyNum == 11){
    //         moveEnemy = moveEnemy1;
    //     } else if (EnemyNum == 12){
    //         moveEnemy = moveEnemy2;
    //     }
    //     //RightUp
    //     if (moveEnemy[5] != 0){
    //         if (EnemyPointY >= 1 && EnemyPointX <= 2 && arrayBoard[EnemyPointY-1,EnemyPointX+1] == 0){
    //             MovableSquare[0,0] = EnemyPointX+1;
    //             MovableSquare[0,1] = EnemyPointY-1;
    //         }
    //         if (moveAlly[5] == 2){
    //             if (EnemyPointY >= 2 && EnemyPointX <= 1 && arrayBoard[EnemyPointY-2,EnemyPointX+2] == 0){
    //                 MovableSquare[1,0] = EnemyPointX+2;
    //                 MovableSquare[1,1] = EnemyPointY-2;
    //             }
    //             if (EnemyPointY >= 3 && EnemyPointX <= 0 && arrayBoard[EnemyPointY-3,EnemyPointX+3] == 0){
    //                 MovableSquare[2,0] = EnemyPointX+3;
    //                 MovableSquare[2,1] = EnemyPointY-3;
    //             }
    //         }
    //     }
    //     //Right
    //     if (moveEnemy[6] != 0){
    //         if (EnemyPointX <= 2 && arrayBoard[EnemyPointY,EnemyPointX+1] == 0){
    //             MovableSquare[3,0] = EnemyPointX+1;
    //             MovableSquare[3,1] = EnemyPointY-1;
    //         }
    //         if (moveAlly[6] == 2){
    //             if (EnemyPointX <= 1 && arrayBoard[EnemyPointY,EnemyPointX+2] == 0){
    //                 MovableSquare[4,0] = EnemyPointX+2;
    //                 MovableSquare[4,1] = EnemyPointY-2;
    //             }
    //             if (EnemyPointX <= 0 && arrayBoard[EnemyPointY,EnemyPointX+3] == 0){
    //                 MovableSquare[5,0] = EnemyPointX+3;
    //                 MovableSquare[5,1] = EnemyPointY-3;
    //             }
    //         }
    //     }
    //     //RightDown
    //     if (moveEnemy[7] != 0){
    //         if (EnemyPointY <= 2 && EnemyPointX <= 2 && arrayBoard[EnemyPointY+1,EnemyPointX+1] == 0){
    //             MovableSquare[6,0] = EnemyPointX+1;
    //             MovableSquare[6,1] = EnemyPointY-1;
    //         }
    //         if (moveAlly[7] == 2){
    //             if (EnemyPointY <= 1 && EnemyPointX <= 1 && arrayBoard[EnemyPointY+2,EnemyPointX+2] == 0){
    //                 MovableSquare[7,0] = EnemyPointX+2;
    //                 MovableSquare[7,1] = EnemyPointY+2;
    //             }
    //             if (EnemyPointY <= 0 && EnemyPointX <= 0 && arrayBoard[EnemyPointY+3,EnemyPointX+3] == 0){
    //                 MovableSquare[8,0] = EnemyPointX+3;
    //                 MovableSquare[8,1] = EnemyPointY+3;
    //             }
    //         }
    //     }
    //     return MovableSquare;
    // }
}
