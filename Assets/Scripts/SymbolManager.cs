using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.Tilemaps;


/// <summary>
/// シンボルの生成・管理・制御を行うクラス
/// </summary>
public class SymbolManager : MonoBehaviour
{
    [SerializeField]
    private List<SymbolBase> symbolsList = new List<SymbolBase>();

    //プロパティ
    public List<SymbolBase> SymbolsList
    {
        set => symbolsList = value;
        get => symbolsList;
    }

    [SerializeField]
    private List<EnemySymbol> enemiesList = new List<EnemySymbol>();

    [SerializeField]
    private Transform spriteMaskTran;

    public Tilemap tilemapCollider;


    /// <summary>
    /// 全てのシンボルの初期設定
    /// </summary>
    public void SetUpAllAymbols()
    {
        List<SymbolBase> spacialSymbols = new List<SymbolBase>();

        for(int i = 0; i < symbolsList.Count; i++)
        {
            symbolsList[i].transform.SetParent(this.transform);
            symbolsList[i].OnEnterSymbol(this);

            // TODO 特殊なシンボルの処理を書く
        }

        // TODO Enemyの種類だけを抽出してエネミー用のListに代入
        enemiesList = GetListSymbolTypeFromSymbolsList(SymbolType.Enemy);

        // TODO 各オーブをエネミーの上に配置

    }


    /// <summary>
    /// SymbolのListを取得
    /// </summary>
    /// <returns></returns>
    public List<SymbolBase> GetSymbolsList()
    {
        return symbolsList;
    }



    /// <summary>
    /// 全てのシンボルの画像を表示/非表示
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchDisplayAllSymbols(bool isSwitch)
    {
        for(int i = 0; i < symbolsList.Count; i++)
        {
            symbolsList[i].SwitchDisplaySymbol(isSwitch);
        }
    }


    /// <summary>
    /// 指定された以外のシンボルのゲームオブジェクトの表示/非表示
    /// </summary>
    /// <param name="isSwitch"></param>
    /// <param name="exceptSymbolTypeNo"></param>
    public void SwitchActivateExceptSymbols(bool isSwitch,int exceptSymbolTypeNo)
    {
        //for(int i = 0; i < symbolsList.Count; i++)
        //{
        //    if(symbolsList[i].symbolType != exceptSymbolType)
        //    {
        //        symbolsList[i].SwitchActivateSymbol(isSwitch);
        //    }
        //}

        //上の for 文を Linq の Where メソッドを利用した場合の処理
        foreach(SymbolBase symbol in symbolsList.Where(x => x.symbolType != (SymbolType)exceptSymbolTypeNo))
        {
            symbol.SwitchActivateSymbol(isSwitch);
        }
    }


    /// <summary>
    /// Listからシンボルを削除
    /// </summary>
    /// <param name="symbol"></param>
    public void RemoveSymbolsList(SymbolBase symbol)
    {
        symbolsList.Remove(symbol);
    }


    /// <summary>
    /// Listから全てのシンボルを削除
    /// </summary>
    public void AllClearSymbolsList()
    {
        symbolsList.Clear();
    }


    /// <summary>
    /// SymbolListから引数で指定した種類のみを抽出する
    /// </summary>
    /// <param name="getSymbolType"></param>
    /// <returns></returns>
    private List<EnemySymbol> GetListSymbolTypeFromSymbolsList(SymbolType getSymbolType)
    {
        return symbolsList.Where(x => x.symbolType == getSymbolType).Select(x => x.GetComponent<EnemySymbol>()).ToList();
    }


    /// <summary>
    /// 全エネミーシンボルの移動処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemisMove()
    {
        for(int i = 0; i < enemiesList.Count; i++)
        {
            //プレイヤーと接触しているエネミーは移動させない
            if (enemiesList[i].isSymbolTriggerd)
            {
                continue;
            }


            //エネミーの移動
            enemiesList[i].EnemyMove();

            //1体ずつ時間差で動くように、少しだけ処理を中断して待機
            yield return new WaitForSeconds(0.05f);

            Debug.Log("敵の移動：" + i + "体目");
        }

    }



    /// <summary>
    /// エネミーのリストからの削除
    /// </summary>
    /// <param name="enemySymbol"></param>
    public void RemoveEnemySymbol(EnemySymbol enemySymbol)
    {
        //enemySymbol.Remove(enemySymbol);
    }


    /// <summary>
    /// 全てのエネミー子ライダーを制御
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchEnemyCollider(bool isSwitch)
    {
        for(int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].SwitchCollider(isSwitch);
        }
    }


    /// <summary>
    /// SpriteMaskゲームオブジェクトのTransformを取得
    /// </summary>
    /// <returns></returns>
    public Transform GetSpriteMaskTransform()
    {
        return spriteMaskTran;
    }

}
