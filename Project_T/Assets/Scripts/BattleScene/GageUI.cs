using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GageUI : MonoBehaviour
{
    private int m_iIndex;               //캐릭터 인덱스 해당 캐릭터의 파티 순서
    private CHARACTER_TYPE m_eType;

    private GameObject m_HPObject;
    private GameObject m_SteminaObject;
    private UILabel m_HPLabel;

    private void Awake()
    {
        GameObject Status = transform.GetChild(0).gameObject;

        m_HPObject = Status.transform.GetChild(0).gameObject;
        m_SteminaObject = Status.transform.GetChild(1).gameObject;
        m_HPLabel = Status.transform.GetChild(2).GetComponent<UILabel>();
    }

    public void Setting(int iIndex, CHARACTER_TYPE eType)
    {
        m_iIndex = iIndex;
        m_eType = eType;
        SliderSetting();
    }

    public void SliderSetting()
    {
        //슬라이더 셋팅(HP, MP, Turn)
        List<CharPartyData> PartyList = null;
        if (m_eType == CHARACTER_TYPE.CHAR_PLAYER)
        {
            PartyList = GameManager.instance.PlayerParty;
        }
        else if (m_eType == CHARACTER_TYPE.CHAR_ENEMY)
        {
            PartyList = GameManager.instance.EnemyParty;
        }

        if(PartyList.Count > 0)
        {
            m_HPObject.GetComponent<HPSlider>().ShowHP(PartyList[m_iIndex].MaxHP, PartyList[m_iIndex].CurHP);
            m_SteminaObject.GetComponent<SteminaSlider>().ShowStemina(PartyList[m_iIndex].CurStemina);
            int iMax = Util.ConvertToInt(PartyList[m_iIndex].MaxHP);
            int iCur = Util.ConvertToInt(PartyList[m_iIndex].CurHP);
            m_HPLabel.text = iCur + "/" + iMax; //HP  max/cur
        }
    }
}
