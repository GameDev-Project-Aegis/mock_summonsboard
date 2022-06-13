using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttack : MonoBehaviour
{
    public GameObject BoardSurface;
    BoardSurface BoardSurfaceClass;

    public GameObject ally1;
    ally Ally1Class;
    public GameObject ally2;
    ally Ally2Class;
    public GameObject ally3;
    ally Ally3Class;
    public GameObject ally4;
    ally Ally4Class;

    void Start()
    {
        Ally1Class = ally1.GetComponent<ally>();
        Ally2Class = ally2.GetComponent<ally>();
        Ally3Class = ally3.GetComponent<ally>();
        Ally4Class = ally4.GetComponent<ally>();
    }

    public void AllyDirectAttack(int[,] arrayBoard)
    {
        //周囲8マスのモンスターを判別する関数
        int[] arrayAround8 = new int[8];

        //守備モンスターの配置を順に確認する
        for(int i=0; i<4; i++){
            for(int j=0; j<4; j++){
                if(arrayBoard[i,j]==11){
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
                    // //ally1の場合
                    // int ally1_index = Array.IndexOf(arrayAround8, 1);
                    // //ally1が存在しない場合
                    // if (ally1_index==-1){
                    //     //
                    // }
                }
            }
        }

        //接している攻撃モンスターの攻撃アニメーションを呼び出す
        Ally1Class.Attack3();
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
