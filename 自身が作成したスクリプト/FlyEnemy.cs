using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{

    int FlyEnemyX = 180;

    public float Speed = 1.0f;//速度

    public bool MoveXorY = false;
    public bool ChoiceX = false;
    public float timeOut = 1.0f;//何秒で反転するか
    private float timeElapsed;//秒数をカウントする


    [SerializeField] GameObject target;


    void Start()
    {
        target = GameObject.Find("Player");//プレイヤーがどこにいるかの取得
        if (ChoiceX == true)
        {
            Speed *= -1f;//最初にどちらに進むか
            FlyEnemyX = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {

       


        transform.rotation = Quaternion.Euler(0, FlyEnemyX, 0); // プレイヤーを→向きにする
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeOut)
        {
            // Do anything
            Speed *= -1f;
            timeElapsed = 0.0f;
            ReturnX();

        }

        if (MoveXorY == false)
        { 
            //横移動
            transform.position += new Vector3(Speed, 0f, 0) * Time.deltaTime;
        }
        else
        {
            //上下移動
            CheckPlayerx();//プレイヤーの方向に向くようにする
            transform.position += new Vector3(0f, Speed, 0) * Time.deltaTime;
        }

    }

    void CheckPlayerx()
    {

        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb.transform.position.x >= transform.position.x)
        {
            FlyEnemyX = 180;

        }
        else
        {
            FlyEnemyX = 0;
        }
    }

    void ReturnX()
    {
        
        if (MoveXorY == false)
        {

            if (FlyEnemyX == 0)
            {
                FlyEnemyX = 180;

            }
            else
            {
                FlyEnemyX = 0;
            }
        }

    }

}
