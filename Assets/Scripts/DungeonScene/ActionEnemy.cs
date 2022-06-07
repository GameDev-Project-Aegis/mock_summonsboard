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

        GameObject? allyDrag = null;

        int[] DistinationSquare = new int[2];
        int[,] AvailableSquares;
        
        //Enemyの中で一番X座標が小さい駒を取得
        for(int i=0; i<4; i++){
            for(int j=0; j<4; j++){
                if (arrayBoard[j,i]==11 || arrayBoard[j,i]==12){
                    EnemyNum = arrayBoard[j,i];
                    EnemyPointX = i;
                    EnemyPointY = j;
                    Debug.Log("EnemyPointX: "+EnemyPointX);
                    Debug.Log("EnemyPointY: "+EnemyPointY);

                    //取得した駒のフォーカスエフェクトを呼び出す
                    if (EnemyNum==11){
                        allyDrag = enemy1;
                    } else if (EnemyNum==12){
                        allyDrag = enemy2;
                    }
                    AvailableSquares = BoardSurfaceClass.GenerateAvailableSquares(allyDrag,EnemyPointX,EnemyPointY,false);

                    //駒の移動先を決定する関数
                    DistinationSquare = CalculateDistinationSquare(AvailableSquares);

                    if(DistinationSquare[0] != 10){
                        //取得した駒のアニメーションを実行する
                        EnemyMoveAnimation(DistinationSquare,EnemyPointX,EnemyPointY,allyDrag);
                        //アニメーション終了後の処理
                        StartCoroutine(ProcessAfterAnimation(EnemyPointX,EnemyPointY,DistinationSquare[0],DistinationSquare[1],EnemyNum,allyDrag));
                    }else{
                        //もし移動可能なマスがなかった場合、同じ処理を違うコマに変更して再度行う
                        if(EnemyNum==11){
                            JudgeOtherMonster(12,allyDrag,arrayBoard);
                        }else if(EnemyNum==12){
                            JudgeOtherMonster(11,allyDrag,arrayBoard);
                        }
                    }
                    return;
                }
            }
        }
    }

    //関数：敵駒の移動先を決定するアルゴリズム
    int[] CalculateDistinationSquare(int[,] AvailableSquares)
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
            {0,0},{3,0},{0,3},{3,3}     //角４マス
        };
        for (int i=0; i<16; i++){
            SquarePoint = VerifyDistinationSquare(PrioritySquare[i,0],PrioritySquare[i,1],AvailableSquares);
            if(SquarePoint[0]!=10 && SquarePoint[1]!=10){
                break;
            }
        }
        return SquarePoint; //breakが呼ばれなかった場合は{10,10}が返される
    }

    //関数：vailableSquaresに期待するマスがあるか判定する処理
    int[] VerifyDistinationSquare(int PointX, int PointY, int[,] AvailableSquares)
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

    //関数：取得した駒のアニメーションを実行
    void EnemyMoveAnimation(int[] DistinationSquare, int EnemyPointX, int EnemyPointY, GameObject? allyDrag)
    {
        int distanceX;
        int distanceY;

        distanceX = DistinationSquare[0]-EnemyPointX;
        distanceY = DistinationSquare[1]-EnemyPointY;
                    
        if (distanceX == -1 && distanceY == -1){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("leftup1");
        }else if (distanceX == -2 && distanceY == -2){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("leftup2");
        }else if (distanceX == -3 && distanceY == -3){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("leftup3");
        }else if (distanceX == -1 && distanceY == 0){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("left1");
        }else if (distanceX == -2 && distanceY == 0){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("left2");
        }else if (distanceX == -3 && distanceY == 0){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("left3");
        }else if (distanceX == -1 && distanceY == 1){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("leftdown1");
        }else if (distanceX == -2 && distanceY == 2){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("leftdown2");
        }else if (distanceX == -3 && distanceY == 3){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("leftdown3");
        }else if (distanceX == 0 && distanceY == -1){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("up1");
        }else if (distanceX == 0 && distanceY == -2){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("up2");
        }else if (distanceX == 0 && distanceY == -3){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("up3");
        }else if (distanceX == 0 && distanceY == 1){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("down1");
        }else if (distanceX == 0 && distanceY == 2){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("down2");
        }else if (distanceX == 0 && distanceY == 3){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("down3");
        }else if (distanceX == 1 && distanceY == -1){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("rightup1");
        }else if (distanceX == 2 && distanceY == -2){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("rightup2");
        }else if (distanceX == 3 && distanceY == -3){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("rightup3");
        }else if (distanceX == 1 && distanceY == 0){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("right1");
        }else if (distanceX == 2 && distanceY == 0){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("right2");
        }else if (distanceX == 3 && distanceY == 0){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("right3");
        }else if (distanceX == 1 && distanceY == 1){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("rightdown1");
        }else if (distanceX == 2 && distanceY == 2){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("rightdown2");
        }else if (distanceX == 3 && distanceY == 3){
            allyDrag.transform.GetChild(0).GetComponent<Animator>().SetTrigger("rightdown3");
        }
    }

    //関数：もし移動可能なマスがなかった場合、同じ処理を違うコマに変更して再度行う
    void JudgeOtherMonster(int OtherEnemyNum, GameObject? allyDrag, int[,] arrayBoard)
    {
        int EnemyNum;
        int EnemyPointX;
        int EnemyPointY;

        int[] DistinationSquare;
        int[,] AvailableSquares;

        for(int i=0; i<4; i++){
            for(int j=0; j<4; j++){
                if (arrayBoard[j,i] == OtherEnemyNum){
                    EnemyNum = OtherEnemyNum;
                    EnemyPointX = i;
                    EnemyPointY = j;
                    //取得した駒のフォーカスエフェクトを呼び出す
                    if (EnemyNum==11){
                        allyDrag = enemy1;
                    } else if (EnemyNum==12){
                        allyDrag = enemy2;
                    }
                    AvailableSquares = BoardSurfaceClass.GenerateAvailableSquares(allyDrag,EnemyPointX,EnemyPointY,false);
                    DistinationSquare = CalculateDistinationSquare(AvailableSquares);

                    if(DistinationSquare[0]!=10){
                        EnemyMoveAnimation(DistinationSquare,EnemyPointX,EnemyPointY,allyDrag);
                        StartCoroutine(ProcessAfterAnimation(EnemyPointX,EnemyPointY,DistinationSquare[0],DistinationSquare[1],EnemyNum,allyDrag));
                    }else{
                        //この駒も移動だ出来ない場合敵ターンを終了する
                        BoardSurfaceClass.InitializationEffect();
                        BoardSurfaceClass.TurnPlayerTurn();
                    }
                    return;
                }
            }
        }
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
        BoardSurfaceClass.UpdateArrayBoard(DistinationX,DistinationY,EnemyNum); //新たに移動したマスに値を書き換える
        
        //フォーカスアニメーションの解除
        BoardSurfaceClass.InitializationEffect();

        //プレイヤーターンをに切り替える
        BoardSurfaceClass.TurnPlayerTurn();
    }
}
