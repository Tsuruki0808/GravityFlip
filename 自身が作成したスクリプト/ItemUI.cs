using UnityEngine;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private AudioSource Sound; // AudioSourceを入れるための箱
    [SerializeField] private AudioClip GetKagiSound;
    [SerializeField] private AudioClip KagianaSound;

    private bool isItemHidden = true; // フラグを追加

    private Player playerscript; // プレイヤースクリプトの参照を保持


    // Start is called before the first frame update
    void Start()
    {
        playerscript = Player.Instance;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
      
        // Player.Keyflgの状態が変わった時だけ処理を実行する
        if (playerscript.getKeyFlg() == 0 && isItemHidden == false)
        {
            HideItem();
            isItemHidden = true; // 一度実行後にフラグを設定
        }
        else if (playerscript.getKeyFlg() != 0 && isItemHidden == true)
        {
            DrawingItem();
            isItemHidden = false; // 一度実行後にフラグを設定
        }
    }

    void DrawingItem()
    {
       
        Sound.PlayOneShot(GetKagiSound);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void HideItem()
    {
       
       // Sound.PlayOneShot(KagianaSound);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
