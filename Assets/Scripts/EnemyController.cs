using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyDataSO.EnemyData enemyData;

    public int enemyNo;

    public float Hp;

    private NavMeshAgent navMeshAgent;

    public PlayerController3D playerController3D;

    private void Start()
    {
        SetUpEnemy();
    }

    private void Update()
    {
        if (navMeshAgent != null && playerController3D!=null)
        {
            navMeshAgent.SetDestination(playerController3D.transform.position);
        }
    }

    public void SetUpEnemy()
    {
        enemyData = DataBaseManager.instance.enemyDataSO.enemyDataList.Find(x => x.no == enemyNo);

        Hp = enemyData.maxHp;

        if(TryGetComponent(out navMeshAgent))
        {
            navMeshAgent.speed = enemyData.moveSpeed;
            navMeshAgent.SetDestination(playerController3D.transform.position);
        }

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
