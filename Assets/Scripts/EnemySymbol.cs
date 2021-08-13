using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySymbol : SymbolBase
{
    public override void TrrigerSymbol()
    {
        Debug.Log("バトルシーン移動");
        base.TrrigerSymbol();
        SceneStateManager.instance.PreparateBattleScene();
    }
}
