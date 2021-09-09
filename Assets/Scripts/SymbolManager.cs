using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


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

        // TODO 各オーブをエネミーの上に配置


    }
}
