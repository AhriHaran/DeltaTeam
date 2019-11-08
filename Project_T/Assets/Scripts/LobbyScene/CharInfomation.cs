using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfomation : MonoBehaviour
{
    private UISprite m_CharSprite;
    private UILabel m_CharName;
    private UILabel m_CharStatus;
    private CallBackInt m_OnClick;
    private int m_iIndex;
    private bool m_bClick;

    private void Awake()
    {
        m_CharSprite = transform.GetChild(0).GetComponent<UISprite>();
        m_CharName = transform.GetChild(1).GetComponent<UILabel>();
        m_CharStatus = transform.GetChild(2).GetComponent<UILabel>();
    }

    public void Setting(int iIndex)
    {
        m_iIndex = iIndex;
        string Name = Util.ConvertToString(UserInfo.instance.CompanionList[iIndex].ReturnData(CHARACTER_DATA.CHAR_NAME));
        m_CharSprite.spriteName = Name;
        m_CharName.text = Name;
        string MaxHP = Util.ConvertToString(UserInfo.instance.CompanionList[iIndex].ReturnData(CHARACTER_DATA.CHAR_MAX_HP));
        string ATK = Util.ConvertToString(UserInfo.instance.CompanionList[iIndex].ReturnData(CHARACTER_DATA.CHAR_ATK));
        m_CharStatus.text = "HP: " + MaxHP + "\t" + "ATK: " + ATK;
        m_bClick = false;
    }
    
    public void OnClickSetting(CallBackInt call)
    {
        m_OnClick = call;
        //클릭 이벤트 셋팅
        m_bClick = true;
    }

    public void OnClick()
    {
        if (m_bClick)
        {
            Debug.Log("ClickCallback");
            Debug.Log(m_iIndex);
            m_OnClick?.Invoke(m_iIndex);
        }
    }

    void OnPress()
    {
        Debug.Log("press");
    }
}
