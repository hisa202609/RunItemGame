using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.Playables;

//自作の列挙型
public enum GameState
{ 
    None,
    IsPlay,
    GameClear,
    GameOver,
    GameEnd
}


public class GameManeger : MonoBehaviour
{
    //自作した型を扱うstatic変数
    public static GameState gameState;

    //private GameObject mixingCamera;

    public CinemachineCamera[] cameras; //CinemachineCameraを配列で扱う

    public PlayableDirector playableDirector; //PlayableDirectorを参照する変数

    void Start()
    {
        gameState = GameState.IsPlay;

        //mixingCamera = GameObject.FindGameObjectWithTag("MixingCamera");
        //mixingCamera.SetActive(false); //最初はミキシングカメラを非表示

        //mainカメラのCinemachineBrainを無効化
        Camera.main.GetComponent<CinemachineBrain>().enabled = false; //最初はミキシングカメラのカメラを無効化
    }

    void Update()
    {
        if (gameState == GameState.GameOver)
        {
            //UIにゲームオーバーの表示命令
            UIController controller = GameObject.FindAnyObjectByType<UIController>();
            controller.GameOverDisplay();

            //mixingCamera.SetActive(true); //ミキシングカメラを有効化

            Camera.main.GetComponent<CinemachineBrain>().enabled = true; //ミキシングカメラのCinemachineBrainを有効化

            //StartCoroutine(GameOverCamera());

            playableDirector.Play(); //意図的に作っておいたタイムラインを再生する

            gameState = GameState.GameEnd;
        }
    }
    IEnumerator GameOverCamera()
    {
        yield return null;
        cameras[1].Priority = 2;
    }
}
