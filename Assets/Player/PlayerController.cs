using UnityEngine;
using static UnityEngine.Rendering.BoolParameter;

public enum GameState           // ゲームの状態：自作の型　GameStateという型
                                // enum 列挙型
{
    InGame,                     // ゲーム中
    GameClear,                  // ゲームクリア
    GameOver,                   // ゲームオーバー
    GameEnd,                    // ゲーム終了
}

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;  // Rigidbody2D 型の変数名 rbody。Rigidbodyをこのスクリプトで使用するため
    float axisH = 0.0f;             // 入力
    public float speed = 3.0f;      // 移動速度   
    public float jump = 9.0f;       //ジャンプ力
    public LayerMask groundLayer;   //着地できるれイヤー
    bool goJump = false;              //ジャンプ開始フラグ
    bool onGround = false;            //地面フラグ

    public float dashSpeed = 12.0f; //勝手に追加したダッシュの速度


    //アニメーション対応
    Animator animator;
    public string stopAnime = "PlayerStop"; //右側はクリップ名
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    // ゲームの状態（テキストは誤植なのでここは次の記述が正解）
    public static GameState gameState = GameState.InGame;

    //カメラ用（スクロール）
    public float camLeft = 0.0f;
    public float camRight = 0.0f;
    public float camTop = 0.0f;
    public float camBottom = 0.0f;

    public GameObject subScreen;

    //強制スクロール
    public bool isForceScrollx = false;
    public float forceScrollSpeedx = 0.5f;
    public bool isForceScrolly = false;
    public float forceScrollSpeedy = 0.5f;


    //スコア
    public int score = 0;

    void Start()
    {

        // Application.targetFrameRate = 15;   // 低FPS 固定で消費電力を抑える

        rbody = this.GetComponent<Rigidbody2D>();
        // Rigidbodyを取ってくる。このスクリプトでRigidbodyコンポーネントに干渉するため

        animator = this.GetComponent<Animator>();
        //Animatorを取ってくる。このスクリプトでAnimatorコンポーネントに干渉するため

        nowAnime = stopAnime;   // 初期状態、停止から開始する。今、なんのアニメ再生中かを保存する
        oldAnime = stopAnime;   // 初期状態、停止から開始する

        gameState = GameState.InGame;
        //        PlayerController.gameState = GameState.InGame;


    }

    void Update()
    {
        if (gameState != GameState.InGame ) //インゲームでないなら
        {
            return; 
            //このフレームをキャンセルしてreturn(抜ける)
            // ＝Updateの一番上に書いてあるので、ここから下は実行されない
        }

        //地上判定
        onGround = Physics2D.CircleCast(
            transform.position, //発射位置
            0.2f,               //円の半径
            Vector2.down,       //発射方向
            0.0f,               //発射距離
            groundLayer);       //検出するレイヤー
        if (Input.GetButton("Jump") || Input.GetButtonDown("Fire1"))    //ジャンプさせる
                                                                        //勝手にGetButtonDownやめた
        {
            goJump = true;  //ジャンプフラグ
        }

        axisH = Input.GetAxisRaw("Horizontal");     //水平方向の入力をチェックする


        if (axisH > 0.0f)                           // 向きの調整
        {
            transform.localScale = new Vector2(1, 1);   // 右移動
            // Debug.Log("みぎ押された");

        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }

        // 勝手に追加したダッシュ機能
        if (Input.GetButton("Fire3"))   //Swith B=Fire1, A=Fire2, Y=Fire3, X=なし,
        {
            speed = dashSpeed;
            // Debug.Log("ダッシュボタン押された");
        }
        else
        {
            speed = 3f;
        }

        //アニメーション更新
        if (onGround)   //地上なら
        {
            if (axisH == 0.0f) //0.0f 左右ボタンが押されていない＝移動してないなら
            {
                nowAnime = stopAnime;   //停止アニメ
            }
            else
            {
                nowAnime = moveAnime;   //移動アニメ
            }
        }
        else //地上ではない＝空中
        {
            nowAnime = jumpAnime;   //空中ならジャンプアニメ
        }

        if (nowAnime != oldAnime)   //再生中アニメと、直前(1フレーム前)アニメが異なるなら
        {
            oldAnime = nowAnime;        //直前アニメに今アニメ（これから再生するアニメ）をセットし、
            animator.Play(nowAnime);    //新しいアニメ(nowAnime)を再生開始する
            //PlayerController.cs（これ）から、animatorに干渉してアニメ再生させている
        }


        //ここからカメラ制御
        float x;
        float y;

        if (isForceScrollx) //横の強制スクロールがある場合
        {
            //xを時間に合わせて変化
            x = Camera.main.transform.position.x + (forceScrollSpeedx * Time.deltaTime);
        }
        else //そうじゃない場合（強制スクロールなし）
        {
            x = Mathf.Clamp(transform.position.x, camLeft, camRight);
        }
        if (isForceScrolly) //縦の強制スクロールがある場合
        {
            //yを時間に合わせて変化
            y = Camera.main.transform.position.y + (forceScrollSpeedy * Time.deltaTime);
        }
        else //そうじゃない場合（強制スクロールなし）
        {
            // y = -transform.position.y;   //自分と逆向きに
            y = Mathf.Clamp(transform.position.y, camBottom, camTop);
        }


        /* //強制スクロール非対応のみの書き方
        // transform.positionは、プレイヤーの座標（かならずVector3型 x,y,z）
        // x = transform.position.x; ←こう書くと、必ず自分が中心スクロール
        // Clampで、カメラ移動の制限（いわゆるマップ端）を設けている
        x = Mathf.Clamp(transform.position.x, camLeft, camRight);
        y = Mathf.Clamp(transform.position.y, camBottom, camTop);
        y = transform.position.y+1;   //わざと自分中心にした
        */
        Vector3 camPos =new Vector3(x, y, -10);
        Camera.main.transform.position = camPos;

        // 上記をすべて次の1行で書くこともできるが、ちょっとわかりにくい
        // Camera.main.transform.position = new Vector3(Mathf.Clamp(transform.position.x, camLeft, camRight), Mathf.Clamp(transform.position.y, camBottom, camTop), -10);


        // 二重スクロール
        if (subScreen != null)  
            //サブスクリーンがnullではない＝何か入っている場合
            //多重スクロールがないステージもある
        {
            y= subScreen.transform.position.y;
            // Vector3 subpos = new Vector3(x / 2.0f, y, subScreen.transform.position.z);
            //subpos,サブスクリーンの場所、xはプレイヤー位置の半分、Yはそのまま、zはサブスクリーンがもともと持っている座標そのままにする

            // y = -transform.position.y;   //自分と逆向きに
            Vector3 subpos = new Vector3(x / 2.0f, y, subScreen.transform.position.z); //勝手にy

            subScreen.transform.position = subpos;
        }

    }

    void FixedUpdate()
    {
        if (gameState != GameState.InGame)
        {
            return;
        }


        if (onGround || axisH != 0)   //地面の上 or 速度が0ではない
        {
            // 速度を更新して rbody(Rigidbody) に反映
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }


        if (onGround && goJump)     //地面の上でジャンプキーが押された
        {
            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);          //ベクトル(2D)
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    //瞬間的な力を加える
            goJump = false;                                 //ジャンプフラグ下ろす
        }

    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D collision) //イベント発生
    {
        if (collision.gameObject.tag == "Goal")
        // collision.gameObject.tag　ぶつかった(collision)相手のコライダーの、
        // 宿主のオブジェクト(gameObject)が持っているタグ(== "Goal")
        {
            Goal();         // ゴール！！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     // ゲームオーバー
        }
        else if(collision.gameObject.tag == "ScoreItem")    //Itemとった
        {
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();
            score = item.itemdata.value;    //value得点

            //collision ぶつかった gameObjectごと Destroy。（Objectを含めないとcollisionのみDestroy）
            Destroy(collision.gameObject);

            // Debug.Log("Get Score: " + score);
        }
    }

    // ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = GameState.GameClear;
        GameStop();
    }

    // ゲームオーバー
    public void GameOver()
    {

        // Debug.Log("GameOver 呼ばれた"); //tesuto 

        animator.Play(deadAnime);
        gameState = GameState.GameOver;
        GameStop();

        //ゲームオーバ
        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        Destroy(gameObject, 1.0f);  //追加、１秒後に消す
        // enabled = false;    //余計なもの追加してみた

    }

    // ゲーム停止
    void GameStop()
    {
        rbody.linearVelocity = new Vector2(0, 0);
    }
}
