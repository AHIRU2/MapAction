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
        if (GameData.instance.MaxMoveCount <= 0)
        {
            //移動できないならボスのターンにする
            CurrentTurnState = TurnState.Boss;
        }
        else
        {
            //まだ移動できるなら、プレイヤーのターンにする
            CurrentTurnState = TurnState.Player;

            // TODO コンディションの残り時間の更新

            // TODO 移動ボタンと足踏みボタンを押せる状態にする

            // TODO コンディションの効果を適用

            //移動の入力を受け付けるようにする
            mapMoveController.IsMoving = false;
        }

        Debug.Log(CurrentTurnState);
    }


    public SymbolManager GetSymbolManager()
    {
        return symbolManager;
    }
}

