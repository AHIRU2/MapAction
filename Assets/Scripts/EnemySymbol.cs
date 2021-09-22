using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

public enum MoveDirectionType
{
    Up,
    Down,
    Left,
    Right,
    Count
}

public class EnemySymbol : SymbolBase
{
    private Tilemap tilemapCollider;
    private BoxCollider2D boxCol;
    private float moveDuration = 0.05f;




    public override void TriggerSymbol(MapMoveController mapMoveController)
    {
        Debug.Log("移動先で敵に接触");

        //SymbolBase(親クラス)のTriggerSymbolメソッドを実行（そのまま）
        base.TriggerSymbol(mapMoveController);

        //DoTweenでエネミーのシンボルアニメをさせるので、OnExitSSymbolメソッド内で実行するのでコメントアウト
        //SceneStateManager.instance.PreparateBattleScene();

        //新しく追加エネミーのシンボルのアニメ演出。演出後、OnCompleteメソッドを使ってOnExitSymbolメソッド実行
        tween = transform.DOShakeScale(0.75f, 1.0f)
            .SetEase(Ease.OutQuart)
            .OnComplete(() => { OnExitSymbol(); });
    }

    public override void OnEnterSymbol(SymbolManager symbolManager)
    {
        base.OnEnterSymbol(symbolManager);

        //移動判定にRayを利用するので、プレイヤーと同じように、コライダーのタイルマップの情報を取得
        tilemapCollider = symbolManager.tilemapCollider;

        //BoxCollider2Dの情報を取得
        TryGetComponent(out boxCol);
    }

    protected override void OnExitSymbol()
    {
        //エネミーのシンボル用のListから削除
        symbolManager.RemoveEnemySymbol(this);

        base.OnExitSymbol();

        //バトルシーン遷移の準備
        SceneStateManager.instance.PreparateBattleScene();
    }



    /// <summary>
    /// エネミーをランダムな方向に１マス移動するか、その場で待機
    /// </summary>
    public void EnemyMove()
    {
        //移動する方向をランダムに一つ設定
        MoveDirectionType randomDirType = (MoveDirectionType)Random.Range(0, (int)MoveDirectionType.Count);

        //移動する方向の情報を座標に変換
        Vector3 nextPos = GetMoveDirection(randomDirType);

        //自分の子ライダーをオフにしてRayが自分の子ライダーに当たってしまう誤判を防ぐ
        SwitchCollider(false);

        //移動する方向にRayを投射して他のシンボルが存在していないかを確認
        RaycastHit2D hit = Physics2D.Raycast(transform.position, nextPos, 0.8f, LayerMask.GetMask("Symbol"));

        //SceneビューにてRayの可視化
        Debug.DrawRay(transform.position, nextPos, Color.blue, 0.8f);

        //コライダーをオン
        SwitchCollider(true);

        //Rayの投射先に別のシンボルがある場合にはエネミーのみとりあえず除外。アイテムの上にエネミーが乗るようになる
        if (hit.collider != null)
        {
            //移動せず終了
            return;
        }

        //Rayがヒットし、それがエネミーであるなら
        if(hit.collider!=null&& hit.collider.TryGetComponent(out EnemySymbol enemySymbol))
        {
            //移動せず終了
            return;
        }

        //移動できるタイルかタイルマップの座標に変換して確認(プレイヤーと同じ手法)
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + nextPos);

        //Debug.Log(tilePos);
        //Debug.Log(tilemapCollider.GetColliderType(tilePos));

        //Grid の子ライダーでなければ(プレイヤーと同じ手法)
        if (tilemapCollider.GetColliderType(tilePos) != Tile.ColliderType.Grid)
        {
            //移動
            transform.DOMove(transform.position + nextPos, moveDuration).SetEase(Ease.Linear);
        }

    }




    private Vector3 GetMoveDirection(MoveDirectionType nextDirection)
    {
        //switchぶんの省略記法(casse:break の代わりに=>を使う。最後の_=>はdefault:breakと同じ)
        return nextDirection switch
        {
            MoveDirectionType.Up => new Vector2(0, 1),
            MoveDirectionType.Down => new Vector2(0, -1),
            MoveDirectionType.Left => new Vector2(-1, 0),
            MoveDirectionType.Right => new Vector2(1, 0),
            _ => Vector2.zero
        };

    }


    /// <summary>
    /// コライダーのオンオフ切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchCollider(bool isSwitch)
    {
        boxCol.enabled = isSwitch;
    }
        
}
