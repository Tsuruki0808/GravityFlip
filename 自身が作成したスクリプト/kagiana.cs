using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kagiana : MonoBehaviour
{
    [SerializeField] private AudioSource Sound; // AudioSourceを入れるための箱
  
    [SerializeField] private AudioClip KagianaSound;

    [SerializeField]
    GameObject KagianaEfe_prefab; 

    private Player playerscript; // プレイヤースクリプトの参照を保持

    // Start is called before the first frame update
    void Start()
    {
        playerscript = Player.Instance;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (GetComponent<SpriteRenderer>().enabled)
        {
            // 鍵を持っているなら
            if (playerscript.getKeyFlg() > 0)
            {
                Instantiate(KagianaEfe_prefab, transform.position, transform.rotation);

                Sound.PlayOneShot(KagianaSound);

                SetActiveState(false); // 親と子のSpriteRendererを隠し、BoxCollider2Dを無効にする
                
                playerscript.setKeyFlg(-1);
            }
            // 鍵を持っていないときの処理
            else
            {
                // 鍵を持っていないときの処理をここに追加
            }
        }
    }
    //ここから親と子供すべて隠してあたり判定も削除する機能
    void DrawOthersItem()
    {
        SetActiveState(true); // 親と子のSpriteRendererを表示し、BoxCollider2Dを有効にする
    }

    // 親オブジェクトと子オブジェクトのSpriteRendererとBoxCollider2Dを操作するメソッド
    void SetActiveState(bool isActive)
    {
        // 親オブジェクトのSpriteRendererとBoxCollider2Dを操作
        SpriteRenderer parentRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D parentCollider = GetComponent<BoxCollider2D>();

        if (parentRenderer != null)
        {
            parentRenderer.enabled = isActive;
        }
        if (parentCollider != null)
        {
            parentCollider.enabled = isActive;
        }

        // 子オブジェクトのSpriteRendererとBoxCollider2Dを操作
        foreach (Transform child in transform)
        {
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            BoxCollider2D childCollider = child.GetComponent<BoxCollider2D>();

            if (childRenderer != null)
            {
                childRenderer.enabled = isActive;
            }
            if (childCollider != null)
            {
                childCollider.enabled = isActive;
            }
        }
    }
    //ここまで親と子供すべて隠してあたり判定も削除する機能
}
