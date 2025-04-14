using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyHantei : MonoBehaviour
{
    int charastate = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        Debug.Log("キャラの状態＝" + charastate);
    }

   public void ChangeBodyHaneti(int num)
    {
        //待機:1
        //歩く:2
        //ジャンプ:3
        //死亡:4
        //ゴール:5
        charastate = num;
    }
}
