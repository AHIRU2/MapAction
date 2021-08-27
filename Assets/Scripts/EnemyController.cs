using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyDataSO.EnemyData enemyData;

    public int enemyNo;

    public float Hp;

    private void Start()
    {
        SetUpEnemy();
    }



    public void SetUpEnemy()
    {
        enemyData = DataBaseManager.instance.enemyDataSO.enemyDataList.Find(x => x.no == enemyNo);

        Hp = enemyData.maxHp;

    }

    public void Damage(float attackPower)
    {
        Hp -= attackPower;

        if (Hp <= 0)
        {
            Destroy(gameObject,0.5f);
        }
    }
}
