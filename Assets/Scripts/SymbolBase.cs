using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SymbolBase : MonoBehaviour
{
    public SymbolType symbolType;

    public int no;

    [SerializeField]
    protected Transform effectTran;

    [SerializeField]
    protected SpriteRenderer spriteSymbol;

    protected Tween tween;

    protected SymbolManager symbolManager;

    public bool isSymbolTriggerd;


    /// <summary>
    /// 侵入判定時のエフェクト生成用
    /// </summary>
    /// <param name="mapMoveController"></param>
    public virtual void TriggerSymbol(MapMoveController mapMoveController)
    {
        if (isSymbolTriggerd)
        {
            return;
        }

        isSymbolTriggerd = true;

        //継承しているそれぞれのクラスで処理を変える
        //DestroySymbol();

    }



    protected virtual void OnExitSymbol()
    {
        if(tween != null)
        {
            tween.Kill();
        }

        //Listからシンボルを削除
        symbolManager.RemoveSymbolsList(this);


        Destroy(gameObject,0.5f);
    }


    public virtual void OnEnterSymbol(SymbolManager symbolManager)
    {
        this.symbolManager = symbolManager;
    }


    /// <summary>
    /// シンボル画像の表示/非表示を切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchDisplaySymbol(bool isSwitch)
    {
        spriteSymbol.enabled = isSwitch;
    }


    /// <summary>
    /// シンボルのゲームオブジェクトの表示/非表示
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateSymbol(bool isSwitch)
    {
        gameObject.SetActive(isSwitch);
    }

}
