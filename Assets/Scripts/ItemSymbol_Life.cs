using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSymbol_Life : SymbolBase
{
    public int recoveryPoint;

    public override void OnEnterSymbol(SymbolManager symbolManager)
    {
        base.OnEnterSymbol(symbolManager);

        // TODO ConditionDataSO スクリプタブル・オブジェクトよりデータを取得して設定を行う


        // TODO フィールドでのエフェクト演出

    }

    public override void TriggerSymbol(MapMoveController mapMoveController)
    {

        base.TriggerSymbol(mapMoveController);

        GameData.instance.hp = Mathf.Clamp(GameData.instance.hp += recoveryPoint, 0, GameData.instance.maxHp);
     
        mapMoveController.uiManager.DisplayHpGauge();

        base.OnExitSymbol();
    }

}