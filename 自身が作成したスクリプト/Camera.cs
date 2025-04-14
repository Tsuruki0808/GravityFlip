using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Camera : MonoBehaviour
{
    private GameObject player;   // ユニティちゃんのオブジェクト情報を格納
    private Vector3 offset;      // カメラとの相対距離を格納

    public float CameraX = 0;
    public float CameraY = 0;
    public bool CameraflgY = true;

    public bool CameraflgX = false;

    private float fixedX;  // 横方向固定用の初期X座標

    void Start()
    {
        this.player = GameObject.Find("Player");

        // 横方向が固定されている場合の初期X座標を保存
        if (CameraflgX)
        {
            fixedX = transform.position.x;
        }
    }

    void Update()
    {
        Vector3 position = this.transform.position;

        // 横方向が固定でない場合、プレイヤーに追従
        if (CameraflgX == false)
        {
            position.x = player.transform.position.x + CameraX;
        }
        else
        {
            // 横方向を固定する場合、初期のX座標を使用
            position.x = fixedX;
        }

        // 縦方向の制御（フラグに基づく）
        if (CameraflgY == false)
        {
            position.y = player.transform.position.y + CameraY;
        }

        // カメラの新しい位置を設定
        this.transform.position = position;
    }
}
