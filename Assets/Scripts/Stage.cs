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

    private void OnEnable()
    {
        uiManager.DisplayHpGauge();
    }

    private void Start()
    {
        stageGenerator.GenerateStageFromRandomTiles();
        symbolManager.SymbolsList = stageGenerator.GenerateSymbols(-1);
        symbolManager.SetUpAllAymbols();
    }
}
