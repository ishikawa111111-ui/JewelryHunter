using UnityEngine;
using UnityEngine.SceneManagement;  //シーンの切り替えに必要
using TMPro;

public class ResultMnager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;   
    //いきなりコンポーネントから指定している（早いパターン）ヒエラルキーはTMPをアタッチ
    public string sceneName;

    //例
    public GameObject scoreTextObject;  
    //ゲームオブジェクトから指定する場合（従来のやり方。遅い）ヒエラルキーはObjectをアタッチ

    void Start()
    {
        scoreText.text=GameManager.totalScore.ToString();
    }

    void Update()
    {
    }

    public void Load()  //変数SceneNameに書いておいたシーンにジャンプするメソッド
    {
        SceneManager.LoadScene(sceneName);
        // SceneManager.LoadScene("Title");　←インスペクタにTitleと書いているので、これと同じ
    }

}
