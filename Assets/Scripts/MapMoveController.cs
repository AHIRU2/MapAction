using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class MapMoveController : MonoBehaviour
{
    private Vector3 movePos;
    private float moveDuration = 0.5f;

    [SerializeField]
    private Tilemap tilemapCollider;

    [SerializeField]
    private bool isMoving;

    // Update is called once per frame
    void Update()
    {
        //キー入力の確認
        InputMove(); 
    }


    /// <summary>
    /// キー入力判定
    /// </summary>
    public void InputMove()
    {
        // TODO 移動禁止なら処理しない

        //移動中には処理しない
        if (isMoving)
        {
            return;
        }

        //キー入力値の受け取り
        movePos.x = Input.GetAxisRaw("Horizontal");
        movePos.y = Input.GetAxisRaw("Vertical");

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

        // 移動
        transform.DOMove(destination, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isMoving = false;
            });
    }
}
