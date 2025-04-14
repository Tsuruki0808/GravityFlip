using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int RisetCnt = 0;
    static int StageCnt = 20; //総ステージ数
    bool[] Clearflg = new bool[StageCnt + 1];

    public static string sceneName = "SceneNameHere";//前回板ステージセレクトがどれかを調べる

    public AudioSource audioSource;
    public AudioClip[] bgmClips; // 5つのBGMをアタッチする配列
    float BGMvolume;

    public static GameManager Instance
    {
        get; private set;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // シーンがロードされたときに PlayBGMForScene を呼び出すイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // イベントを解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        PlayBGMForScene(); // シーンがロードされるたびに呼び出す
    }

    void Start()
    {
       
        BGMvolume = audioSource.volume;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
      //  PlayBGMForScene(); // ゲーム開始時に適切なBGMを再生
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "StageSelect" ||
              SceneManager.GetActiveScene().name == "StageSelect2")
        {

            sceneName = SceneManager.GetActiveScene().name;//ステージセレクトのステージ名を記憶
            for (int i = 1; i <= StageCnt; i++)
            {
                //対応した王冠があれば
                GameObject crownStage = GameObject.Find("CrownStage" + i); 
                if (crownStage != null)
                {
                    GameObject.Find("CrownStage" + i).GetComponent<SpriteRenderer>().enabled = Clearflg[i];
                }
            }
            if (Input.GetKey(KeyCode.P))
            {
                RisetCnt += 1;
                if (RisetCnt > 300)
                {
                    for (int i = 1; i <= StageCnt; i++)
                    {
                        Clearflg[i] = false;
                    }
                }
            }
            else
            {
                RisetCnt = 0;
            }
        }
    }

    void SetClearflg()
    {
  
        
            for (int i = 1; i <= StageCnt; i++)
            {
                if (SceneManager.GetActiveScene().name == "Stage" + i)
                {
                    Clearflg[i] = true;
                    Debug.Log(i + "のステージをクリア");
                }
           }
        
       
    }

    void PlayBGMForScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "StageSelect" ||
            sceneName == "StageSelect2")
        {
            audioSource.clip = bgmClips[0]; // ステージセレクト用のBGM
          
        }
        else if (sceneName == "Stage12")
        {
            audioSource.Stop();//ボス戦前にBGMをストップ
            return;
        }
        else if (sceneName == "BossScene")
        {
            audioSource.clip = bgmClips[2]; // ボス戦前のBGM 
            return; // ここで終了
        }
        else
        {
            audioSource.clip = bgmClips[1]; // 通常ステージのBGM
        }

        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void SelectedStage()//ステージセレクトでステージが決まったら行う処理
    {
        audioSource.Stop();
    }

    void StopBGM()
    {
        audioSource.Stop();
    }

    public static string GetSceneName()
    {
        return sceneName;
    }
}
