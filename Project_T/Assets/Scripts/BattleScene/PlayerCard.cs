using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    private UISprite m_CharSprite;
    private UILabel m_CharLevel;
    private UISprite m_HpSprite;
    private UISprite m_ExpSprite;

    private void Awake()
    {
        m_CharSprite = transform.GetChild(0).GetComponent<UISprite>();
        m_CharLevel = transform.GetChild(1).GetComponent<UILabel>();
        m_HpSprite = transform.GetChild(2).GetComponent<UISprite>();
        m_ExpSprite = transform.GetChild(3).GetComponent<UISprite>();
    }

    public void Setting(int iIndex)
    {
        CharPartyData Node = GameManager.instance.PlayerParty[iIndex];

        m_CharLevel.text = "Lv." + Util.ConvertToString(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_LEVEL));   //레벨

        m_HpSprite.fillAmount = Util.FillValue(Node.MaxHP, Node.CurHP); //HP

        m_ExpSprite.fillAmount = Util.FillValue(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_MAX_EXP), Node.CharData.ReturnData(CHARACTER_DATA.CHAR_CUR_EXP));
    }
}
