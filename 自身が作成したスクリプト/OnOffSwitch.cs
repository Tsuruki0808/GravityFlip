using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OnOffSwitch : MonoBehaviour
{

    [SerializeField] private AudioSource Sound;//AudioSourceを入れるための箱
    [SerializeField] private AudioClip SwitchSound;

    float bottonx = 0;

    [SerializeField]
    GameObject SwitchEfe_prefab;//プレファブ化した球の用意  

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        // タグを指定してゲームオブジェクトを取得
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Box");
        transform.rotation = Quaternion.Euler(0, bottonx, gameObject.transform.localEulerAngles.z); // プレイヤーを→向きにする


        /* if (Input.GetKeyDown(KeyCode.M))
         {
             foreach (GameObject Box in enemyObjects)
             {
                 GameObject.Find(Box.name).SendMessage("SwitchReturn");
             }
             // SwitchReturn();
         }*/
    }


    void OnCollisionEnter2D(Collision2D c)//イズトリガーがついていないオブジェクトに触れたとき
    {
        if (c.gameObject.tag != "Ground")
        {
            Sound.PlayOneShot(SwitchSound);

            SendMessage();
            GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Box");

            Instantiate(SwitchEfe_prefab,
               transform.position, transform.rotation);

            foreach (GameObject Box in enemyObjects)
            {

                GameObject.Find(Box.name).SendMessage("SwitchReturn");
            }
        }
    }


    void OnTorigerEnter2D(Collision2D c)//イズトリガーがついていないオブジェクトに触れたとき
    {
        if (c.gameObject.name != "Ground")
        {
            Sound.PlayOneShot(SwitchSound);
            SendMessage();
            GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Box");

            Instantiate(SwitchEfe_prefab,
               transform.position, transform.rotation);

            foreach (GameObject Box in enemyObjects)
            {
                
                GameObject.Find(Box.name).SendMessage("SwitchReturn");
            }
        }
    }

    void SendMessage()
    {     
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Botton");

        foreach (GameObject Botton in gameObjects)
        {
         
            GameObject.Find(Botton.name).SendMessage("returnbottan");
        }
    }

    void returnbottan()//イラストの反転
    {     
        if (bottonx == 0)
        {
            bottonx = 180;

        }
        else
        {
            bottonx = 0;
        }
    }
}