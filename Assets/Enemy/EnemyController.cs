using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          // 移動速度
    public bool isToRight = false;      // true=右向き　false=左向き
    public float revTime = 0;           // 反転するまでの時間
    public LayerMask groundLayer;       // 地面レイヤー
    bool onGround = false;              // 地面フラグ
    float time = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isToRight)  // 向きの変更チェック。インスペクタでONなら右向きで開始する（元が左向きの絵なので）
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 地上判定（プレイヤーと同じ）
        onGround = Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        groundLayer);          // 検出するレイヤー（地面レイヤー）
        if (revTime > 0)    //反転時間以上なら、
        {
            //テキストをみて完成させましょう
            time += Time.deltaTime;
            if (time >= revTime)
            {
                isToRight = !isToRight; //右向きフラグを逆にする！マークで逆にしている
                time = 0;               //反転時間をリセット
                if (isToRight)
                {
                    transform.localScale = new Vector2(-1, 1);  //右向きなら左向きの絵に変える
                }
                else
                {
                    transform.localScale = new Vector2(1, 1);  //左向きなら→向きの絵に変える
                }
            }
        }
    }

    void FixedUpdate()
            {
                if (onGround)
                {
                    //テキストをみて完成させましょう
                    Rigidbody2D rbody = GetComponent<Rigidbody2D>();    //グローバル変数にしてstart()に書いてもOK
                    if (isToRight)
                    {
                        rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);  //右へ移動
                    }
                    else
                    {
                        rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y); //左へ移動
                    }
                }
            }


    // 接触
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //テキストをみて完成させましょう
        isToRight = !isToRight;
        time = 0;
        if(isToRight)
        {
            transform.localScale=new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }
    }
}
