using UnityEngine;

public class BulletController : MonoBehaviour
{
    // 弾が消えるまでの時間（秒）
    [SerializeField] private float deltaTime = 3.0f;

    void Start()
    {
        // deltaTime秒後に弾を消す
        Destroy(gameObject, deltaTime);
    }

    // 何かに接触したら弾を消す
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}