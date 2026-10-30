
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFreeMove : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;

    CharacterController controller;
    [SerializeField] private GameObject animatorBody;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;

    [Header("前進加速力")]
    public float accelerationZ;

    // Input System
    private InputAction moveAction;
    private InputAction jumpAction;

    // 入力間隔を管理するコルーチン
    Coroutine resetIntervalCol;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //animator = animatorBody.GetComponent<Animator>();

        // PlayerInputからアクションを取得
        PlayerInput playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move", true);
        jumpAction = playerInput.actions.FindAction("Jump", true);
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            // コルーチンが動いていない場合だけ入力
            if (resetIntervalCol == null)
            {
                float horizontal = moveAction.ReadValue<Vector2>().x;

                if (horizontal < -0.5f)
                    MoveToLeft();
                else if (horizontal > 0.5f)
                    MoveToRight();
            }

            // レーンへの横移動
            float ratioX =
                (targetLane * LaneWidth - transform.position.x)
                / LaneWidth;
           moveDirection.x = ratioX * speedX;

            // 前進方向に徐々に加速
            float acceleratedZ =
                moveDirection.z + accelerationZ * Time.deltaTime;

            moveDirection.z = Mathf.Clamp(
                acceleratedZ, 0, speedZ);

            // ジャンプ入力
            if (jumpAction.IsPressed())
            {
                Jump();
            }
        }

        // 重力
        moveDirection.y -= gravity * Time.deltaTime;

        // ローカル座標からワールド座標へ変換
        Vector3 globalDirection =
            transform.TransformDirection(moveDirection);

        controller.Move(globalDirection * Time.deltaTime);

        // 接地時に落下速度をリセット
        if (controller.isGrounded)
        {
            moveDirection.y = 0;
        }
    }

    // 左レーンへ移動
    public void MoveToLeft()
    {
        if (targetLane > MinLane)
        {
            targetLane--;

            resetIntervalCol =
                StartCoroutine(ResetIntervalCol());
        }
    }

    // 右レーンへ移動
    public void MoveToRight()
    {
        if (targetLane < MaxLane)
        {
            targetLane++;

            resetIntervalCol =
                StartCoroutine(ResetIntervalCol());
        }
    }

    // 次の入力まで0.1秒待機
    IEnumerator ResetIntervalCol()
    {
        yield return new WaitForSeconds(0.1f);
        resetIntervalCol = null;
    }

    // ジャンプ
    public void Jump()
    {
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;
        //    animator.SetTrigger("Jump");
        }
    }
}
