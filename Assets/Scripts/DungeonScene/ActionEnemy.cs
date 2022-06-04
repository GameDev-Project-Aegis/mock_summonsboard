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

    int[,] AvailableSquares;

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

        // int[,] MovableSquare = new int[9,2];
        GameObject? allyDrag = null;

        int[] DistinationSquare = new int[2];
        
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

                    //ここ以下に敵の動きギミックを作成する
                    
                    //移動先を決定する関数
                    DistinationSquare = CalculateDistinationSquare();

                    //もし移動可能なマスがなかった場合、同じ処理を違うコマに変更して再度行う
                    if (DistinationSquare[0]==10){
                        DistinationSquare = new int[2];
                        if (EnemyNum==11){
                            allyDrag = enemy2;
                            BoardSurfaceClass.GenerateAvailableSquares(allyDrag,EnemyPointX,EnemyPointY,false);
                            DistinationSquare = CalculateDistinationSquare();
                            //最終移動可能マスがなかった場合、移動を行わずターンを終了する
                            if (DistinationSquare[0]==10){
                                StartCoroutine(ProcessAfterAnimation(EnemyPointX,EnemyPointY,EnemyPointX,EnemyPointY,EnemyNum,allyDrag));
                                break;
                            }
                        } else if (EnemyNum==12){
                            allyDrag = enemy1;
                            BoardSurfaceClass.GenerateAvailableSquares(allyDrag,EnemyPointX,EnemyPointY,false);
                            DistinationSquare = CalculateDistinationSquare();
                            //最終移動可能マスがなかった場合、移動を行わずターンを終了する
                            if (DistinationSquare[0]==10){
                                StartCoroutine(ProcessAfterAnimation(EnemyPointX,EnemyPointY,EnemyPointX,EnemyPointY,EnemyNum,allyDrag));
                                break;
                            }
                        }
                    }

                    AvailableSquares = BoardSurfaceClass.AvailableSquares;

                    //取得した駒のアニメーションを実行する
                    // allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("right1");   //ひとまず右に1マス
                    int distanceX;
                    int distanceY;
                    string trigger = "";

                    distanceX = DistinationSquare[0]-EnemyPointX;
                    distanceY = DistinationSquare[1]-EnemyPointY;
                    
                    if (distanceX == -1 && distanceY == -1){
                        trigger = "leftup1";
                    }else if (distanceX == -2 && distanceY == -2){
                        trigger = "leftup2";
                    }else if (distanceX == -3 && distanceY == -3){
                        trigger = "leftup3";
                    }else if (distanceX == -1 && distanceY == 0){
                        trigger = "left1";
                    }else if (distanceX == -2 && distanceY == 0){
                        trigger = "left2";
                    }else if (distanceX == -3 && distanceY == 0){
                        trigger = "left3";
                    }else if (distanceX == -1 && distanceY == 1){
                        trigger = "leftdown1";
                    }else if (distanceX == -2 && distanceY == 2){
                        trigger = "leftdown2";
                    }else if (distanceX == -3 && distanceY == 3){
                        trigger = "leftdown3";
                    }else if (distanceX == 0 && distanceY == -1){
                        trigger = "up1";
                    }else if (distanceX == 0 && distanceY == -2){
                        trigger = "up2";
                    }else if (distanceX == 0 && distanceY == -3){
                        trigger = "up3";
                    }else if (distanceX == 0 && distanceY == 1){
                        trigger = "down1";
                    }else if (distanceX == 0 && distanceY == 2){
                        trigger = "down2";
                    }else if (distanceX == 0 && distanceY == 3){
                        trigger = "down3";
                    }else if (distanceX == 1 && distanceY == -1){
                        trigger = "rightup1";
                    }else if (distanceX == 2 && distanceY == -2){
                        trigger = "rightup2";
                    }else if (distanceX == 3 && distanceY == -3){
                        trigger = "rightup3";
                    }else if (distanceX == 1 && distanceY == 0){
                        trigger = "right1";
                    }else if (distanceX == 2 && distanceY == 0){
                        trigger = "right2";
                    }else if (distanceX == 3 && distanceY == 0){
                        trigger = "right3";
                    }else if (distanceX == 1 && distanceY == 1){
                        trigger = "rightdown1";
                    }else if (distanceX == 2 && distanceY == 2){
                        trigger = "rightdown2";
                    }else if (distanceX == 3 && distanceY == 3){
                        trigger = "rightdown3";
                    }
                    allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger(trigger);

                    //アニメーション終了後の処理
                    StartCoroutine(ProcessAfterAnimation(EnemyPointX,EnemyPointY,DistinationSquare[0],DistinationSquare[1],EnemyNum,allyDrag));

                    break;
                }
            }
            if(EnemyNum != 0){
                break;
            }
        }
    }

    int[] CalculateDistinationSquare()
    {
        int[] SquarePoint = new int[2];

        //より中央の方向から順に移動先を選択する
        //中央のマスっていうのは要するに{1,1}{1,2}{2,1}{2,2}
        //角移動はなるべく避けるようにする
        //つまり移動先マスの選択順としては{1,1}{1,2}{2,1}{2,2}{0,1}{0,2}{3,1}{3,2}{1,0}{2,0}{1,3}{2,3}{0,0}{1,1}{2,2}{3,3}

        int[,] PrioritySquare = new int[16,2]{
            {1,1},{1,2},{2,1},{2,2},    //中央４マス
            {0,1},{0,2},{3,1},{3,2},    //左右端４マス
            {1,0},{2,0},{1,3},{2,3},    //上下端４マス
            {0,0},{1,1},{2,2},{3,3}     //角４マス
        };
        for (int i=0; i<16; i++){
            SquarePoint = VerifyDistinationSquare(PrioritySquare[i,0],PrioritySquare[i,1]);
            if(SquarePoint[0]!=10 && SquarePoint[1]!=10){
                break;
            }
        }
        return SquarePoint; //breakが呼ばれなかった場合は{10,10}が返される
    }

    //AvailableSquaresに期待するマスがあるか判定する処理
    int[] VerifyDistinationSquare(int PointX, int PointY)
    {
        int[] SquarePoint = new int[2];
        SquarePoint[0] = 10;
        SquarePoint[1] = 10;

        for(int i = 0; i < 24; i++){
            if (AvailableSquares[i,0]==PointX && AvailableSquares[i,1]==PointY){
                SquarePoint[0] = PointX;
                SquarePoint[1] = PointY;
                break;
            }
        }
        return SquarePoint;
    }

    // コルーチン本体
    private IEnumerator ProcessAfterAnimation(int EnemyPointX,int EnemyPointY, int DistinationX, int DistinationY, int EnemyNum, GameObject? allyDrag)
    {
        // 1秒間待つ
        yield return new WaitForSeconds(1);

        //敵オブジェクトの座標の指定し直し
        allyDrag.transform.localPosition = new Vector3(PointValueXArray[DistinationX], PointValueYArray[DistinationY], 0);
        
        //配列arrayBoardの書き換え
        BoardSurfaceClass.UpdateArrayBoard(EnemyPointX,EnemyPointY,0);    //元いたマスのステータスを0にする
        BoardSurfaceClass.UpdateArrayBoard(DistinationX,DistinationY,EnemyNum); //ひとまず右に1マス
        
        //フォーカスアニメーションの解除
        BoardSurfaceClass.InitializationEffect();

        //プレイヤーターンをに切り替える
        BoardSurfaceClass.TurnPlayerTurn();
    }
}
