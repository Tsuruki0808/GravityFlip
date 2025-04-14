using UnityEngine;
using System.Collections;
using UnityEngine.UI; // UIコンポーネントの使用
using UnityEngine.EventSystems; // EventSystemの使用

public class menu : MonoBehaviour
{
    // 前回クリックしたボタンの名前を保存するキー
    private const string LastSelectedButtonKey = "LastSelectedButton";

    // ステージセレクトが読み込まれたときに前回のボタンにフォーカスを移す
    private void Start()
    {
        // 保存されたボタン名を取得
        string lastButtonName = PlayerPrefs.GetString(LastSelectedButtonKey, null);

        if (!string.IsNullOrEmpty(lastButtonName))
        {
            // ボタンオブジェクトを探す
            GameObject lastButton = GameObject.Find(lastButtonName);
            if (lastButton != null)
            {
                // イベントシステムでファーストセレクトを設定
                EventSystem.current.SetSelectedGameObject(lastButton);
            }
        }
    }

    // ボタンがクリックされたときに呼び出す
    public void OnStageButtonClicked(Button clickedButton)
    {
        // クリックしたボタンの名前を保存
        PlayerPrefs.SetString(LastSelectedButtonKey, clickedButton.gameObject.name);
        PlayerPrefs.Save();
    }
}