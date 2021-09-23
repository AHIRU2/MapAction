using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Fatigue : PlayerConditionBase
{
    private int originValue; //現在の攻撃力を保持しておくための変数


    protected override void OnEnterCondition()
    {
        //元に戻すために保持
        originValue = GameData.instance.attackPower;

        //バトル時の攻撃力を半減
        GameData.instance.attackPower = Mathf.FloorToInt(GameData.instance.attackPower * conditionValue);

        //親クラスのOnEnterConditionメソッドを実行
        base.OnEnterCondition();
    }


    /// <summary>
    /// 攻撃力を戻す
    /// </summary>
    protected override void OnExitCondition()
    {
        //攻撃力を元の値に戻す
        GameData.instance.attackPower = originValue;

        //親クラスのOnExitConditionメソッドを実行
        base.OnExitCondition();
    }
}
