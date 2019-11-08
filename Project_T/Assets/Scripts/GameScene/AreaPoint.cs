using UnityEngine;
using System.Collections;

public class AreaPoint : MonoBehaviour
{
    //해당 포인트에 도달하면 맵과 상호작용으로 갈 수 있다.
    public int m_iArea;

    private Transform m_PlayerTR;
    private CallBackBool m_AreaPoint;

    private void Awake()
    {
        m_PlayerTR = GameObject.Find("GameObject").transform.GetChild(1).GetChild(0).transform;
    }

    public void CallBackSetting(CallBackBool callBack)
    {
        m_AreaPoint = callBack;
    }

    public void CoroutineStart(bool bStart)
    {
        if(bStart)
        {
            StartCoroutine("AreaPointCheck");
        }
        else
        {
            StopCoroutine("AreaPointCheck");
        }
    }

    IEnumerator AreaPointCheck()
    {
        while (true)
        {
            if ((transform.position.x - m_iArea) <= m_PlayerTR.position.x && (transform.position.x + m_iArea) >= m_PlayerTR.position.x)
            {
                //플레이어가 해당 포인트에 도달했다.
                m_AreaPoint?.Invoke(true);
                Debug.Log("AreaPoint");
            }
            else
            {
                m_AreaPoint?.Invoke(false);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
