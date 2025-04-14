using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityItemUI : MonoBehaviour
{
  
    public int DrawGravity = 0;
    private Player playerscript; // プレイヤースクリプトの参照を保持


    // Start is called before the first frame update
    void Start()
    {
        playerscript = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (playerscript.getGravityCnt() >= DrawGravity)
        {
         
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
