using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    CharacterController controller; //CharacterControlloerを参照できる変数
    [SerializeField] private GameObject animatorBody; //Animatorコンポーネントの取得
    Animator animator;

    //Vector3 moveDirection = new Vector3(0, 0, 0)
    Vector3 moveDirection = Vector3.zero; //Moveメソッドに与える方向

    public float gravity; //重力値
    public float speedZ; //前に進む力
    public float speedJump; //ジャンプ力

    void Start()
    {
        //必要なコンポーネント取得
        controller = GetComponent<CharacterController>();
        animator = animatorBody.GetComponent<Animator>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            if (Input.GetAxis("Vertical") != 0.0f)
            {
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            }
            else
            {
                moveDirection.z = 0;
            }

            //横回転 Rotateメソッドで直接回転させる
            transform.Rotate(0, Input.GetAxis("Horizontal") * 300 * Time.deltaTime, 0);

            //ジャンプボタン　押されている間
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = speedJump; //y方向をセッティング
                //animator.SetTrigger("Jump"); //ジャンプアニメへの切り替え
            }
        }
        //重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行
        //セッティングしたmoveDirectionの値を使ってMoveメソッドの発動
        //実際にPlayerが動くTime.deltaTimeでPC性能差によらないように動作安定
        //回転を含む前進だと、グローバルな座標指定からズレるので、グローバルな座標になるように変換してからMoveに与える
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後接地してたらY方向の速度はリセットする
        if (controller.isGrounded) moveDirection.y = 0;

        //速度が0以上なら走っているフラグをtrueにする
        //animator.SetBool("run", moveDirection.z != 0.0f);
    }
}
