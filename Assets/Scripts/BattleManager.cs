using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private int maxEnemy;

    [SerializeField]
    private int totalComboCount;

    [SerializeField]
    private int currentBattleTotalExp; //バトルでエネミーから獲得した経験値の総数

    [SerializeField]
    private NomalResultCanvas nomalResultCanvas;　

    public int bonusStaminaPoint; //バトルクリア時にスタミナを回復する場合

    public int deathEnemyCount;

    // Start is called before the first frame update
    void Start()
    {
        // TODO この中の処理が増えてきたら、メソッド化して処理をまとめる

        //リザルト用のCanvasを隠す
        nomalResultCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void UpdateDethEnemycount()
    {
        deathEnemyCount++;

        if (maxEnemy<=deathEnemyCount)
        {
            //SceneStateManager.instance.PreparateStageScene(SceneName.Main);
            StartCoroutine(OnExit());
        }
    }


    /// <summary>
    /// バトル終了時の処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnExit()
    {
        Debug.Log("バトル終了時の処理");

        // TODO ここに待機処理を入れるとエネミーを倒した後、リザルト表示されるまでの間を作れる

        //リザルト用のCanvasを表示
        nomalResultCanvas.gameObject.SetActive(true);

        //リザルト内容を表示してアニメ演出
        nomalResultCanvas.DisplayResult(currentBattleTotalExp, totalComboCount);

        //リザルト表示＋Battle終了の余韻
        yield return new WaitForSeconds(3.5f);

        // TODO ノーダメージボーナスの判定

        //今回のバトルで獲得したEXPを総Expに加算
        GameData.instance.totalExp += currentBattleTotalExp;

        //今回のバトルがボスなのか判定
        if (GameData.instance.isBossBattled)
        {
            //ボスの場合、ボス討伐したので次のボスバトルのために初期化
            GameData.instance.isBossBattled = false;

            // TODO クリア演出

            Debug.Log("ボスバトル終了：ワールドへ戻る");

            // TODO ワールドへ遷移(ワールドシーンを作ってから実装)
            //SceneStateManager.instance.PreparateBattleScene(SceneName.World);

        }
        else
        {
            //ボスではない場合

            //スタミナ獲得（任意機能）
            GameData.instance.staminaPoint += bonusStaminaPoint;

            Debug.Log("バトル終了：ステージへ戻る");

            //Stageへ戻る
            SceneStateManager.instance.PreparateStageScene(SceneName.Main);
        }


    }


    /// クリティカル(コンボした)総数のカウントアップ
    /// </summary>
    public void AddTotalBattleCount()
    {
        totalComboCount++;
    }

    /// <summary>
    /// バトル内で獲得した EXP のカウントアップ
    /// </summary>
    /// <param name="exp"></param>
    public void AddCurrentBattleTotalExp(int exp)
    {
        currentBattleTotalExp += exp;
    }

}
