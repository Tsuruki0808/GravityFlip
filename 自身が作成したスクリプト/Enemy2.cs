using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private Rigidbody2D rb; // プレイヤーのRigidbody2Dコンポーネント

    int EnemyX = 0;
    int EnemyY = 0;

    public bool Stopfloor = false;
    public bool EnemyGravity = true;

    public float EnemySpeed = 2.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // オブジェクトがカメラに映っている場合のみ処理を行う
        if (isVisibleByCamera())
        {
            var velocity = Vector3.zero;
            velocity.x = EnemySpeed;
            transform.position += transform.rotation * -velocity * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, 0); // 回転をゼロにすることで、オブジェクトの回転を固定します。
            AddGravity();
        }
        else
        {
            // Object is not visible, stop its movement
            StopMovement();
        }
    }

    void StopMovement()
    {
        // Set velocity to zero to stop movement
        rb.velocity = Vector2.zero;
    }

    // オブジェクトがカメラに映っているかどうかを判定するメソッド
    bool isVisibleByCamera()
    {
        return GetComponent<Renderer>().isVisible;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        //ぼたんにふれたらはんてんする
        if (c.gameObject.tag == "Botton")
        {
            ReturnX();
        }

        else if (c.gameObject.tag == "Enemy")
        {
            ReturnX();
        }
    }

    void ReturnX()
    {
        // X軸の方向を反転させる
        EnemySpeed *= -1;

        // 敵のスプライトを水平方向に反転させる
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        
        if (EnemyX == 0)
        {
            EnemyX = 180;

        }
        else
        {
            EnemyX = 0;
        }
    }


    void AddGravity()
    {
        if (EnemyGravity == false)
        {
            EnemyY = 180;
            Vector2 myGravity = new Vector2(0, 1.0f);
            rb.AddForce(myGravity);
        }
        else
        {
            EnemyY = 0;
            Vector2 myGravity = new Vector2(0, -1.0f);
            rb.AddForce(myGravity);

        }
    }
}
