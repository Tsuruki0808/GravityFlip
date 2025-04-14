using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    // ボタンが押されたときに再生する音（InspectorでAudioClipを割り当てる）
    public AudioClip soundEffect;

    // AudioSourceコンポーネント（音の再生に必要）
    // インスペクターからアタッチするためpublicに設定
    public AudioSource audioSource;

    void Start()
    {
        // AudioSourceコンポーネントが未設定の場合にのみ追加
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // ボタンが押されたときに呼び出される関数
    public void StringArgFunction(string sceneName)
    {



        if (string.IsNullOrEmpty(sceneName))
        {
            
            return; // 処理を終了
        }


        // GameObject.Find("StageManager").SendMessage("DeselectButton");
        // コルーチンを開始して音の再生完了を待つ
        StartCoroutine(PlaySoundAndChangeScene(sceneName));
    }

    private IEnumerator PlaySoundAndChangeScene(string sceneName)
    {
        if (soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect);

            // 音が再生中かどうかをチェックしながら待機
            while (audioSource.isPlaying)
            {
                yield return null; // 次のフレームまで待機
            }

        }

        // 音の再生が終わった後にシーンを変更
        SceneManager.LoadScene(sceneName);
    }
}
