using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
   
   
    public float Speed = 1.0f;//速度

    public  bool DontMove = false;//床を触れるまで動かさないようにするかの選択
    public bool MoveXorY = false;
    public bool ChoiceX = false;
    public float timeOut = 1.0f;//何秒で反転するか
    private float timeElapsed;//秒数をカウントする

    void Start()
    {
        if (ChoiceX == true)
        {
            Speed *= -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (DontMove == false)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= timeOut)
            {
                // Do anything
                Speed *= -1f;
                timeElapsed = 0.0f;


            }
            if (MoveXorY == false)
            {

                transform.position += new Vector3(Speed, 0f, 0) * Time.deltaTime;
            }
            else
            {
                transform.position += new Vector3(0f, Speed, 0) * Time.deltaTime;
            }
        }

    }



    void OnCollisionStay2D(Collision2D collision)
    {
        DontMove = false;//止まっていた床を動かす

        // 衝突したオブジェクト名がPlayerなら、床の子オブジェクトにする
       // if (collision.gameObject.name == "Player")
      //  {
            collision.gameObject.transform.SetParent(transform);
       // }
    }
  
    void OnCollisionExit2D(Collision2D collision)
    {
        // 衝突したオブジェクト名がPlayerなら、床の子オブジェクトから解除する
       // if (collision.gameObject.name == "Player")
      //  {
          
            collision.gameObject.transform.SetParent(null);

       // }
    }

}
