using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPlayer : MonoBehaviour
{
    public GameObject BoardSurface;
    BoardSurface BoardSurfaceClass;

    //駒オブジェクトをアタッチする
    public GameObject ally1;
    public GameObject ally2;
    public GameObject ally3;
    public GameObject ally4;
    public GameObject enemy1;
    public GameObject enemy2;
    
    //HighLight(originally)をアタッチ
    public GameObject HighLight_O;
    //HighLight(distination)をアタッチ
    public GameObject HighLight_D;

    // int[,] arrayBoard;
    private int[] PointArray = new int[2];
    private int PointX;
    private int PointY;
    // private int DragObjectNum;

    // //マス目に対応するlocal座標を配列としたもの
    float[] PointValueXArray = {-105, -35, 35, 105};
    float[] PointValueYArray = {105, 35, -35, -105};

    void Start()
    {
        BoardSurfaceClass = BoardSurface.GetComponent<BoardSurface>();
    }

    //関数：タップした先のモンスターを返す
    public GameObject? MonsterOnTaped(Vector3 mousePos, int InitialPointX, int InitialPointY, int[,] arrayBoard)
    {
        //①タップしたマスに味方駒があるか判定
        //②味方駒がいた場合にallyDragにその駒のオブジェクトをセットする
        //transform.SetAsLastSibling()でヒエラルキー内の順序を変更し，一番手前に表示
        if (arrayBoard[InitialPointY,InitialPointX] == 1) {
            BoardSurfaceClass.UpdateDragObjectNum(1);
            ally1.transform.SetAsLastSibling();
            return ally1;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 2) {
            BoardSurfaceClass.UpdateDragObjectNum(2);
            ally2.transform.SetAsLastSibling();
            return ally2;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 3) {
            BoardSurfaceClass.UpdateDragObjectNum(3);
            ally3.transform.SetAsLastSibling();
            return ally3;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 4) {
            BoardSurfaceClass.UpdateDragObjectNum(4);
            ally4.transform.SetAsLastSibling();
            return ally4;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 11) {
            BoardSurfaceClass.UpdateDragObjectNum(11);
            return enemy1;
        }else if (arrayBoard[InitialPointY,InitialPointX] == 12) {
            BoardSurfaceClass.UpdateDragObjectNum(12);
            return enemy2;
        }else{
            return null;
        }
    }

    //関数：float型の座標を引数で与えるとどのマス目かを返す関数
    //返り値の型は長さ2の配列（int）
    int[] GetBoardPoint(float mPosX, float mPosY)
    {
        
        if (mPosX > -2.2 && mPosX < -1.1) {
            PointX = 0;
        }else if (mPosX > -1.1 && mPosX < 0) {
            PointX = 1;
        }else if (mPosX > 0 && mPosX < 1.1) {
            PointX = 2;
        }else if (mPosX > 1.1 && mPosX < 2.2) {
            PointX = 3;
        }

        if (mPosY < 2.2 && mPosY > 1.1) {
            PointY = 0;
        }else if (mPosY < 1.1 && mPosY > 0) {
            PointY = 1;
        }else if (mPosY < 0 && mPosY > -1.1) {
            PointY = 2;
        }else if (mPosY < -1.1 && mPosY > -2.2) {
            PointY = 3;
        }

        int[] arr = {PointX, PointY};

        return arr;
    }

    //関数：タップ時にハイライトを表示する関数
    public void SetHighLight(int InitialPointX, int InitialPointY)
    {
        //初期配置の場所にHighLight(originally)を表示する
        HighLight_O.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
        HighLight_O.GetComponent<Animator>().SetTrigger("setOriginally");
        //HighLight(distination)を表示する
        HighLight_D.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
        HighLight_D.GetComponent<Animator>().SetTrigger("setDistination");
    }

    //関数：モンスターをドラッグに合わせて移動させる
    public void DragMonster(Vector3 mousePos, GameObject? allyDrag, float PosZ)
    {
        mousePos.z = PosZ;
        if(allyDrag != null){
            allyDrag.transform.position = mousePos;
        }

        //座標からマス目を取得
        PointArray = GetBoardPoint(mousePos.x, mousePos.y);
        //HighLghtの移動
        HighLight_D.transform.localPosition = new Vector3(PointValueXArray[PointArray[0]], PointValueYArray[PointArray[1]], 0);
    }

    //関数：モンスターをドロップしたときの処理を行う
    public bool DropMonster(int[,] AvailableSquares, int PointX, int PointY, int InitialPointX, int InitialPointY, GameObject? allyDrag, int DragObjectNum)
    {
        //動けるマスの整理
        bool immovable = false;
        for(int i = 0; i < AvailableSquares.GetLength(0); i++){
            if (AvailableSquares[i,0] == PointX && AvailableSquares[i,1] == PointY){
                //駒オブジェクトのpositionの書き換え
                //ドロップ時のマウスの位置するマスに対応したローカル座標を配列PointValueX(Y)Arrayで取得している
                allyDrag.transform.localPosition = new Vector3(PointValueXArray[PointX], PointValueYArray[PointY], 0);
                //配列arrayBoardの書き換え
                BoardSurfaceClass.UpdateArrayBoard(InitialPointX,InitialPointY,0);  //元いたマスのステータスを0にする
                BoardSurfaceClass.UpdateArrayBoard(PointX,PointY,DragObjectNum);    //移動した先のマスのステータスを駒の番号にする

                immovable = true;
                break;
            }
        }

        return immovable;
    }
            
    //関数：駒の位置とエフェクトを初期化する
    public void InitializationDrag(GameObject? allyDrag, CanvasGroup Ally, bool immovable, int InitialPointX, int InitialPointY)
    {
        if (!immovable){
            allyDrag.transform.localPosition = new Vector3(PointValueXArray[InitialPointX], PointValueYArray[InitialPointY], 0);
        }

        //alluDragがセットされた場合に味方駒のcanvasを半透明にする
        Ally.alpha = 1.0f;
            
        //HighLightの解除
        HighLight_O.GetComponent<Animator>().SetTrigger("outOriginally");
        HighLight_D.GetComponent<Animator>().SetTrigger("outDistination");

        BoardSurfaceClass.InitializationEffect();
    }
}
