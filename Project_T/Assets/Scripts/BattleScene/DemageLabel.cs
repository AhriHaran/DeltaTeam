using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemageLabel : MonoBehaviour
{
    private UILabel m_DemageLabel;
    // Start is called before the first frame update
    private void Awake()
    {
        m_DemageLabel = transform.GetComponent<UILabel>();
    }

    public void Setting(string strDemage)
    {
        m_DemageLabel.text = strDemage; //데미지 라벨
        gameObject.GetComponent<TweenPosition>().enabled = true;
        gameObject.GetComponent<TweenAlpha>().enabled = true;
    }
    
    public void OnFinished()
    {
        gameObject.GetComponent<TweenPosition>().from = new Vector3(0, 300, 0);
        gameObject.GetComponent<TweenPosition>().to = new Vector3(0, 500, 0);

        gameObject.GetComponent<TweenAlpha>().from = 1.0f;
        gameObject.GetComponent<TweenAlpha>().to = 0.0f;
        gameObject.SetActive(false);
    }
}
