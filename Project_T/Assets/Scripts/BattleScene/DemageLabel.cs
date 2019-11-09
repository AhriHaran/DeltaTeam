using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemageLabel : MonoBehaviour
{
    private UILabel m_DemageLabel;
    private TweenPosition m_TweenPos;
    private TweenAlpha m_TweenAlpha;
    // Start is called before the first frame update
    private void Awake()
    {
        m_DemageLabel = transform.GetComponent<UILabel>();
        m_TweenPos = gameObject.GetComponent<TweenPosition>();
        m_TweenAlpha = gameObject.GetComponent<TweenAlpha>();
    }

    public void Setting(string strDemage)
    {
        m_DemageLabel.text = strDemage; //데미지 라벨

        m_TweenPos.from = new Vector3(0, 300, 0);
        m_TweenPos.to = new Vector3(0, 500, 0);

        m_TweenAlpha.from = 1.0f;
        m_TweenAlpha.to = 0.0f;
        
        m_TweenPos.PlayForward();
        m_TweenAlpha.PlayForward();
    }

    public void OnFinished()
    {
        gameObject.SetActive(false);
    }
}
