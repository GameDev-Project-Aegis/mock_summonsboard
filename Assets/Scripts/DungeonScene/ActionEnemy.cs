using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEnemy : MonoBehaviour
{
    public GameObject BoardSurface;
    BoardSurface BoardSurfaceClass;
    
    public GameObject enemy1;
    public GameObject enemy2;

    //マス目に対応するlocal座標を配列としたもの
    float[] PointValueXArray = {-105, -35, 35, 105};
    float[] PointValueYArray = {105, 35, -35, -105};

    void Start()
    {
        BoardSurfaceClass = BoardSurface.GetComponent<BoardSurface>();
    }

    //関数：相手ターンの一連の処理を行う関数
    public void ActionEnemyTurn(int[,] arrayBoard)
    {
        int EnemyNum = 0;

        int EnemyPointX = 10;
        int EnemyPointY = 10;

        int[,] MovableSquare = new int[9,2];
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
                    BoardSurfaceClass.GenerateAvailableSquares(allyDrag,EnemyPointX,EnemyPointY,false);

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
    }

    // コルーチン本体
    private IEnumerator ProcessAfterAnimation(int EnemyPointX,int EnemyPointY, int EnemyNum, GameObject? allyDrag)
    {
        // 1秒間待つ
        yield return new WaitForSeconds(1);

        //敵オブジェクトの座標の指定し直し
        allyDrag.transform.localPosition = new Vector3(PointValueXArray[EnemyPointX+1], PointValueYArray[EnemyPointY], 0);
        
        //配列arrayBoardの書き換え
        BoardSurfaceClass.UpdateArrayBoard(EnemyPointX,EnemyPointY,0);    //元いたマスのステータスを0にする
        BoardSurfaceClass.UpdateArrayBoard(EnemyPointX+1,EnemyPointY,EnemyNum);
        
        //フォーカスアニメーションの解除
        BoardSurfaceClass.InitializationEffect();

        //プレイヤーターンをに切り替える
        BoardSurfaceClass.TurnPlayerTurn();
    }
}
