using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f;     // 自動落下検知距離
    public bool isDelete = false;   // 落下後に削除するフラグ
    GameObject deadObj;             // 死亡当たり（起動前は死亡しない）
    bool isFell = false;            // 落下フラグ（落下完了＝地面で消滅フラグ）
    float fadeTime = 0.5f;          // フェードアウト時間

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidbody2Dの物理挙動を停止
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;            
        //static にすることで Rigidbody2Dを止めている＝重力オフ

        // 開始時は死亡しないように隠す
        deadObj = transform.Find("DeadObject").gameObject;  //死亡あたり取得
        // Find 探す deadObjの中から、"DeadObject"という名前のついたObjectを探し、
        // tranceformコンポーネント情報の、gameObject

        deadObj.SetActive(false);                           //死亡あたりを非表示
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // プレイヤーを探す
        //わざわざ毎フレーム探しているので、工夫して軽くできるよ

        if (player != null)
        {
            // プレイヤーとの距離計測 Distance
            float d = Vector2.Distance(transform.position, player.transform.position);

            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static)   //staticならば
                {
                    // Rigidbody2Dの物理挙動を開始
                    rbody.bodyType = RigidbodyType2D.Dynamic;   //Dynamicに切り替える
                    deadObj.SetActive(true);    //死亡あたりを表示
                }
            }
        }
        if (isFell) //消滅開始がオンならば
        {
            // 落下した
            // 透明値を変更してフェードアウトさせる
            fadeTime -= Time.deltaTime; // 前フレームの差分秒マイナス
            Color col = GetComponent<SpriteRenderer>().color;   // カラーを取り出す　Color型に入れる
            col.a = fadeTime;   // 透明値(A)を変更　float 1＝不透明、0＝完全透明
            GetComponent<SpriteRenderer>().color = col; // カラーを再設定する
            if (fadeTime <= 0.0f)
            {
                // 0以下(透明)になったらgameObjectをヒエラルキー上から消す
                Destroy(gameObject);    //this.gameObject
            }
        }
    }

    // 接触開始
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete)
        {
            isFell = true; // 落下フラグオン（＝消滅開始フラグ）
        }
    }
    //範囲表示
    void OnDrawGizmosSelected() // Scene の情報が何か変更されると呼び出される
    {
        // Gizmos シーンビューに何かを書き込む
        Gizmos.DrawWireSphere(transform.position, length);
    }
}
