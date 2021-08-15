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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // InputManager の Horizontal に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        horizontal = Input.GetAxis("Horizontal");

        // InputManager の Vertical に登録してあるキーが入力されたら、水平(横)方向の入力値として代入
        vertical = Input.GetAxis("Vertical");
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
    }
}
