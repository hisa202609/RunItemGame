
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("生成される弾")]
    [SerializeField] private GameObject _bullet;

    [Header("弾の速度")]
    [SerializeField] private float _bulletSpeed = 10.0f;
    [SerializeField] private float _shootInterval = 0.3f;

    const int startRemaining = 10; // 初期残弾数
    private int bulletRemaining = startRemaining; // 残弾数

    //UIなどに残数を伝えるアクセサメソッド
    public int GetRemaining()
    {
        return bulletRemaining;
    }

    //弾数を回復するメソッド
    public void AddBullet(int recoverAmount)
    {
        bulletRemaining += recoverAmount;
        if (bulletRemaining > startRemaining)
        {
            bulletRemaining = startRemaining;
        }
    }

    InputAction shootAction;

    Coroutine shootStart;

    void Start()
    {
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        shootAction = actionMap.FindAction("Attack");
    }
    void Update()
    {
        // Attackボタンが押された時、かつ残弾があればShoot()を呼ぶ
        if (shootAction.WasPerformedThisFrame() && bulletRemaining > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shootStart == null)
        {
            bulletRemaining--; //残弾を減らす

            UIController controller =
            GameObject.FindAnyObjectByType<UIController>();

            controller.UpdateBullet();

            // 弾を生成する
            GameObject newBullet = Instantiate(
                _bullet,
                transform.position,
                Quaternion.identity
            );

            // Z軸の正方向に弾を飛ばす
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            rb.AddForce(
                Vector3.forward * _bulletSpeed,
                ForceMode.Impulse
            );

            // インターバル開始
            shootStart = StartCoroutine(ShootStart());
        }
    }

    // インターバル用コルーチン
    IEnumerator ShootStart()
    {
        // _shootInterval秒待つ
        yield return new WaitForSeconds(_shootInterval);

        // コルーチン変数の開放
        shootStart = null;
    }
}
