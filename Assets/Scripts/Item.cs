using UnityEngine;

// アイテムの種類
public enum ItemType
{
    Life,
    Bullet
}

public class Item : MonoBehaviour
{
    // アイテムの種類
    public ItemType itemType;

    // 回復量・補充量
    public int amount = 1;
    
    
    public Material lifeMaterial; //アイテムの見た目のマテリアルを設定するための変数
    public Material bulletMaterial; //アイテムの見た目のマテリアルを設定するための変数

    void Start()
    {
        Renderer itemRenderer = GetComponentInChildren<Renderer>();

        if (itemType == ItemType.Life)
        {
            itemRenderer.material = lifeMaterial; //アイテムの見た目をライフ回復用に設定
        }
        else if (itemType == ItemType.Bullet)
        {
            itemRenderer.material = bulletMaterial; //アイテムの見た目を弾薬補充用に設定
        }
    }

    // プレイヤーに触れたとき
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("接触検知：" + other.gameObject.name);

        if (!other.CompareTag("Player")) return;

        Debug.Log("プレイヤーがアイテムに接触！");

        switch (itemType)
        {
            case ItemType.Life:
                other.GetComponent<PlayerRun>().LifeUp(amount);
                break;

            case ItemType.Bullet:
                other.GetComponent<PlayerShooter>().AddBullet(amount);
                break;
        }

        Destroy(gameObject);
    }
}