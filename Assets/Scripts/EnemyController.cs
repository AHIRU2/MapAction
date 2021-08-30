using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float intervalAttackTime = 60.0f;

    [SerializeField]
    private bool isAttack;

    public EnemyDataSO.EnemyData enemyData;

    public int enemyNo;

    public float Hp;

    private NavMeshAgent navMeshAgent;

    public PlayerController3D playerController3D;

    private Animator animator;

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
            isAttack = false;
            Destroy(gameObject,1.5f);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!isAttack && other.gameObject.tag == "Player")
        {
            Debug.Log("プレイヤーへの攻撃");

            if (other.gameObject.TryGetComponent(out playerController3D))
            {
                isAttack = true;

                StartCoroutine(PrepareteAttack());
            }
        }


    }

    /// <summary>
    /// 攻撃準備
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrepareteAttack()
    {
        Debug.Log("攻撃準備開始");

        int timer = 0;

        //攻撃中の間だけループ処理を繰り返す
        while (isAttack)
        {
            timer++;

            //攻撃のための待機時間が経過したら
            if (timer > intervalAttackTime)
            {
                //次の攻撃に備えて、待機時間のタイマーをリセット
                timer = 0;

                //攻撃
                Attack();
            }

            yield return null;
        }
    }


    private void Attack()
    {
        Debug.Log("敵の攻撃");

        GameData.instance.hp -= enemyData.attackPower;

        playerController3D.PlayerHpGuage();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("敵攻撃停止");

            isAttack = false;
            playerController3D = null;
        }
    }
}
