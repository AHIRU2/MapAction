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

[System.Serializable]
public struct SymbolGenerateData
{
    //生成するシンボルのプレファブを登録
    public SymbolBase symbolBasePrefab;
    public int symbolWeight;
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

    //シンボル生成用のデータリスト
    [SerializeField]
    private List<SymbolGenerateData> symbolGenerateDataList = new List<SymbolGenerateData>();
    [SerializeField]
    private List<SymbolGenerateData> spacialSymbolGenerateDataList = new List<SymbolGenerateData>();
    [SerializeField, Header("シンボルの生成率"), Range(0, 100)]
    private int generateSymbolRate;

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
                        if(i==-row || i==row-1 || j==-column || j == column-1)
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
    /// <summary>
    /// 通常のシンボルをランダムに作成
    /// </summary>
    /// <param name="generateSymbolCount"></param>
    /// <returns></returns>
    public List<SymbolBase> GenerateSymbols(int generateSymbolCount)
    {
        //Listに登録する
        List<SymbolBase> symbolsList = new List<SymbolBase>();

        //重み付けの合計値を算出
        int totalWaight = symbolGenerateDataList.Select(x => x.symbolWeight).Sum();
        Debug.Log(totalWaight);

        for(int i = -row + 1; i < row - 1; i++)
        {
            for(int j = -column + 1; j < column - 1; j++)
            {
                //プレイヤーのスタート地点の場合
                if (i == 0 && j == 0)
                {
                    //何も行わずに次の処理へ
                    continue;
                }

                //タイルマップの座標に変換
                Vector3Int tilePos = tileMapCollison.WorldToCell(new Vector3(i, j, 0));

                //タイルのColliderTypeがGridではないか確認
                if (tileMapCollison.GetColliderType(tilePos) == Tile.ColliderType.Grid)
                {
                    //Gridの場合には配置しないので、何も行わずに次の処理へ
                    continue;
                }

                //80%は新ボウルなし=>264マスの場合、大体35〜55個のシンボルが出来る
                if (UnityEngine.Random.Range(0, 100) > generateSymbolRate)
                {
                    continue;
                }

                int index = 0;
                int value = UnityEngine.Random.Range(0, totalWaight);

                //重み付けから生成するシンボルを確認
                for(int x = 0; x < symbolGenerateDataList.Count; x++)
                {
                    if (value <= symbolGenerateDataList[x].symbolWeight)
                    {
                        index = x;
                        Debug.Log(index + "value:" + value);
                        break;
                    }
                    value -= symbolGenerateDataList[x].symbolWeight;
                }

                //抽選されたシンボルを生成
                symbolsList.Add(Instantiate(symbolGenerateDataList[index].symbolBasePrefab, new Vector3(i, j, 0), Quaternion.identity));

                generateSymbolCount--;

                //generateSymbolCount=-1でスタートの場合は抽選回数なし
                if (generateSymbolCount == 0)
                {
                    break;
                }
            }

            if (generateSymbolCount == 0)
            {
                break;
            }
        }

        //完成したリストを戻す
        return symbolsList;
    }

    // TODO その他にステージ制作に関わる処理を書く
}
