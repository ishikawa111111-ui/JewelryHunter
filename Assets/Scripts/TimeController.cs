using Unity.VisualScripting;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true;     // true=時間をカウントダウン計測する
    public float gameTime = 0;          // ゲームの最大時間
    public bool isTimeOver = false;     // true=タイマー停止
    public float displayTime = 0;       // 表示時間
    float times = 0;                    // 現在時間

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isCountDown)    //bool型 if (isCountDown == true)と同じ。省略
        {
            displayTime = gameTime;     // カウントダウン
        }

        //if(!isCountDown) // ! を付けるとif (isCountDown == false)or(isCountDown != true)と同じ。省略

    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeOver == false)    //タイムオーバーでないなら、処理
        {
            times += Time.deltaTime;    //毎フレームかかった時間を足しているので、経過時間


            if (isCountDown)        // カウントダウン
            {
                displayTime = gameTime - times;
                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f;
                    isTimeOver = true;  //時間が0になったらタイムオーバー
                }
            }
            else                    // カウントアップ
            {
                displayTime = times;
                if (displayTime >= gameTime)
                {
                    displayTime = gameTime;
                    isTimeOver = true;  //時間上限になったらタイムオーバー
                }
            }
            //Debug.Log("TIMES: " + displayTime);
        }
    }
}
