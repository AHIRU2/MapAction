using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public int staminaPoint;

    public int hp;

    public int maxHp;

    //現在のレベル
    public int playerLevel;

    //経験値のトータル
    public int totalExp;

    //アビリティポイント
    public int abilityPint;

    //バトルで付与されたデバフのリスト
    public List<ConditionType> debuffConditionList = new List<ConditionType>();

    //選択しているステージの番号
    public int chooseStageNo;

    //クリア済みのステージの番号
    public List<int> clearedStageNos;

    // TODO 選択しているステージのデータ
    //public StageData currentStageData;

    //ボスバトルになったかどうかの確認用
    public bool isBossBattled;

    //public int attackPower;

    public int MaxMoveCount;

    public enum GameState
    {
        Map,
        Battle,
        GameOver,
        GameClear
    }

    public GameState currentGameState;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //ゲームの初期化
    InitialzeGameData();

    //ゲームの初期化用ローカル関数
    void InitialzeGameData()
    {
        // TODO キャラ用のデータがある場合には、そのキャラごとの最大&#13259;を設定
        //maxHp=currentCharaData.maxHp;

        hp = maxHp;

        playerLevel = 1;

        totalExp = 0;

        // TODO レベルアップで獲得できるボーナスのポイント
        //abilityPoint+=playerLevel;
    }

}
