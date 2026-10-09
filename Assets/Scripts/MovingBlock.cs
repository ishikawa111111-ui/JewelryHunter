using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;          //X移動距離
    public float moveY = 2.0f;          //Y移動距離
    public float times = 3.0f;          //時間
    public float wait = 0.0f;           //移動後の停止時間
    public bool isMoveWhenOn = false;   //bool型。乗った時に動き始めるフラグ（インスペクタでチェックON）OFFなら勝手に動く
    public bool isCanMove = true;       //動くフラグ。動くか、動かないか。
    Vector3 startPos;                   //初期位置
    Vector3 endPos;                     //移動位置
    bool isReverse = false;             //反転フラグ
    float movep = 0;                    //移動補完値

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;                                 //初期位置
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY);  //移動位置

        if (isMoveWhenOn) 
        {
            isCanMove = false;  //乗ったときに動くので最初は動かない
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isCanMove)
        {
            float distance =Vector2.Distance(startPos, endPos); //移動距離
            float ds= distance / times;                         //1秒の移動距離   距離を時間で割っている５ｍを３秒で行く、１秒１．６６ｍみたいな
            float df = ds * Time.deltaTime;                     //1フレームの移動距離　例１フレーム0.01秒なら、dsxフレーム(0.01)
            movep += df / distance;                             //移動補完値 全体の何％進むのか足す（蓄積）やがて1.0(=100%)にたどり着く
            if (isReverse)
            {
                transform.position = Vector2.Lerp(endPos, startPos, movep); //逆移動　らーぷ補完関数（スタート、ゴール、進捗率（0～1＝100%））
            }
            else
            {
                transform.position = Vector2.Lerp(startPos, endPos, movep); //正移動
            }
            if (movep >= 1.0f)  //たどり着いた（１００％の位置）↓逆送の準備
            {
                movep = 0.0f;               //移動補完リセット
                isReverse = !isReverse;     //フラグの反転
                isCanMove= false;           //一旦今の移動はストップ（waitあるかもしれないから）
                if (isMoveWhenOn == false)
                {
                    Invoke("Move", wait);   //Ｉｎｖｏｋｅで、処理を待ってからMove()メソッドを実行する

                }
            }

        }
    }

    //移動フラグを立てる
    public void Move()  //移動はじめていいよ
    {
        isCanMove= true;
    }

    //移動フラグを下ろす
    public void Stop()  //強制的に移動を止める
    {
        isCanMove = false;

    }

    //接触開始
    void OnCollisionEnter2D(Collision2D collision)  //プレイヤーが接触したとき、プレイヤーを子オブジェクトにする
        //すり抜け物体(IsTriggerがぶつかったときは、OnTriggerEnter2D、Collider2Dを取得)
        //今回は PlayerにもBlockにもIsTriggerじゃないので、Collision2Dを取得。
    {
        //テキストをみて完成させましょう
        if (collision.gameObject.tag == "Player")   //Playerタグを持っている物体が接触したら
        {
            collision.transform.SetParent(transform);   //親子にしてプレイヤーが一緒に動くようにする
            //接触した相手(collision.transform)=Playerの親は自分(SetParent(transform))=MovingBlockだと宣言
            if (isMoveWhenOn)   //もしisMoveWhenOnがtrueなら、
            {
                isCanMove=true; //ブロック動いてOK
            }
        }
    }

    //接触終了
    void OnCollisionExit2D(Collision2D collision)   //ExitはCollisionが離れたときの処理
    {
        if(collision.gameObject.tag =="Player")     //Playerが親子関係なら、
        {
            collision.transform.SetParent(null);    //親子関係何もなしnullにする
        }
    }

    //移動範囲表示
    void OnDrawGizmosSelected()     //Editor上でのギズモ表示
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //移動線
        //Gizmos.DrawLine(fromPos, new Vector2(fromPos.x + moveX, fromPos.y + moveY));
        Gizmos.DrawLine(startPos, endPos);

        //スプライトのサイズ
        Vector2 size = GetComponent<SpriteRenderer>().size;

        //初期位置の四角
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));

        //移動位置（ゴールの四角）
        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
