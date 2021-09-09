using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController3D : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;

    private Rigidbody rb;

    private float horizontal;                    // x 軸(水平・横)方向の入力の値の代入用
    private float vertical;                      // y 軸(垂直・縦)方向の入力の値の代入用

    private Animator animator;

    private float scale;

    [SerializeField]
    private float maxTime;

    private float timer;

    [SerializeField]
    private float actionInterval;

    [SerializeField]
    private float avoideInterval;

    [SerializeField]
    private float chargePower;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TimingGaugeController timingGaugeController;

    [SerializeField]
    private float criticalRate;

    [SerializeField]
    private float attackPower;

    public GameObject[] enemyObjects;

    public PlayerState currentPlayerState;

    public enum PlayerState{
        Wait,
        Ready,
        Attack,
        Avoidance,
        Death
        }

    // Start is called before the first frame update
    void Start()
    {
        PlayerHpGuage();

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        scale = transform.localScale.x;

        StartCoroutine(ChargeTime());
    }

    // Update is called once per frame
    void Update()
    {
        // InputManager の Horizontal に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        horizontal = Input.GetAxis("Horizontal");

        // InputManager の Vertical に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        vertical = Input.GetAxis("Vertical");

        ActionTrriger();
    }

    private void FixedUpdate()
    {
        //移動
        Move();

    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (currentPlayerState == PlayerState.Death)
        {
            return;
        }

        Vector3 dir = new Vector3(horizontal, 0, vertical);

        rb.velocity = dir*moveSpeed;

        if (horizontal != 0)
        {
            Vector3 temp = transform.localScale;

            temp.x = horizontal < 0 ? -scale : scale;

            //if (horizontal < 0)
            //{
            //    temp.x = -scale;
            //}
            //else
            //{
            //    temp.x = scale;
            //}

            transform.localScale = temp;
        }

        animator.SetFloat("speed", horizontal != 0 ? Mathf.Abs(horizontal) : Mathf.Abs(vertical));

        //if (horizontal != 0 || vertical !=0)
        //{
        //    animator.SetFloat("speed",0.2f);
        //}
        //else
        //{
        //    animator.SetFloat("speed", 0);
        //}
    }


    /// <summary>
    /// 攻撃と回避
    /// </summary>
    private void ActionTrriger()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { 

            if (currentPlayerState == PlayerState.Ready)
            {
                currentPlayerState = PlayerState.Attack;
                animator.SetTrigger("Attack");


                StartCoroutine(ActionInterval(actionInterval));

            }
            else if (currentPlayerState == PlayerState.Wait)
            {
                currentPlayerState = PlayerState.Avoidance;
                animator.SetTrigger("Avoidance");

                StartCoroutine(ActionInterval(avoideInterval));
            }

        }
    }


    /// <summary>
    /// 攻撃可能になるまでの時間
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChargeTime()
    {
        while (currentPlayerState == PlayerState.Wait)
        {
            timer += chargePower;

            if (timer >= maxTime)
            {
                currentPlayerState = PlayerState.Ready;

                // TODO 攻撃できるよエフェクト

                timer = 0;

                Debug.Log("攻撃できる");
            }

            yield return null;
        }

    }


    private IEnumerator ActionInterval(float interval)
    {
        yield return new WaitForSeconds(interval);

        currentPlayerState = PlayerState.Wait;

        StartCoroutine(ChargeTime());
    }


    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyController enemyController) && currentPlayerState==PlayerState.Attack)
        {

            StartCoroutine(timingGaugeController.PausePointer());

            float currentAttackPower = attackPower;

            if (timingGaugeController.CheckCritical())
            {
                currentAttackPower = currentAttackPower * criticalRate;

                Debug.Log("攻撃！" + currentAttackPower);
            }

            // TODO 敵に付いてるスクリプトを取得して、ダメージを引数で渡す
            enemyController.Damage(currentAttackPower);
        }
    }

    public void PlayerHpGuage()
    {
        slider.DOValue((float)GameData.instance.hp / GameData.instance.maxHp, 0.25f);

        if (GameData.instance.hp <= 0)
        {
            GameData.instance.currentGameState = GameData.GameState.GameOver;
            Debug.Log(GameData.instance.currentGameState);
            currentPlayerState = PlayerState.Death;
            animator.SetTrigger("Death");
            //Destroy(this.gameObject, 1.0f);
        }
    }

}
