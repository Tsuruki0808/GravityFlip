using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

using Transform = UnityEngine.Transform;

public class GravityItem : MonoBehaviour
{
    public int rotationSpeed = 0;
  
    [SerializeField] private AudioSource Sound;//AudioSourceを入れるための箱
    [SerializeField] private AudioClip GetItemsound;
    private Player playerscript; // プレイヤースクリプトの参照を保持


    // Start is called before the first frame update
    void Start()
    {
        playerscript = Player.Instance;
    }

    private void Update()
    {
     transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player" && GetComponent<SpriteRenderer>().enabled == true)
        {
            //GameObject.Find("Player").SendMessage("ChangeGravity");
            playerscript.addGravityCnt(1);

            Sound.PlayOneShot(GetItemsound);//再生直後に消すと音が再生されないので、1秒後に削除します
            GetComponent<SpriteRenderer>().enabled = false;
           
        }    
    }

    void DrawOthersItem()
    {
        GetComponent<SpriteRenderer>().enabled = true;
      
    }

}
