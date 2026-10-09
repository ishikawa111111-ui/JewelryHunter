using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;            //発生させるPrefabデータ    弾を指定できる
    public float delayTime = 3.0f;          //遅延時間  発射間隔
    public float fireSpeed = 4.0f;          //発射速度　弾の速度
    public float length = 8.0f;             //範囲    プレイヤーが8mに入ったら発射する

    GameObject player;                      //プレイヤーとの距離を測るために使用
    Transform gateTransform;                //発射口のTransform　発射口の位置を取得するため
    float passedTimes = 0;                  //経過時間（発射間隔）チェック用。チェックだけなのでpublicではない

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //発射口オブジェクトのTransformを取得　子要素であるgateを認識
        gateTransform = transform.Find("gate"); 
        //Findは、自分の子要素から名称のオブジェクトを探してくる(gateという名称)
        //cannonのTransformコンポーネントの子"gate"を探し、そのgateのtransfoam情報を取得

        //プレイヤーを取得  Playerタグがついているオブジェクトを取得：位置を参照できるように
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //待機時間加算
        passedTimes += Time.deltaTime;  //deltaTimeで1フレームにかかる時間を積み上げ＝経過時間

        //Playerとの距離チェック
        if (CheckLength(player.transform.position)) //自作メソッド　戻り値はtrue / false。もし
        {
            //待機時間経過　delayTime(3秒)
            if (passedTimes > delayTime)
            {
                //テキストをみて完成させましょう
                passedTimes = 0;    //カウントしなおし

                //弾(Shell)をプレハブから作る
                Vector2 pos = new Vector2(gateTransform.position.x, gateTransform.position.y);    
                //gateの位置をposに入れている。gateTransform.positionと同じだが、Vector3なのでVector2にしている(でもVector3でも問題ない)

                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);    //Instantiateいんすたんてーと　prehubから弾を生成
                //引数（対象オブジェクトobjPrehub(プレハブのshellをアタッチすれば、毎回違うオブジェクトとしてプレハブから生成される),
                //  ,生成位置pos(gateの位置、直前に取得している),
                //  ,角度Quaternion.identity（Quaternionは角度Ritationのときに使用、identityは角度をいじらない設定のときに使用））
                //objというGameObjectに右側の生成したShellの情報を入れている。生み出すだけならobjに入れる必要はない。

                //砲身が向いている方向に、
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                float angleZ = transform.localEulerAngles.z;    //砲身の角度はZ軸(float)を見るとわかる

                // floatのZを、Vector型に変換する
                float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);    //直線距離から、底辺 X を計算して出す DeglyからRadianに変換
                float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);    //直線距離から、高さ Y を計算して出す DeglyからRadianに変換
                Vector2 v =new Vector2(x, y)*fireSpeed;     //上２行の答えをvにVector2として代入
                //普段使っているオイラー角（45度とか）は使えず、ここで指定するのはラジアン角(円周率2π(3.14*2))
                //angleZに入っているはオイラー角なので、Mathf.Deg2Ra

                rbody.AddForce(v, ForceMode2D.Impulse); //Impulseの力で、vの方向に飛ばす
                //ここのrbodyは、shell(弾)のRigidbody。第一引数はVector型なので、Intやfloatでは指定できない
            }
        }
    }

    bool CheckLength(Vector2 targetPos) //ターゲットポジション＝プレイヤーの位置
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos);  //Distanceでキャノンとプレイヤーの距離
        if (length >= d)    //距離がlength以下に近づいたなら
        {
            ret = true;
        }
        return ret;
    }

    //範囲表示
    void OnDrawGizmosSelected() //Editorのギズモ
    {
        Gizmos.DrawWireSphere(transform.position, length);  //自分の位置からlengthの円を描画
    }
}
