using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSymbol : SymbolBase
{
    public override void TrrigerSymbol()
    {
        Debug.Log("回復");

        base.TrrigerSymbol();
    }
}
