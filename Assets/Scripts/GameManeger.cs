using UnityEngine;
using Unity.Cinemachine;

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

    private GameObject mixingCamera;

    void Start()
    {
        gameState = GameState.IsPlay;

        mixingCamera = GameObject.FindGameObjectWithTag("MixingCamera");
        mixingCamera.SetActive(false); //最初はミキシングカメラを非表示

        //mainカメラのCinemachineBrainを無効化
        Camera.main.GetComponent<CinemachineBrain>().enabled = false; //最初はミキシングカメラのカメラを無効化
    }

    void Update()
    {
        if(gameState == GameState.GameOver)
        {
            //UIにゲームオーバーの表示命令
            UIController controller = GameObject.FindAnyObjectByType<UIController>();
            controller.GameOverDisplay();

            mixingCamera.SetActive(true); //ミキシングカメラを有効化
            Camera.main.GetComponent<CinemachineBrain>().enabled = true; //ミキシングカメラのCinemachineBrainを有効化

            gameState = GameState.GameEnd;
            Debug.Log("GameEND");
        }
    }
}
