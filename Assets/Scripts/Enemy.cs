
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyLife = 3; // 敵の体力
    [SerializeField] private GameObject enemyBody; // 敵の体

    // 無敵時間
    [SerializeField] private float invincibleTime = 1.0f;

    // ダメージ中のコルーチン
    Coroutine damageStart;

    void Update()
    {
        // ダメージ中なら点滅する
        if (damageStart != null)
        {
            float val = Mathf.Sin(Time.time * 20.0f);

            if (val > 0)
            {
                //メッシュレンダラーを有効にする
                enemyBody.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                enemyBody.GetComponent<MeshRenderer>().enabled = false;
            }

            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ダメージ中でなければ
        if (damageStart == null)
        {
            // 弾に当たったとき
            if (other.gameObject.CompareTag("Bullet"))
            {
                enemyLife--;

                if (enemyLife <= 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    damageStart = StartCoroutine(DamageStart());
                }
            }
        }
    }

    // ダメージ処理のコルーチン
    IEnumerator DamageStart()
    {
        // 無敵時間が終わるまで待つ
        yield return new WaitForSeconds(invincibleTime);

        // 敵の表示を元に戻す
        enemyBody.GetComponent<MeshRenderer>().enabled = true;

        // ダメージ状態を解除
        damageStart = null;
    }
}
