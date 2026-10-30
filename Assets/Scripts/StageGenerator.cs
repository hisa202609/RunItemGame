using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30; //ステージ奥行き(Z)のサイズ

    int currentChipIndex; //生成済みのフロア番号

    //教科書ではpublic Transform character;
    Transform character;
    public GameObject[] stageChips;
    public int startChipIndex = 1;
    public int preInstantiate = 5;
    public List<GameObject> generatedStageList = new List<GameObject>();

    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").transform; //Playerを検索し、そのTransformを取得
        currentChipIndex = startChipIndex - 1; //現在基準のフロアを-1にする
        UpdateStage(preInstantiate); //まず最初にいくつかのフロアを生成する
    }

    void Update()
    {
        //キャラクターの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        //次のステージチップに入ったらステージの更新処理を行う
        if (charaPositionIndex + preInstantiate > currentChipIndex)
        {
            //新しく予備分の生成を補う(引数：ここまで作っておいてほしいというフロア番号を指定)
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    // 指定のIndexまでのステージチップを生成して、管理下に置く
    void UpdateStage(int toChipIndex)
    {
        //if(toChipIndex <= currentChipIndex) return;

        // 現在生成済みフロア番号＋１　から始め、作るべきフロア番号にたどり着くまで繰り返し生成
        for (int i = currentChipIndex + 1; i <= toChipIndex; i++)
        {
            //実際にInstantiateするメソッドを発動、それにより生成されたフロア情報は変数に確保
            GameObject stageObject = GenerateStage(i);
            generatedStageList.Add(stageObject);　//生成したステージチップを管理リストに追加
        }

        // 必要な数に減少するまで、フロア削除メソッドを繰り返す
        // 管理リストに記帳されているフロア数が(予備数)
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldStage();

        currentChipIndex = toChipIndex;
    }
    
    //指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int chipIndex)
    {
        //どのフロア種類を生成しようか、配列からランダムにチョイス
        int nextStageChip = Random.Range(0, stageChips.Length);

        //引数のフロア番号に対応した座標に、配列から選ばれたフロアを生成
        GameObject stageObject = Instantiate(
            stageChips[nextStageChip],
            new Vector3(0, 0, chipIndex * StageChipSize),
            Quaternion.identity
            );

        return stageObject; //生成されたフロア情報を戻り値として返す
    }

    //管理リストから古いフロアを　①リストから　②実際にオブジェクトも削除するメソッド
    void DestroyOldStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}

