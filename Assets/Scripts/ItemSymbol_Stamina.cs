using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSymbol_Stamina : SymbolBase
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

        GameData.instance.staminaPoint += recoveryPoint;

        mapMoveController.uiManager.DisplayStaminaPoint();

        base.OnExitSymbol();
    }
}
