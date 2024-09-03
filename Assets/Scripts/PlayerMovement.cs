using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 1�b�ԂɃL�����N�^�[����]������p�x�����W�A���P�ʂŎw��
    [SerializeField] float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    // ��]��ۑ�����
    Quaternion m_Rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // �������Z�����s����FixedUpdate�֐�
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Vector3(x, y, z) �ɒl���Z�b�g����
        m_Movement.Set(horizontal, 0f, vertical);
        // �x�N�g���̑傫���𐳋K���i1�Ɂj����
        // �ړ��x�N�g����X,Y���ǂ����1�̏ꍇ�A�x�N�g���̒�����1���傫���Ȃ�i�s�^�S���X�̒藝�j
        // �L�����N�^�[��1�̎��ɉ����Ĉړ���������΂߂ɕ����ɑ����ړ����邱�Ƃ��Ӗ�����
        // �����Ȃ�Ȃ����߂ɁA���K�����s���x�N�g���̕����𓯂��ɂ��āA���̑傫����1�ɂ���
        m_Movement.Normalize();

        // ���������̓��͂����邩�𔻒f����
        // Mathf.Approximately(float a, float b) a, b�̒l���������ꍇ��true��Ԃ�
        // !(�_���ے艉�Z�q)�����邽��bool�𔽓]���ĕԂ�
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        // ���������̓��͂����邩�𔻒f����
        // Mathf.Approximately(float a, float b) a, b�̒l���������ꍇ��true��Ԃ�
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        // ���� or �����̓��͂�����ꍇ��true��Ԃ�
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        // Animator��bool IsWalking��bool�l�iisWalking�j���Z�b�g����
        m_Animator.SetBool("IsWalking", isWalking);

        // �L�����N�^�[�̑O���x�N�g�����v�Z����
        // transform.forward����m_Movement��ڎw���B
        // turnSpeed * Time.deltaTime�Ŋp�x���}�O�j�`���[�h0�ɂ���
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement , turnSpeed * Time.deltaTime, 0f);

        // ������desiredForward�ɉ�]������
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove()
    {
        // Rigidbody�R���|�[�l���g��MovePosition���Ăяo�����߂ɐV�����ʒu��n��
        // Animator.deltaPosition�̓��[�g���[�V�����ɂ��ʒu�̈ړ���
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);

        // ��]�̐ݒ�i�ύX�������Ă���킯�ł͂Ȃ��j
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
