using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Janphantei : MonoBehaviour
{

    public float CoyoteTime = 0.15f;
    private Player playerscript; // プレイヤースクリプトの参照を保持


    // Start is called before the first frame update
    void Start()
    {
        playerscript = Player.Instance;
    }

    void OnTriggerEnter2D(Collider2D c)//先行入力フラグが真で床に触れたならジャンプする
    {
        //先行入力がされていたら
        if (playerscript.getAdvanceJanpflg() == true)
        {
            //設置した床がジャンプできるものかどうか
            if (c.gameObject.tag == "Ground" || c.gameObject.tag == "Box" || c.gameObject.tag == "Botton" || c.gameObject.tag == "BossBotton")
            {
                playerscript.AddJanp();
            }
            else
            {
                Debug.Log("床じゃないからとばん。");

            }
        }
    }

    void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag == "Ground" || c.gameObject.tag == "Box" || c.gameObject.tag == "Botton" || c.gameObject.tag == "BossBotton")
        {
          
            playerscript.changeJanpFlg(false);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        StartCoroutine(DontJanpCoroutine(CoyoteTime));
       
    }

    IEnumerator DontJanpCoroutine(float waitTime)
    {
        // 指定した秒数だけ待機
        yield return new WaitForSeconds(waitTime);

        // その後、以下の処理を実行
        playerscript.changeJanpFlg(true);
      
    }



}