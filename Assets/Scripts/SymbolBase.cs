using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolBase : MonoBehaviour
{
    public SymbolType symbolType;

    public virtual void TrrigerSymbol()
    {
        //継承しているそれぞれのクラスで処理を変える
        DestroySymbol();

    }

    protected virtual void DestroySymbol()
    {
        Destroy(gameObject,0.5f);
    }

}
