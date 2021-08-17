using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum PlayerState{
        Wait,
        Ready,
        Attack,
        Avoidance
        }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // InputManager の Horizontal に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        horizontal = Input.GetAxis("Horizontal");

        // InputManager の Vertical に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        vertical = Input.GetAxis("Vertical");

        AttackTrigger();

        AvoidanceTrigger();
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
    /// 攻撃
    /// </summary>
    private void AttackTrigger()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Attack");
        }
    }


    private void AvoidanceTrigger()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Avoidance");
        }
    }
}
