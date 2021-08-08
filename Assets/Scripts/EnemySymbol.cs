using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySymbol : MonoBehaviour
{
    public SymbolType symbolType;

    public void TriggerEnemy()
    {
        Debug.Log("バトルシーン移動");
        SceneStateManager.instance.MoveBattleScene();
    }
}
