using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Key : MonoBehaviour
{
    // 浮遊の速度
    public float floatSpeed = 1.0f;
    // 浮遊の振幅
    public float floatAmplitude = 0.5f;

    // 初期位置を記録
    private Vector3 startPos;

    private Player playerscript; // プレイヤースクリプトの参照を保持

    // Start is called before the first frame update
    void Start()
    {
        playerscript = Player.Instance;

        // オブジェクトの初期位置を記録
        startPos = transform.position;
        // Keyflg = 0;
    }

    void Update()
    {
        // 時間経過に基づいてオブジェクトのY座標を変更
        if (floatSpeed > 0)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            // 新しい位置にオブジェクトを移動
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player" && GetComponent<SpriteRenderer>().enabled == true)
        {
            playerscript.setKeyFlg(1);
            
            GameObject.Find("ItemUI").SendMessage("DrawingItem");
            GameObject.Find("KeyUI").SendMessage("DrawingItem");
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void DrawOthersItem()
    {
        GetComponent<SpriteRenderer>().enabled = true;

    }
}
