using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private StageGenerator stageGenerator;

    [SerializeField]
    private SymbolManager symbolManager;

    [SerializeField]
    private MapMoveController mapMoveController;

    public enum TurnState
    {
        None,
        Player,
        Enemy,
        Boss
    }

    private TurnState currentTurnState = TurnState.None;


    public TurnState CurrentTurnState
    {
        set => currentTurnState = value;
        get => currentTurnState;
    }


    //StageゲームオブジェクトがActive状態になるたびに実行されるメソッド
    //ゲーム開始時にもStartメソッドよりも前に実行される（Awakeメソッドの後）
    private void OnEnable()
    {
        //HPの更新
        uiManager.DisplayHpGauge();

        // TODO バトル後にレベルアップした時のカウントの初期化

        // TODO レベルアップするか確認

        // TODO レベルアップしていたらレベルアップのボーナス

        // TODO　バトルで付与されたデバフの確認と付与
        CheckDebuffConditions();

        //ターンの確認とプレイヤーのターンに切り替え。コンディションの更新
        CheckTurn();


        // TODO 特殊シンボルを獲得している場合は悪徳処理を実行

        // ボスの出現確認
        if (CurrentTurnState == TurnState.Boss)
        {
            //ボスの出現
            Debug.Log("Boss 出現");

            // TODO　演出

            // TODO　シーン遷移
        }
    }


    private void Start()
    {
        // TODO ステージ選択機能を実装した時の処理

        //ステージのランダム生成
        stageGenerator.GenerateStageFromRandomTiles();

        symbolManager.AllClearSymbolsList();

        //通常のシンボルのランダム作成してListに追加
        symbolManager.SymbolsList = stageGenerator.GenerateSymbols(-1);

        // TODO 特殊シンボルのランダム作成してListに追加

        //全シンボルの設定
        symbolManager.SetUpAllAymbols();

        //プレイヤーの設定
        mapMoveController.SetUpMapMoveController(this);

        //現在のターンをプレイヤーのターン設定
        CurrentTurnState = TurnState.Player;

        // TODO エネミーのシンボルに侵入できるようにする(特殊シンボルを使う場合)

    }


    /// <summary>
    /// エネミーのターン経過監視処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator ObserveEnemyTurnState()
    {
        Debug.Log("敵の移動開始");

        //エネミーの移動を一体ずつ行う。全て移動が終わるまで、下の処理にはいかない
        yield return StartCoroutine(symbolManager.EnemisMove());

        Debug.Log("全ての敵の移動　完了");

        //シンボルのイベントを発生させる
        bool isEnemyTriggerEvent = mapMoveController.CallBackEnemySymbolTriggerEvent();

        Debug.Log(isEnemyTriggerEvent);

        //ターンの状態を確認
        if (!isEnemyTriggerEvent) CheckTurn();

        if (CurrentTurnState == TurnState.Boss)
        {
            //ボスの出現
            Debug.Log("Boss 出現");

            // TODO 演出

            // TODO シーン遷移
        }
    }


    /// <summary>
    /// ターンの確認。プレイヤーのターンに切り替え。コンディションの更新
    /// </summary>
    private void CheckTurn()
    {
        //移動できるか確認
        if (GameData.instance.staminaPoint <= 0)
        {
            //移動できないならボスのターンにする
            CurrentTurnState = TurnState.Boss;
        }
        else
        {
            //まだ移動できるなら、プレイヤーのターンにする
            CurrentTurnState = TurnState.Player;

            // TODO コンディションの残り時間の更新
            mapMoveController.UpdateConditionsDuration();

            // TODO 移動ボタンと足踏みボタンを押せる状態にする

            // TODO コンディションの効果を適用
            ApplyEffectConditions();

            //移動の入力を受け付けるようにする
            mapMoveController.IsMoving = false;
        }

        Debug.Log(CurrentTurnState);
    }


    /// <summary>
    /// SymbolManagerの情報を取得
    /// </summary>
    /// <returns></returns>
    public SymbolManager GetSymbolManager()
    {
        return symbolManager;
    }


    /// <summary>
    /// バトルで付与されたデバフの確認
    /// </summary>
    private void CheckDebuffConditions()
    {
        //バトル内で付与されているデバフがないか判定
        if (GameData.instance.debuffConditionList.Count == 0)
        {
            //デバフが登録されていなければ、処理を終了する
            return;
        }

        //登録されているデバフを順番に
        for(int i = 0; i < GameData.instance.debuffConditionList.Count; i++)
        {
            //デバフの付与
            AddDebuff(GameData.instance.debuffConditionList[i]);
        }

        //デバフのリストをクリア。次のバトルに備える
        GameData.instance.debuffConditionList.Clear();
    }


    /// <summary>
    /// デバフの付与
    /// </summary>
    /// <param name="conditionType"></param>
    private void AddDebuff(ConditionType conditionType)
    {
        // TODO　ConditionDataSOスクリプタぶる・オブジェクトを作成してから適用
        //ConditionData conditionData = DataBaseManager.instance.conditionDataSO.conditionDatasList.Find(x => x.conditionType == conditionType);

        //すでに同じコンディションが付与されているか確認
        if (mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == conditionType))
        {
            // すでに付与されている場合は、持続時間を更新し、効果は上書きして処理を終了する
            //mapMoveController.GetConditionsList().Find(x => x.GetConditionType() == conditionType).ExtentionCondition(conditionData.duration, conditionData.conditionValue);
            return;
        }

        // 付与するコンディションが睡眠かつ、すでに混乱のコンディションが付与されているときには、睡眠のコンディションは無視する(操作不能になるため)
        if (conditionType == ConditionType.Sleep && mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == ConditionType.Confusion))
        {
            return;
        }

        // 付与されていないコンディションの場合は、付与する準備する
        PlayerConditionBase playerCondition;

        // Player にコンディションを付与
        playerCondition = conditionType switch
        {

            // TODO コンディションの種類が増えたら、ここに追加する


            ConditionType.Fatigue => mapMoveController.gameObject.AddComponent<PlayerCondition_Fatigue>(),
            _ => null
        };

        // 初期設定を実行
        //playerCondition.AddCondition(conditionType, conditionData.duration, conditionData.conditionValue, mapMoveController, symbolManager);

        // コンディション用の List に追加
        mapMoveController.AddConditionsList(playerCondition);

    }


    /// <summary>
    /// 付与されているコンディションの効果をすべて適用
    /// </summary>
    private void ApplyEffectConditions()
    {

        // 付与されているコンディションの効果を順番にすべて適用
        foreach (PlayerConditionBase condition in mapMoveController.GetConditionsList())
        {

            // コンディションの効果を適用
            condition.ApplyEffect();
        }
    }
}

