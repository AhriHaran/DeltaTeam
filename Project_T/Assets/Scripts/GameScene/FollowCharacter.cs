using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Transform m_PlayerTR;
    public float m_OffsetX;
    public float m_fSpeed = 2.0f;
    private Vector3 m_OffsetVector = new Vector3();

    private void LateUpdate()
    {
        if(m_PlayerTR.position.x >= 0 && m_PlayerTR.position.x <= 60)
        {
            float fX = m_PlayerTR.position.x + m_OffsetX;
            m_OffsetVector.Set(fX, transform.position.y, transform.position.z);
            transform.position = m_OffsetVector;
        }
    }
}
