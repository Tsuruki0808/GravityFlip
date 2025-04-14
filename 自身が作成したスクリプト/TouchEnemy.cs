using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEnemy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D c)//イズトリガーがついていないオブジェクトに触れたとき
    {
        if (c.gameObject.tag == "Player")
        {
            GameObject.Find("Player").SendMessage("GameOver");
        }
    }
}
