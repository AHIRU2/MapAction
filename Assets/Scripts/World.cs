using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class World : MonoBehaviour
{
    [SerializeField]
    private Transform playerTran;

    [SerializeField]
    private Button btnSubmit;

    //[SerializeField]
    //private GameObject stageSelectDetailPrefab;

    [SerializeField]
    private Transform[] stageSelectDetailTran;

    private StageGenerator stageGenerator;

    public void StageChoice1()
    {
        //stageGenerator.GenerateStageFromRandomTiles(StageType.Field);

        playerTran.position = stageSelectDetailTran[0].position;

    }


}
