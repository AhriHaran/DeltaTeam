using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPanel : MonoBehaviour
{
    public int m_iPartyPanelIndex;
    private int m_iIndex = -1;  //플레이어 인덱스
    public UISprite m_CharSprite;
    public GameObject m_HPSlider;  //캐릭터 HP 슬라이더
    public GameObject m_Status;

    private void Awake()
    {
        int Icount = 0;
        m_CharSprite = transform.GetChild(Icount).GetComponent<UISprite>();
        Icount++;
        m_HPSlider = transform.GetChild(Icount).gameObject;
        Icount++;
        m_Status = transform.GetChild(Icount).gameObject;
    }

    public void ShowPartyInfo(int iIndex)
    {
        CharPartyData Party = UserInfo.instance.PartyList[iIndex];
        m_HPSlider.GetComponent<HPSlider>().ShowHP(Party.MaxHP, Party.CurHP);
        m_Status.GetComponent<ShowStatus>().ShowPartyStatus(iIndex, CHARACTER_TYPE.CHAR_PLAYER, false);
        m_CharSprite.spriteName = Util.ConvertToString(Party.CharData.ReturnData(CHARACTER_DATA.CHAR_NAME));
        m_iIndex = iIndex;
    }

    public void DragEnd()
    {
        Debug.Log(m_iPartyPanelIndex);

    }
}
