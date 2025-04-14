using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player : MonoBehaviour
{
    /****************コンポーネントの取得***************************/
    private Rigidbody2D rb; // プレイヤーのRigidbody2Dコンポーネント.
    BossStage bossstage;
    public PlayerBodyHantei playerbodyhantei;//パブリックじゃなくてよくね？
    /***************************************************************/

    /****************スピード、ジャンプ力等の値*********************/
    private float moveSpeed = 5.0f; // プレイヤーの移動速度.
    private float jumpForce = 7.0f; // ジャンプの力.



    public float GravityPower = 5.0f;//重力の強さ
    /***************************************************************/

    /********************入力関連************************************/
    private int MoveX = 0;//今右に進んでいるのかの確認
    private float moveInput = 0; //左右入力の取得
    private float moveInputUP = 0; //上下入力の取得
    private bool AdvanceJanpflg = false;//先行入力用
    public float coyotetime = 0.1f;//先行入力を受け付ける時間



    private bool Janpflg = true;//falseならジャンプできる
    private bool dontMove = false;//trueならプレイヤーの操作を受け付けないようにする
    /***************************************************************/

    /****************ギミックの数値を保存する変数*******************/
    public int SetStartGravitycnt = 0;//スタート時から重力反転をストックできるようにする変数.



    public int Gravitycnt = 0;//何回重力反転できるかのカウント.
    public int Keyflg = 0;//何個カギを持っているかのフラグ(複数持っていても描画は一つのみ).
    /***************************************************************/


    /************************ボス関連********************************/
    public bool Bossflg = false;//trueならボス戦
    public bool BossGravitySousaflg = false;//trueならボスが重力を操作する
    public bool UDflg = false;//重力の上下のフラグtrueが上
    public bool LRflg = false;//重力の左右のフラグtrueが左
    public bool ULflg = true;//重力の縦か横かのフラグtrueが縦
    int MoveY1 = 0;//今上に進んでいるかの確認
    int MoveY2 = 0;//今上に進んでいるかの確認
    /***************************************************************/



    //dontMoveいらなくね？

    //USEチェックポイント関連が汚い



    private int GoolAnimeCnt = 0;//いらないかも、改良の余地あり




    // 中間地点の静的変数(これは必要なstatic)
    public static Vector3 CheckpointPosition = Vector2.zero;
    private static int ChackPointNum = 0;//チェックポイントが複数あり、2番目の中間→1番目の中間に触れても座標を更新しないようにするために使用
    public bool USECheckPoint = false;

    [SerializeField] private AudioSource Sound;//AudioSourceを入れるための箱
    [SerializeField] private AudioClip Janp;//ジャンプの音
    [SerializeField] private AudioClip Miss;//敵に触れてミスした時の音の音
    [SerializeField] private AudioClip CGravity;//重力を反転した時の音の音
    [SerializeField] GameObject BossHoudai;//ボス戦の砲台
    [SerializeField] GameObject BossStageobj;//ボス前のステージ管理
    [SerializeField] GameObject GravityEfe_prefab;//プレファブ化した重力反転素材の用意  
    [SerializeField] GameObject JanpEfe_prefab;//プレファブ化したジャンプエフェクトの用意  

    //ここかんすうでいじれ
    //プレイヤーの状態をがどれか判別する用(ですフラグ等と合併したい)
    private bool isGround = true;//外部スクリプトから変更されるため、これだけセッター関数アリ
    private bool isJump;
    private bool isMove;
    private bool isDatch = false;
    private bool isGoal = false;

    // enum型の定義
    public enum PLAYERMODE
    {
        STAY,
        WALK,
        JUMP,
        DEATH,
        GOAL,
    };
    PLAYERMODE PMode;

    void Start()
    {
       
       rb = GetComponent<Rigidbody2D>();

        if (SceneManager.GetActiveScene().name == "BossScene")
        {
            bossstage = BossStageobj.GetComponent<BossStage>();
        }
        if (USECheckPoint == true)
        {
            if (CheckpointPosition != Vector3.zero)
            {
                transform.position = new Vector3(CheckpointPosition.x, CheckpointPosition.y, 0);
            }
        }
        if (USECheckPoint == true)
        {
            GetCheckPoint(ChackPointNum); // チェックポイントの座標を保存
        }
        //スタチックのリセット
        dontMove = false;

    }

    //このスクリプトを参照しやすくする
    public static Player Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // 複数生成を防止
    }

    public void HandleGoal()
    {
        if (isGoal == true)//350
        {
            // 中間地点のリセット
            CheckpointPosition = Vector3.zero;

            // アニメーションカウンターを増加
            GoolAnimeCnt += 1;
            if (GoolAnimeCnt % 100 == 0 || GoolAnimeCnt == 1)
            {

                GetComponent<Rigidbody2D>().velocity =
                    transform.TransformDirection(Vector3.up) * 5.0f;
            }
        }
    }





    void Update()
    {
        if (Time.timeScale == 1)//ポーズを開いていないなら
        {
            HandleGoal();//ゴールアニメ(ここにいるの汚い)
            SendPlayerMode();//自身の状態に応じてキャラの当たり判定を変更する関数(PlayerBodeHanteiに続く)
        
            //↓Start関数においていいかも。コントローラーがないから確認できません
            GetGamePad();//ゲームパットを使えるようにする&左スティックの入力を取得
            GetPlayerData();//ジャンプしていいか、どちらの方向を向いているかの判定.
            AddGravity();//向きに応じた重力を加える.
            PlayerMove();//プレイヤーの移動、ジャンプをここで受け付ける.
            SetAnimatorflg();//アニメーション用のフラグをセット.
        }
    }





    void SetAnimatorflg()//アニメーション用のフラグをセット.
    {
        GetComponent<Animator>().SetBool("isJump", isJump);
        GetComponent<Animator>().SetBool("isMove", isMove);
        GetComponent<Animator>().SetBool("isGround", isGround);
        GetComponent<Animator>().SetBool("isDatch", isDatch);
        GetComponent<Animator>().SetBool("isGoal", isGoal);
    }


    void OnCollisionEnter2D(Collision2D c)//イズトリガーがついていないオブジェクトに触れたとき
    {

        if (c.gameObject.tag == ("Miss"))
        {
            //即ステージの再読み込み
            RisetPlayer();
        }
        else if (c.gameObject.tag == ("BossBotton"))
        {
            BossHoudai.GetComponent<BossHoudai>().HoudaiHassya();
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == ("Miss"))
        {
            //即ステージの再読み込み
            RisetPlayer();
        }

        if (c.gameObject.tag == ("flg1"))
        {
            BossGravitySousaflg = true;
            bossstage.BossStageGimmick1flg = true;
        }
        else if (c.gameObject.tag == ("flg2"))
        {
            BossGravitySousaflg = true;
            bossstage.cnt = 0;
            bossstage.BossStageGimmick2flg = true;
            bossstage.GravityMukiflg = true;
        }
        else if (c.gameObject.tag == ("flg3"))
        {
            BossGravitySousaflg = false;
            bossstage.BossStageGimmick2flg = false;
            bossstage.BossStageGimmick1flg = false;
        }
        else if (c.gameObject.tag == ("CheckPoint"))
        {
            if (USECheckPoint == true)
            {
                // 相手のオブジェクトに ExampleScript がアタッチされているか確認
                CheckPointflag exampleScript = c.GetComponent<CheckPointflag>();
                if (exampleScript != null)
                {
                    // 変数の値を取得
                    int Number = exampleScript.FlagNumber;
                    // Debug.Log("触れ旗は:" + Number);
                    GetCheckPoint(Number);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == ("BossBottonRem"))
        {
            BossHoudai.GetComponent<BossHoudai>().SwitchReSpawn();
        }
    }


    void GetGamePad()
    {
        // ゲームパッド（デバイス取得）
        var gamepad = Gamepad.current;
        if (gamepad == null) return;

        // ゲームパッドの左右のスティックの入力値を取得
        // 得られる結果はVector2型
        var leftStick = gamepad.leftStick.ReadValue();
    }

    void GetPlayerData()//プレイヤーがジャンプしていいか、どちらの方向を向いているかなどのデータの取得
    {
        var velocityY = rb.velocity.y;//Y方向の速度の取得
        var velocityX = rb.velocity.x;//X方向の速度の取得
        if (ULflg == true)
        {
            if (velocityY > 0.75 || velocityY < -0.75)//Yの速度が速ければジャンプできないようにする
            {
                //コヨーテタイム導入の関係で隠してます。不都合あればコメント化を解除してください
               //  Janpflg = true;
            }
        }
        else if (ULflg == false)
        {
            if (velocityX > 0.75 || velocityX < -0.75)//Xの速度が速ければジャンプできないようにする
            {
              //  Janpflg = true;
            }
        }


        //プレイヤーが待機状態でなければ
        if (rb.velocity.x != 0 && dontMove == false)//向きを0より大きいか小さいかに変更したので敏感に→敵に触れた後は向きが変わらないようにする
        {
            //スティックの押し込み具合で向きを変更
            if (moveInput < -0.3f)
            {
                MoveX = 180;
            }
            if (moveInput > 0.3f)
            {
                MoveX = 0;
            }


        }
        if (rb.velocity.y != 0 && dontMove == false)
        {
            // プレイヤーが下に移動している場合
            if (rb.velocity.y < -0.3)
            {
                MoveY1 = 0;
                MoveY2 = 180;
            }
            // プレイヤーが上に移動している場合
            if (rb.velocity.y > 0.3)
            {
                MoveY1 = 180;
                MoveY2 = 0;
            }
        }
    }

    void PlayerMove()//プレイヤーの左右移動、ジャンプ
    {
        //動いていいかのフラグがfalseなら
        if (dontMove == false)
        {


            //ジャンプ.
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick 1 button 0"))
            {
                if (Janpflg == false)//地上にいてジャンプできるなら.
                {
                    if (dontMove == false)//うごける状態なら.
                    {
                        Janpflg = true;//フラグをジャンプ中にする.
                        AddJanp();//実際にジャンプする.
                    }
                }
                else//地上にいないなら先行入力用フラグをtrueにする(足元の判定が床に触れたとき、このフラグが真ならジャンプするよう別箇所で記載).
                {
                    AdvanceJanpflg = true;//このフラグがtrue中に地上に着地するとジャンプに移行できる.
                    StartCoroutine(AdvanceWaitTime());//数フレーム後にフラグをfalseに変更.
                }
            }



            if(BossGravitySousaflg == false)
            {
                //ボタンでの重力反転
                if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick 1 button 1"))//またはBボタン
                {
                    if (Gravitycnt > 0)//アイテムを持っていたら
                    {
                        Gravitycnt -= 1;
                        ChangeGravity();//重力のオンオフ切り替え
                    }
                    else if (Bossflg == true)//フラグが真なら無条件で切り替え
                    {
                        ChangeGravity();
                    }
                }
            }

            // 左右の入力を取得、移動
            moveInput = Input.GetAxisRaw("Horizontal");
            if (ULflg == true)
            {
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
                if (moveInput != 0)
                {
                    isMove = true;
                }
                else
                {
                    isMove = false;
                }
            }
            else
            {
                // 上下の入力を取得

                moveInputUP = Input.GetAxisRaw("Vertical");
                rb.velocity = new Vector2(rb.velocity.x, moveInputUP * moveSpeed);
                if (moveInputUP != 0)
                {
                    isMove = true;
                }
                else
                {
                    isMove = false;
                }              
            }
        }
    }

    void ChangeGravity()//重力のオンオフ切り替え
    {
        //各種エフェクト、SE
        Instantiate(GravityEfe_prefab,
      transform.position, transform.rotation);

        Sound.PlayOneShot(CGravity);

        UDflg = !UDflg;
        ULflg = true;


    }

    void AddGravity()//実際に重力を加える
    {
        if (ULflg == true)
        {
            if (UDflg == true)
            {
                Vector2 myGravity = new Vector2(0, GravityPower);
                rb.AddForce(myGravity);
                transform.rotation = Quaternion.Euler(180, MoveX, 0);
            }
            else
            {
                Vector2 myGravity = new Vector2(0, -GravityPower);
                rb.AddForce(myGravity);
                transform.rotation = Quaternion.Euler(0, MoveX, 0);
            }
        }
        else
        {

            if (LRflg == true)//
            {
                Vector2 myGravity = new Vector2(-GravityPower, 0);
                rb.AddForce(myGravity);
                transform.rotation = Quaternion.Euler(MoveY1, 0, 270);
            }
            else
            {

                Vector2 myGravity = new Vector2(GravityPower, 0);
                rb.AddForce(myGravity);
                transform.rotation = Quaternion.Euler(MoveY2, 0, 90);
            }
        }
    }

    void GameOver()//1秒後にシーンのリロードをする
    {
        if (isDatch == false)
        {
            Sound.PlayOneShot(Miss);
            Invoke(nameof(RisetPlayer), 1.0f);//1秒後にシーンをリセットする関数を呼ぶ
            dontMove = true;//trueならプレイヤーが動けなくなる
            isDatch = true;
        }
    }

    void GollAnime()//ゴールフラグを真にし、Updateのほうにて動作を記述
    {
        isGoal = true;
        dontMove = true;
        resetCheckPoint();//中間に使用されるstaticへんすうのりせっと
        GetComponent<Rigidbody2D>().velocity =
           new Vector2(0, 0);
    }

    public void BossStart()//ボス戦開始時に呼ばれる
    {
        Bossflg = true;
    }

    void RisetPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GetCheckPoint(int Number)//チェックポイントに触れたら座標を取得する
    {
        if (ChackPointNum < Number || ChackPointNum == 0)
        {
            CheckpointPosition = this.transform.position;
            ChackPointNum = Number;//触れた旗の記録を更新       
        }
    }

    void DrawGravityEfe()//反転時に残像の描画
    {
        Sound.PlayOneShot(CGravity);
        Instantiate(GravityEfe_prefab,
      transform.position, transform.rotation);
    }

    void SendPlayerMode()//プレイヤーの状態保存したEnum型をもとに状態を判定の方に送信
    {
        //ここでisGroundを更新
        if (Janpflg == true)
        {
            isGround = false;
        }
        else
        {
            isGround = true;
        }

        //キャラの状態に合わせてあたり判定を変えようとした名残
        switch (PMode)
        {

            case PLAYERMODE.STAY:
                playerbodyhantei.ChangeBodyHaneti(1);
                break;

            case PLAYERMODE.WALK:
                playerbodyhantei.ChangeBodyHaneti(2);
                break;

            case PLAYERMODE.JUMP:
                playerbodyhantei.ChangeBodyHaneti(3);
                break;

            case PLAYERMODE.DEATH:
                playerbodyhantei.ChangeBodyHaneti(4);
                break;
            case PLAYERMODE.GOAL:
                playerbodyhantei.ChangeBodyHaneti(5);
                break;

        }
        if (isGoal == true)
        {
            PMode = PLAYERMODE.GOAL;
        }
        else
        {
            if (isMove == true)
            {
                PMode = PLAYERMODE.WALK;
            }
            else
            {
                PMode = PLAYERMODE.STAY;
            }
            if (isGround == false)
            {
                PMode = PLAYERMODE.JUMP;
            }

            if (isDatch == true)
            {
                PMode = PLAYERMODE.DEATH;
                GetComponent<Rigidbody2D>().velocity =
               new Vector2(0, 0);
            }
        }
    }


    IEnumerator AdvanceWaitTime()//ボタンを押されてからcoyotetime待ってからジャンプフラグを偽にする、先行入力用.
    {
        yield return new WaitForSeconds(coyotetime);
        //上記のタイミングで少し待つ
        AdvanceJanpflg = false;

    }

    //ここから外部から呼び出す関数
    public void Stage17Gravity(int Gravity)
    {
        GravityPower = Gravity;
    }

    public void changeJanpFlg(bool flg)
    {
        Janpflg = flg;
    }

    public int getGravityCnt()
    {
        return Gravitycnt;
    }

    public void addGravityCnt(int num)
    {
        Gravitycnt += num;
    }

    public void setKeyFlg(int num)
    {
        Keyflg += num;
    }

    public int getKeyFlg()
    {
        return Keyflg;
    }

    public bool ChackCanMove()
    {
        return !dontMove;
    }

    public void resetCheckPoint()
    {
       
        CheckpointPosition = Vector2.zero;
        ChackPointNum = 0;
       
    }

    public void ChangeGravityBoss(bool checkGrvflg, bool changeGrvflg)
    {
        if (checkGrvflg == true)
        {
            ULflg = checkGrvflg;
            UDflg = changeGrvflg;
        }
        else
        {
            ULflg = checkGrvflg;
            LRflg = changeGrvflg;
        }
    }

 
    public bool getAdvanceJanpflg()
    {
        return AdvanceJanpflg;
    }

    public void AddJanp()
    {
        //重力が横向きなら(effectを出しつつ、所定の方向の速度を0に)
        if (ULflg == false)
        {
            rb.velocity = new Vector2(0f,rb.velocity.y);
            if (LRflg == true)
            {
                Instantiate(JanpEfe_prefab,
                      new Vector3(transform.position.x - 0.7f, transform.position.y - 0.0f), transform.rotation);
            }
            else
            {
                Instantiate(JanpEfe_prefab,
                    new Vector3(transform.position.x + 0.7f, transform.position.y - 0.0f), transform.rotation);
            }
        }

        //それ以外(通常ステージの重力の向きがどっちか)
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            if (UDflg == true)
            {
                Instantiate(JanpEfe_prefab,
                       new Vector3(transform.position.x, transform.position.y + 0.7f, 0f), transform.rotation);
            }
            else
            {
                Instantiate(JanpEfe_prefab,
                     new Vector3(transform.position.x, transform.position.y - 0.7f, 0f), transform.rotation);
            }
        }

        Sound.PlayOneShot(Janp);//ジャンプのSE
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);//実際に力を加える
    }

}





