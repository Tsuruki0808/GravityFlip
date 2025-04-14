using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyMoveEnemy : MonoBehaviour
{
    private Rigidbody2D rb; // プレイヤーのRigidbody2Dコンポーネント
    public float EnemySpeed = 2.0f;

    int EnemyX = 0;
    int EnemyY = 0;

    private bool EnemyGravity2 = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        var velocity = Vector3.zero;
        velocity.x = EnemySpeed;
        transform.position += transform.rotation * -velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(EnemyY, EnemyX, 0); // プレイヤーを→向きにする
       // AddGravity2();
    }
   

    void ReturnX()
    {    
        //地面ぎりぎりで反転するなら
        
            if (EnemyX == 0)
            {
                EnemyX = 180;

            }
            else
            {
                EnemyX = 0;
            }
        
    }


    void AddGravity2()//実際に重力を加える
    {
        if (EnemyGravity2 == false)
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
