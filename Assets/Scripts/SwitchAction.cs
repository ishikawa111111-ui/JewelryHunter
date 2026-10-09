using UnityEngine;

public class SwitchAction : MonoBehaviour
{
    public GameObject targetMoveBlock;  //どのブロックがスイッチに対応しているのか、インスペクタで指名
    public Sprite imageOff; //publicでインスペクタで絵が指定できる
    public Sprite imageOn;  //publicでインスペクタで絵が指定できる
    public bool on = false; // スイッチの状態(true:押されている false:押されていない)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (on) //絵の切り替え
        {
            GetComponent<SpriteRenderer>().sprite = imageOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = imageOff;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") //接触した相手がPlayerなら
        {
            if (on) //スイッチONならOFFにする
            {
                on = false;
                GetComponent<SpriteRenderer>().sprite = imageOff;   //スイッチ画像差し替え
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock>(); //コンポーネントを一旦movBlockに入れて、
                movBlock.Stop();    //OFFになったのでブロック移動をストップさせる
            }
            else //スイッチOFFならONにする
            {
                on = true;
                GetComponent<SpriteRenderer>().sprite = imageOn;   //スイッチ画像差し替え
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock>();
                movBlock.Move();    //ONになったのでブロック移動を開始させる
            }
        }
    }
}
