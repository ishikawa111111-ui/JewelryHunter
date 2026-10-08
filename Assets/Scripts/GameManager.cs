using UnityEngine;
using UnityEngine.UI;               // UIを使うのに必要
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;        //画像を持つImageゲームオブジェクト。初期はGAME STARTという絵
                                        //GameObjectは、ヒエラルキーにある物体を指名したい時に使用
    public Sprite gameOverSpr;          //画像
    public Sprite gameClearSpr;         //画像
    public GameObject panel;            //パネル
    public GameObject restartButton;    //ボタン(アタッチ)
    public GameObject nextButton;       //ボタン
    Image titleImage;                   //画像を表示するImageコンポーネント情報。使っていない
    GameState gameState=GameState.InGame;   //GameStateは自作の型PlayerController内のenum

    public string nextSceneName;    //次のステージ（またはオールクリア）

    //時間制限
    public GameObject timeBar;
    public GameObject timeText;
    TimeController timeCnt;         //自作のTimeControllerを使う宣言。あとでGetComponent

    //スコア
    public GameObject scoretext;
    public static int totalScore;
    public int stageScore = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InactiveImage", 1.0f);  
        // Invoke = 1秒後に指定したメソッド(InactiveImage)を実行する。ちな画像を消す自作メソッド

        panel.SetActive(false); //panelは即非表示

        //制限時間
        timeCnt = GetComponent<TimeController>();   //GameManager(これ自身)からTimeControllerをいじる
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)   //特に時間設定されていないなら
            {
                timeBar.SetActive(false);   //UI隠す
            }
        }

        UpdateScore();


    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == GameState.GameClear)
        {
            gameState = GameState.GameClear;
            mainImage.SetActive(true);  //画像を表示
            panel.SetActive(true);      //panelを表示

            Button bt=restartButton.GetComponent<Button>(); 
            // リスタートボタンのコンポーネントをいじる宣言
            bt.interactable = false;
            // リスタートボタンを無効化（インスペクタのinteractable チェックOFF）
            // restartButton.GetComponent<Button>().interactable = true; この書き方でもOK

            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            // GAME START画像をGAME CLEAR画像に差し替える

            PlayerController.gameState = GameState.GameEnd;

            if (timeCnt != null)    //時間があるなら
            {
                timeCnt.isTimeOver=true; //カウント停止

                int time=(int)timeCnt.displayTime;
                totalScore += time * 10;    //残り時間をスコアに足す
            }

            totalScore += stageScore;   //ステージ中に稼いだスコアをトータルに足す
            stageScore = 0;
            UpdateScore();

        }
        else if (PlayerController.gameState == GameState.GameOver)
        {
            gameState=GameState.GameOver;
            mainImage.SetActive(true);
            panel.SetActive(true);
            Button bt=nextButton.GetComponent<Button>();
            bt.interactable=false;
            mainImage.GetComponent <Image>().sprite = gameOverSpr;
            PlayerController.gameState=GameState.GameEnd;
            if (timeCnt != null)    //時間があるなら
            {
                timeCnt.isTimeOver = true; //カウント停止
            }
        }
        else if (PlayerController.gameState == GameState.InGame)    //ゲーム中の処理
        {
            //下2行の処理は、UpdateのInGameで毎回負荷をかけるより、Startの中に書いた方が軽くなる
            //（ただし、ローカルにならないようにグローバル変数化が必要）

            //Playerというタグがついているオブジェクトを探してくる
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //PlayerオブジェクトのPlayerControllcomponentを取得
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            //ここまでで、扱えるようになったので

            //UI
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)   //時間があるなら
                {
                    //表示用に、int型にして小数点以下を切り捨て
                    int time = (int)timeCnt.displayTime;

                    //timeTextというGameObjectのGetCompornent＝time(intなのでstringにする)
                    timeText.GetComponent<TextMeshProUGUI>().text = time.ToString();
                    // TextMeshProは3Dオブジェクト、これはUIなので、TextMeshProUGUIになる

                    //タイムオーバー
                    if (time == 0)
                    {
                        playerCnt.GameOver();
                    }
                }
            }

            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();

            }

        }
    }

    // 画像を非表示にする
    void InactiveImage()
    {
        mainImage.SetActive(false); //SetActiveよく使う。インスペクタ一番上のチェックON/OFF
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //自分自身を読み込みなおす(GetActiveScene)ことで、最初からになり、＝リトライになる。
        //現シーンの名前(name)を引数。ここを別のシーンにすると、それを読む(次のNextみたいに)
    }

    public void Next()  //次のステージ（またはオールクリア）
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void UpdateScore()  //スコア表示の更新
    {
        int score = stageScore + totalScore;
        scoretext.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
