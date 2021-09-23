using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using UnityEngine.Events;

public class MapMoveController : MonoBehaviour
{
    private Vector3 movePos;
    private float moveDuration = 0.5f;

    [SerializeField]
    private Tilemap tilemapCollider;

    [SerializeField]
    private bool isMoving;

    [SerializeField]
    private List<PlayerConditionBase> conditionsList = new List<PlayerConditionBase>();

    [SerializeField]
    private Transform conditionEffectTran;

    public GameManager gameManager;

    public UIManager uiManager;

    public SceneStateManager sceneStateManager;

    public bool IsMoving { set => isMoving = value; get => isMoving; }

    private Stage stage;

    private int steppingRecoveryPoint = 3; //足踏みした時のHP回復量

    private UnityEvent<MapMoveController> enemySymbolTriggerEvent;

    private UnityEvent<MapMoveController> orbSymbolTriggerEvent;


    private void Start()
    {
        //デバッグ用

        //MapMoveControllerクラスのアタッチされているゲームオブジェクト（Player）に新しいクラス（PlayerCondition＿Fatigue）を追加する
        PlayerConditionBase condition = gameObject.AddComponent<PlayerCondition_Fatigue>();

        //PlayerConditionBaseクラスのAddConditonメソッドを実行する。引数は左から順番に（コンディションの種類、コンディションの持続時間、コンディションの効果(今回は攻撃力に乗算する値)、MapMoveControllerクラス、SymbolManagerクラス）
        //condition.AddCondition(ConditionType.Fatigue, 5, 0.5f, this, stage.GetSymbolManager());

        //コンディション用のリストに追加
        //conditionsList.Add(condition);

    }

    // Update is called once per frame
    void Update()
    {
        if (stage != null)
        {
            //キー入力の確認
            InputMove();
        }

    }


    /// <summary>
    /// キー入力判定
    /// </summary>
    public void InputMove()
    {

        //プレイヤーの番でなければ処理しない
        if (stage.CurrentTurnState != Stage.TurnState.Player)
        {
            return;
        }

        // TODO 移動禁止なら処理しない
        if (GameData.instance.staminaPoint <= 0)
        {
            return;
        }

        //移動中には処理しない
        if (isMoving)
        {
            return;
        }

        //キー入力値の受け取り
        movePos.x = Input.GetAxisRaw("Horizontal");
        movePos.y = Input.GetAxisRaw("Vertical");

        //取得タイミングによって不用意な数値が入るので、その場合には処理しない
        if (movePos == Vector3.zero)
        {
            return;
        }

        isMoving = true;

        //斜め移動はなしにする
        if (Mathf.Abs(movePos.x) != 0)
        {
            movePos.y = 0;
        }

        //タイルマップの座標に変換
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + movePos);

        Debug.Log(tilemapCollider.GetColliderType(tilePos));

        //Gridの子ライダーの場合
        if (tilemapCollider.GetColliderType(tilePos) == Tile.ColliderType.Grid)
        {
            //移動しないで終了
            isMoving = false;

            //Grid以外の場合
        }
        else
        {
            //移動させる
            Move(transform.position + movePos);
        }

    }


    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="destination"></param>
    private void Move(Vector2 destination)
    {
        // TODO 移動できる回数を減算する
        GameData.instance.staminaPoint--;

        //スタミナ表示更新
        gameManager.staminaPoint.text = GameData.instance.staminaPoint.ToString();

        // 移動
        transform.DOMove(destination, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                //エネミーのターンがおわっれからfalseにしてキー入力を受け付けるようにするのでコメントアウト
                //isMoving = false;

                //敵のターン開始
                StartCoroutine(stage.ObserveEnemyTurnState());

            });

        //ターンのチェック時にスタミナのチェックを行うためコメントアウト
        //if (GameData.instance.staminaPoint == 0)
        //{
        //    sceneStateManager.PreparateBattleScene();
        //}

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Symbolスクリプトがついているか確認して処理を実行
        if (collision.TryGetComponent(out SymbolBase symbolBase))
        {
            //シンボルの種類によって分岐するのでコメントアウト
            //symbolBase.TriggerSymbol(this);

            // TODO エネミーのシンボルに接触したさい、プレイヤーに透明のコンディションが付与されている場合

            //同じシンボルに接触した場合は処理しない
            if (symbolBase.isSymbolTriggerd)
            {
                return;
            }

            Debug.Log("移動先でシンボルに接触：" + symbolBase.symbolType.ToString());

            //エネミーシンボルの場合
            if (symbolBase.symbolType == SymbolType.Enemy)
            {
                //エネミーのシンボルイベントの重複登録はしない
                if (enemySymbolTriggerEvent != null)
                {
                    return;
                }

                symbolBase.isSymbolTriggerd = true;

                //シンボルイベントを登録して予約し、全てのエネミーの移動が終了してから実行
                enemySymbolTriggerEvent = new UnityEvent<MapMoveController>();
                enemySymbolTriggerEvent.AddListener(symbolBase.TriggerSymbol);

                Debug.Log("エネミーとのバトルを登録");
            }

            // TODO　特殊シンボルの場合、特殊シンボル用のイベントを登録して予約し、バトル後stageに戻ってきてから実行

            if (symbolBase.symbolType != SymbolType.Enemy)
            {
                // TODO 呪いのコンディション
                //呪い状態である時は、シンボルのイベントを発生させない

                //それ以外のシンボルはすぐに実行
                symbolBase.TriggerSymbol(this);
            }

        }

    }



    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="stage"></param>
    public void SetUpMapMoveController(Stage stage)
    {
        this.stage = stage;
    }



    /// <summary>
    /// 足踏み
    /// </summary>
    public void Stepping()
    {
        //プレイヤーの番でなければ処理しない
        if (stage.CurrentTurnState != Stage.TurnState.Player)
        {
            return;
        }

        GameData.instance.staminaPoint--;

        //足踏みしてHP回復
        GameData.instance.hp = Mathf.Clamp(GameData.instance.hp += steppingRecoveryPoint, 0, GameData.instance.maxHp);

        //HPゲージ更新
        uiManager.DisplayHpGauge();

        //敵のターン
        StartCoroutine(stage.ObserveEnemyTurnState());
    }


    /// <summary>
    /// 登録されているエネミーシンボルイベント（エネミーとのバトル）を実行
    /// </summary>
    /// <returns></returns>
    public bool CallBackEnemySymbolTriggerEvent()
    {
        if (enemySymbolTriggerEvent != null)
        {
            //変数名の最後に？がある場合、変数の値がnull出ない場合のみ実行。つまり、イベントがある時だけ実行する
            enemySymbolTriggerEvent?.Invoke(this);

            //イベントをクリア。上記同様変数内に値があるかを確認して、ある場合のみ、Remove~してイベントをクリアする
            enemySymbolTriggerEvent?.RemoveAllListeners();
            enemySymbolTriggerEvent = null;

            return true;

        }

        return false;
    }


    /// <summary>
    /// Stageの情報を取得
    /// </summary>
    /// <returns></returns>
    public Stage GetStage()
    {
        return stage;
    }
}

