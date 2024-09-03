using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 1秒間にキャラクターを回転させる角度をラジアン単位で指定
    [SerializeField] float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    // 回転を保存する
    Quaternion m_Rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // 物理演算を実行するFixedUpdate関数
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Vector3(x, y, z) に値をセットする
        m_Movement.Set(horizontal, 0f, vertical);
        // ベクトルの大きさを正規化（1に）する
        // 移動ベクトルがX,Yがどちらも1の場合、ベクトルの長さが1より大きくなる（ピタゴラスの定理）
        // キャラクターが1つの軸に沿って移動するよりも斜めに方向に速く移動することを意味する
        // そうならないために、正規化を行いベクトルの方向を同じにして、その大きさを1にする
        m_Movement.Normalize();

        // 水平方向の入力があるかを判断する
        // Mathf.Approximately(float a, float b) a, bの値が等しい場合にtrueを返す
        // !(論理否定演算子)があるためboolを反転して返す
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        // 垂直方向の入力があるかを判断する
        // Mathf.Approximately(float a, float b) a, bの値が等しい場合にtrueを返す
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        // 水平 or 垂直の入力がある場合にtrueを返す
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        // Animatorのbool IsWalkingにbool値（isWalking）をセットする
        m_Animator.SetBool("IsWalking", isWalking);

        // キャラクターの前方ベクトルを計算する
        // transform.forwardからm_Movementを目指す。
        // turnSpeed * Time.deltaTimeで角度をマグニチュード0にする
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement , turnSpeed * Time.deltaTime, 0f);

        // 向きをdesiredForwardに回転させる
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove()
    {
        // RigidbodyコンポーネントのMovePositionを呼び出すために新しい位置を渡す
        // Animator.deltaPositionはルートモーションによる位置の移動量
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);

        // 回転の設定（変更を加えているわけではない）
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
