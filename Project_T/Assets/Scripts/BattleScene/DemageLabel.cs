using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemageLabel : MonoBehaviour
{
    private float m_fPosY = 0.0f;
    private float m_fYSpeed = 5.0f;
    private float m_fAlpha = 0.05f;

    private UILabel m_DemageLabel;
    // Start is called before the first frame update
    private void Awake()
    {
        m_DemageLabel = transform.GetComponent<UILabel>();
    }

    public void Setting(string strDemage)
    {
        m_DemageLabel.text = strDemage; //데미지 라벨
        transform.localPosition.Set(0.0f, m_fPosY, 0.0f);
        m_DemageLabel.alpha = 1.0f;
        StartCoroutine("TweenLabel");
    }
    
    IEnumerator TweenLabel()
    {
        while(true)
        {
            float fY = transform.localPosition.y;
            bool bStop = false;
            fY += m_fYSpeed;
            transform.localPosition = new Vector3(0.0f, fY);

            fY = m_DemageLabel.alpha;
            fY -= m_fAlpha;

            if (fY < 0.0f)
            {
                fY = 0.0f;
                bStop = true;
            }

            m_DemageLabel.alpha = fY;

            if (bStop)
                StopCoroutine("TweenLabel");

            yield return null;
        }
    }
}
