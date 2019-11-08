using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResqueObject : MonoBehaviour
{
    private UISprite m_CharSprite;
    private UILabel m_TextLabel;
    private void Awake()
    {
        m_CharSprite = gameObject.transform.GetChild(0).GetComponent<UISprite>();
        m_TextLabel = gameObject.transform.GetChild(1).GetComponent<UILabel>();
        gameObject.SetActive(false);
    }

    public void SettingText(string strName)
    {
        //구할 시 이름을 띄운다.
        string strText = m_TextLabel.text;
        strText = string.Format(strText, strName);
        m_TextLabel.text = strText;
        m_CharSprite.spriteName = strName;
        //바꾼다.
        StartCoroutine("ExitTime");
    }

    IEnumerator ExitTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            StopCoroutine("ExitTime");
            gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        //끈다
        StopCoroutine("ExitTime");
        gameObject.SetActive(false);
    }
}
