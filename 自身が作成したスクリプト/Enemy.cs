using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  
   

    int EnemyX = 0;
    int EnemyY = 0;
   

    private Rigidbody2D rb; // プレイヤーのRigidbody2Dコンポーネント

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
        var velocity = Vector3.zero;
        velocity.x = EnemySpeed;
        transform.position += transform.rotation * -velocity * Time.deltaTime; ;
        transform.rotation = Quaternion.Euler(EnemyY, EnemyX, 0);
        AddGravity();
    }
    void OnCollisionEnter2D(Collision2D c)//イズトリガーがついていないオブジェクトに触れたとき
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
        //地面ぎりぎりで反転するなら

        if (Stopfloor == true)
        {
            if (EnemyX == 0)
            {
                EnemyX = 180;

            }
            else
            {
                EnemyX = 0;
            }
        }

    }

   

    void AddGravity()//実際に重力を加える
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