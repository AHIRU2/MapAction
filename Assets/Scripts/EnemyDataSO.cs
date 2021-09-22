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
        public EnemyController enemyPrefab; //敵キャラ
        public int attackPower; //攻撃力
        public float maxHp; //HP
        public float moveSpeed; //速さ
        public int exp;
        public float attackInterval;

        //デバフ用のコンディションのデータ
        public EnemyDebuffData[] debuffDatas;
    }



    /// <summary>
    /// デバフ用のコンディションの登録用
    /// </summary>
    [System.Serializable]
    public class EnemyDebuffData
    {
        //デバフ用のコンディションの設定
        public ConditionType debuffconditionType;

        //デバフ用のコンディションの付与確率
        [Range(0, 100)]
        public int rate;
    }
    

}
