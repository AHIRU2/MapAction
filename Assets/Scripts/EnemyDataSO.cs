using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="EnemDataSO",menuName ="Create EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public List<EnemyData> enemyDataList = new List<EnemyData>();

    [Serializable]
    public class EnemyData
    {
        public int no; //番号
        public string name; //名前
        public GameObject enemyPrefab; //敵キャラ
        public int attackPower; //攻撃力
        public float maxHp; //HP

    }
    

}
