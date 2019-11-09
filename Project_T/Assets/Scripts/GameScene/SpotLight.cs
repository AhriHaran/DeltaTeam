using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLight : MonoBehaviour
{
    //히트 체크
    public GameObject m_HitPoint;
    private Light m_Light;
    private Light m_ChildeLight;
    private HitRaycast m_HitRayCast;

    private int m_iBlockIndex;
    public int BlockIndex
    {
        get { return m_iBlockIndex; }
        set { m_iBlockIndex = value; }
    }   //현재 블럭 인덱스
    
    private float m_fOnOffSpeed = 0.0f; //켜지는 시간
    public float OnOffSpeed
    {
        get { return m_fOnOffSpeed; }
        set { m_fOnOffSpeed = value; }
    }

    private float m_fOnOffTime = 0.0f;  //켜지는 거 유지되는 시간
    public float OnOffTime
    {
        get { return m_fOnOffTime; }
        set { m_fOnOffTime = value; }
    }

    private float m_fCurTime = 0.0f;
    private bool m_bOn = false;
    //0~ 1.5 히트 체크
    private void Awake()
    {
        m_Light = transform.GetComponent<Light>();
        m_ChildeLight = transform.GetChild(0).GetComponent<Light>();
        OnOffSpeed = 0.0f;
        OnOffTime = 0.0f;
        m_fCurTime = 0.0f;
        m_bOn = false;
        m_HitRayCast = m_HitPoint.GetComponent<HitRaycast>();
    }
    
    public void ColorSetting(float R, float G, float B)
    {
        m_Light.color = new Color(R, G, B);
        m_ChildeLight.color = new Color(R, G, B);
    }

    public void CoroutineStart(bool bStart)
    {
        if (OnOffSpeed != 0.0f)
        {
            if (bStart)
            {
                StartCoroutine("SpotLightMove");
            }
            else
            {
                StopCoroutine("SpotLightMove");
            }
        }
    }


    //스포트 라이트 꺼졌다 켜졌다.

    IEnumerator SpotLightMove()
    {
        while (true)
        {
            m_fCurTime += Time.deltaTime;
            
            if(!m_bOn)
            {
                //안켜짐
                if (m_fCurTime >= OnOffSpeed)
                {
                    m_Light.enabled = true;
                    m_ChildeLight.enabled = true;
                    //안켜진 상태 시간 지나면 켜준다.
                    m_HitPoint.SetActive(true);
                    m_bOn = true;
                    m_fCurTime = 0.0f;
                }
            }
            else
            {
                //켜진 상태
                if (m_fCurTime >= OnOffTime)
                {
                    //켜진상태 시간 지나면 꺼주고
                    m_Light.enabled = false;
                    m_ChildeLight.enabled = false;
                    m_HitPoint.SetActive(false);
                    m_bOn = false;
                    m_fCurTime = 0.0f;
                }
            }
            
            yield return null;
        }
    }
}
