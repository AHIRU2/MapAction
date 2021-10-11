using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Create StageDataSO")]
public class StageDataSO : ScriptableObject
{
    public List<StageData> stageDataList = new List<StageData>();

    [System.Serializable]
    public class StageData
    {
        public string stageName;
        public int stageNo;
        public Sprite stageView;         //ステージの背景画像
        public Transform playerIconTran; //プレイヤーアイコンの配置場所
        public int initStamina;          //ステージ開始時の初期スタミナ
        public int[] appearEnemyNos;     //出現するエネミーの種類
        public int bossNo;               //出現するボスの種類
        public int clearBonusPoint;      //クリアした時のボーナス
        public StageType stageType;      //ステージのタイルマップの種類
    }
}
