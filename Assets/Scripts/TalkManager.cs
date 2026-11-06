using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;


public class TalkManager : MonoBehaviour
{
    public TalkData currentTalkData; //現在の会話データを保持する変数

    //UI側のパーツ
    public GameObject talkPanel; //会話パネル
    public TextMeshProUGUI nameText; //名前表示用テキスト
    public TextMeshProUGUI messageText; //セリフ表示用テキスト
    public Image faceImage; //顔画像表示用イメージ

    public float textSpeed = 0.04f; //文字送りのスピード

    private int currentIndex = -1; //現在の会話データのインデックス
    private bool isTypeing = false; //文字送り中かどうかのフラグ
    private Coroutine typingCoroutine; //文字送りコルーチンの参照変数
    private string currentFullText = ""; //現在のセリフの全文
    public string nextScene;

    //ボタン入力
    InputAction submitAction;


    void Start()
    {
        //現マップ(UI)の情報を取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        submitAction = actionMap.FindAction("Submit");　//現マップ上にいる"Submit"というアクションを取得

        //最初のメッセージ
        AdvanceTalk();
    }

    void Update()
    {
        if(submitAction.WasPressedThisFrame())
        {
            AdvanceTalk();
        }
    }

    public void AdvanceTalk()
    {
        if (currentTalkData == null || currentTalkData.talks.Count == 0)
        {
            Debug.LogWarning("会話データが設定されていません。");
            return;
        }

        if(isTypeing)
        {
            //文字送り中にボタンが押されたら、全文を表示する
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            messageText.text = currentFullText; //全文を表示
            isTypeing = false; //文字送り中フラグをOFF
            return; 
        }

        currentIndex++;

        if (currentIndex >= currentTalkData.talks.Count)
        {
            EndTalk(); //終了のメソッド
            return;
        }

        if (talkPanel != null && talkPanel.activeSelf)
        {
            talkPanel.SetActive(true); //会話パネルを表示
        }

        //対象データべースに記載されているリストのIndex番目を参照
        //SingleTalkクラスには名前、セリフ、顔画像の情報が入っている
        SingleTalk talk = currentTalkData.talks[currentIndex];

        if(nameText != null)
        {
            nameText.text = talk.name; //名前を表示
        }

        if(faceImage != null)
        {
            faceImage.gameObject.SetActive(talk.faceImage != null);
            faceImage.sprite = talk.faceImage; //あれば差し替え、nullなら非表示
        }

        currentFullText = talk.message; //抜き出したSingleTalkクラスのメッセージを変数に
        //messageText.text = currentFullText; //メッセージUIに反映
        typingCoroutine = StartCoroutine(TypeTextCoroutine(currentFullText)); //文字送りコルーチンを発動
    }

    IEnumerator TypeTextCoroutine (string text)
    {
        isTypeing = true; //文字送り中フラグをON
        messageText.text = ""; //メッセージUIを空にする

        //引数に渡したメッセージ全文を分解
        foreach (char c in text.ToCharArray())
        {
            messageText.text += c; //1文字ずつ追加していく
            yield return new WaitForSeconds(textSpeed); //指定秒数待つ
        }

        isTypeing = false; //文字送り中フラグをOFF
    }

    void EndTalk()
    {
        currentIndex = -1; //インデックスを初期化

        if (talkPanel != null)
        {
            talkPanel.SetActive(false); //会話パネルを非表示
        }
        //次のシーンに遷移する
        if (nextScene != "")
        {
            StartCoroutine(NextScene());
        }
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(nextScene); //次のシーンに遷移する
    }
}
