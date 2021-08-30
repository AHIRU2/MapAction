using System.Collections;
using System.Collections.Generic;
using UnityEngine;    
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public enum StageType
{
    Field,
    Dungeon,
}

public class StageGenerator : MonoBehaviour
{
    //StageType.Field用のタイル群
    [SerializeField] private Tile[] fieldBaseTiles;
    [SerializeField] private Tile[] fieldWalkTiles;
    [SerializeField] private Tile[] fieldCollisionTiles;

    //タイルを配置するタイルマップ
    [SerializeField] private Tilemap tileMapBase;
    [SerializeField] private Tilemap tileMapWalk;
    [SerializeField] private Tilemap tileMapCollison;

    //並べる数
    [SerializeField] private int row; // 行
    [SerializeField] private int column; // 列                        

    // Start is called before the first frame update
    void Start()
    {
        //Debug用
        GenerateStageFromRandomTiles();
    }


    /// <summary>
    /// ランダムなタイルをタイルマップに配置してステージを作る
    /// </summary>
    /// <param name="stageType"></param>
    public void GenerateStageFromRandomTiles(StageType stageType = StageType.Field)
    {
        //Grid_Baseと外壁用のGrid_Colliderを配置
        for(int i = -row; i < row; i++)
        {
            for(int j = -column; j < column; j++)
            {
                switch (stageType)
                {
                    case StageType.Field:
                        //一番外側の場合
                        if(i==-row || i==row-1 || j==-column || j == column)
                        {
                            //壁用の子ライダータイルを配置
                            tileMapCollison.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[0]);
                        }
                        else
                        {
                            //フィールド用のタイルを配置
                            tileMapBase.SetTile(new Vector3Int(i, j, 0), fieldBaseTiles[0]);
                        }

                        break;

                    case StageType.Dungeon:
                    default:
                        break;
                }
            }
        }

        //Grid_WalkとGrid_Colliderを配置
        int generateValue = 0;

        for(int i = -row; i < row; i++)
        {
            for(int j = -column; j < column; j++)
            {
                //一番外側の場合とプレイヤーのスタート地点の場合
                if(i == -row || i == row -1 || j == -column || j ==column - 1 || (i == 0 && j == 0))
                {
                    //何も行わずに次の処理へ
                    continue;
                }

                //生成値用のランダム値を取得
                int maxRandomRange = UnityEngine.Random.Range(30,80);

                //生成値を加算
                generateValue += UnityEngine.Random.Range(0, maxRandomRange);

                //生成値が生成目標値（仮）を超えていない場合
                if (generateValue <= 100)
                {
                    //何も行わずに次の処理へ
                    continue;
                }

                //WalkかCollisionか決める（仮に、20％の確率でCollision）
                if (UnityEngine.Random.Range(0, 100) <= 20)
                {
                    //Collision用のタイルの中でランダムにタイルを決める
                    tileMapCollison.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[UnityEngine.Random.Range(0, fieldCollisionTiles.Length)]);
                }
                else
                {
                    //Walk用タイルの中でランダムにタイルを決める
                    tileMapWalk.SetTile(new Vector3Int(i, j, 0), fieldWalkTiles[UnityEngine.Random.Range(0, fieldWalkTiles.Length)]);
                }

                //タイルを生成したので生成値をリセット
                generateValue = 0;
                
            }
        }
    }

    // TODO シンボルのランダム生成のメソッド

    // TODO その他にステージ制作に関わる処理を書く
}
