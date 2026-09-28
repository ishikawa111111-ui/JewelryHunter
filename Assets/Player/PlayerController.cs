using UnityEngine;

public enum GameState           // ゲームの状態
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
    bool goJump=false;              //ジャンプ開始フラグ
    bool onGround=false;            //地面フラグ

    public float dashSpeed = 12.0f; //勝手に追加したダッシュの速度


    //アニメーション対応
    Animator animator;


    // ゲームの状態（テキストは誤植なのでここは次の記述が正解）
    public static GameState gameState = GameState.InGame;

    void Start()
    {

        Application.targetFrameRate = 15;   // 低FPS 固定で消費電力を抑える

        rbody = this.GetComponent<Rigidbody2D>();   // Rigidbodyをこのスクリプトで使用するため


    }

    void Update()
    {
        //地上判定
        onGround = Physics2D.CircleCast(
            transform.position, //発射位置
            0.2f,               //円の半径
            Vector2.down,       //発射方向
            0.0f,               //発射距離
            groundLayer);       //検出するレイヤー
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))    //ジャンプさせる
        {
            goJump = true;  //ジャンプフラグ
        }

        axisH = Input.GetAxisRaw("Horizontal");     //水平方向の入力をチェックする


        if (axisH > 0.0f)                           // 向きの調整
        {
            transform.localScale = new Vector2(1, 1);   // 右移動
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

    }

    void FixedUpdate()
    {
        if(onGround || axisH !=0)   //地面の上 or 速度が0ではない
        {
            // 速度を更新して rbody(Rigidbody) に反映
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }


        if (onGround && goJump)     //地面の上でジャンプキーが押された
        {
            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);          //ベクトル
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    //瞬間的な力を加える
            goJump = false;                                 //ジャンプフラグ下ろす
        }

    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();         // ゴール！！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     // ゲームオーバー
        }
    }
    // ゴール
    public void Goal()
    {
        
    }
    // ゲームオーバー
    public void GameOver()
    {
        
        gameState = GameState.GameOver;
    }

    // ゲーム停止
    void GameStop()
    {

    }
}
