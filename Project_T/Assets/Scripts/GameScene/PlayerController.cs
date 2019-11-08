using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //게임 씬에서 플랫포머 이동
    [Header("Components")]
    public CharacterController m_PlayerController;

    [Header("Animation")]
    public Animator m_Animator;
    private GameObject m_Visual;

    [Header("Move")]
    public float m_MoveSpeed = 3.0f;
    public float m_Move = 0.1f;
    Vector3 m_Input = Vector3.zero;
    private bool m_bLeft = false;
    private bool m_bRight = false;

    private void Awake()
    {
        m_Visual = transform.GetChild(0).gameObject;
    }

    public void OnPressLeft()
    {
        m_bLeft = true;
    }

    public void OnReleaseLeft()
    {
        m_bLeft = false;
    }

    public void OnPressRight()
    {
        m_bRight = true;
    }

    public void OnReleaseRight()
    {
        m_bRight = false;
    }

    private void Update()
    {
        OnInput();
    }

    private void OnInput()
    {
        //버튼 클릭으로 이동
        m_Input = Vector3.zero;
        if (m_bLeft)
        {
            m_Input = Vector3.left;
            m_Visual.transform.localScale = new Vector3(-1, 1, 1);
        }

        if(m_bRight)
        {
            m_Input = Vector3.right;
            m_Visual.transform.localScale = new Vector3(1, 1, 1);
        }


        m_Input = transform.TransformDirection(m_Input);
        m_Input *= m_MoveSpeed;

        m_PlayerController.Move(m_Input * Time.deltaTime);
        m_Animator.SetFloat("Run", (m_Input.x * m_Visual.transform.localScale.x));
        GameManager.instance.CurXPosition = transform.position.x;

    }
}
