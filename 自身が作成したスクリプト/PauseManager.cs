using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PauseManager : MonoBehaviour
{
    private bool Pauseflg = false;
    private bool FastSetflg = false;// 関数の初回起動時にボタン1を選択するようにするためのフラグ

    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;

    private Transform square;

    private Player playerscript; // プレイヤースクリプトの参照を保持

    // Start is called before the first frame update
    void Start()
    {
        playerscript = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Escape))

        {
            if (playerscript.ChackCanMove())
            {
                if (Pauseflg == false)
                {
                    Pauseflg = true;
                    FastSetflg = false;
                }
                else
                {
                    Pauseflg = false;
                }
            }
        }
        if (Pauseflg == true)
        {
            PauseGame();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);//選択カーソルの解除(これがないと画面からボタンが消えても選択できてしまう)
            ResumeGame();
        }

    }


    public void PauseGame()//ゲームを止めてボタンUIの表示
    {
        Time.timeScale = 0;
        GetComponent<Canvas>().enabled = true;
        if (FastSetflg == false)
        {
            button1.Select();
            FastSetflg = true;
        }


    }

    public void ResumeGame()//ゲームを動かしてボタンUIを隠す
    {
        GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
    }

    public void PauseExit()
    {
        Pauseflg = false;
    }

    public void RistartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void ReturnStageSelect()
    {
        playerscript.resetCheckPoint();
        SceneManager.LoadScene("StageSelect");
        if (GameManager.GetSceneName() == "StageSelect")
        {
            Debug.Log("1");
            SceneManager.LoadScene("StageSelect");
        }
        else if (GameManager.GetSceneName() == "StageSelect2")
        {
            Debug.Log("2");
            SceneManager.LoadScene("StageSelect2");
        }
    }
    public void ReturnTitle()
    {
        SceneManager.LoadScene("taitoru");

    }
}
