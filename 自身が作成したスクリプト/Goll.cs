using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Goll : MonoBehaviour
{

    [SerializeField] private AudioSource Sound;//AudioSourceを入れるための箱
    [SerializeField] private AudioClip Gollsound;//ゴールした時の音の音

    private bool goolflg = false;
    public string NextStage = "";

    public float GoNextStageTimer = 4.0f;



    private Player playerscript; // プレイヤースクリプトの参照を保持

    // Start is called before the first frame update
    void Start()
    {
        playerscript = Player.Instance;

    }
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))//またはBボタン
        {
            if (goolflg == false)
            {
                GameObject.Find("Player").SendMessage("GollAnime");
                Sound.PlayOneShot(Gollsound);//ゴールしたサウンド
           
                goolflg = true;

                playerscript.resetCheckPoint();
                GameObject.Find("GameManager").SendMessage("SetClearflg");
                SceneManager.LoadScene(NextStage);
            }

        }
    }*/


        void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            if (goolflg == false)
            {
                GameObject.Find("Player").SendMessage("GollAnime");
                Sound.PlayOneShot(Gollsound);//ゴールしたサウンド
                Invoke(nameof(GoNextStage), GoNextStageTimer);
                goolflg = true;

                playerscript.resetCheckPoint();
            }
        }
    }

    void GoNextStage()
    {
        GameObject.Find("GameManager").SendMessage("SetClearflg");
        SceneManager.LoadScene(NextStage);
    }

}
