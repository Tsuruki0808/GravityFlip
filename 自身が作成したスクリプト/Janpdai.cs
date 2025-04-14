using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Janpdai : MonoBehaviour
{
    [SerializeField] private AudioSource Sound;//AudioSourceを入れるための箱
    [SerializeField] private AudioClip JanpSound;

    public float jumpForce = 500f; // ジャンプにかかる力


    [SerializeField]
    GameObject Wind_prefab;//プレファブ化した球の用意  



    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name != "Ground")
        {
            Jump(c.gameObject); // プレイヤーをジャンプさせる
        }
    }

    private void Jump(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
      
        if (rb != null)
        {
            Sound.PlayOneShot(JanpSound);//箱を破壊したサウンド

                // ジャンプの方向を上に向けて力を加える
                rb.velocity = new Vector2(rb.velocity.x, 0f); // 現在の速度をリセット
                rb.AddForce(transform.up.normalized * jumpForce * Time.deltaTime, ForceMode2D.Impulse);


                Instantiate(Wind_prefab,
                transform.position, transform.rotation);
          
        }
    }
    
}


/*
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Janpdai : MonoBehaviour
{
    [SerializeField] private AudioSource Sound;//AudioSourceを入れるための箱
    [SerializeField] private AudioClip JanpSound;

    public float jumpForce = 500f; // ジャンプにかかる力


    [SerializeField]
    GameObject Wind_prefab;//プレファブ化した球の用意  



    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name != "Ground")
        {
            Jump(c.gameObject); // プレイヤーをジャンプさせる
        }
    }

    private void Jump(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
      
        if (rb != null)
        {
            Sound.PlayOneShot(JanpSound);//箱を破壊したサウンド
            if (rb.transform.rotation.x == 0)
            {
                // ジャンプの方向を上に向けて力を加える
                rb.velocity = new Vector2(rb.velocity.x, 0f); // 現在の速度をリセット
                rb.AddForce(Vector2.up * jumpForce * Time.deltaTime, ForceMode2D.Impulse);


                Instantiate(Wind_prefab,
                transform.position, transform.rotation);
            }
            else
            {            
                // ジャンプの方向を下に向けて力を加える
                rb.velocity = new Vector2(rb.velocity.x, 0f); // 現在の速度をリセット
                rb.AddForce(Vector2.up * -jumpForce * Time.deltaTime, ForceMode2D.Impulse);


                Instantiate(Wind_prefab,
                transform.position, Quaternion.Euler(0, 0, 180));
            }
        }
    }
    
}

 */