using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUI : MonoBehaviour
{
    private UISprite m_CharSprite;
    private GameObject m_PlayerObject;
    private GameObject m_EnemyObject;
    private GameObject m_ShowStatus;
    private UILabel m_SkillATK;
    private GameObject m_HPUIObject;

    private void Awake()
    {
        int iIndex = 0;
        m_CharSprite = transform.GetChild(iIndex).GetComponent<UISprite>();
        iIndex++;
        m_PlayerObject = transform.GetChild(iIndex).gameObject;
        iIndex++;
        m_EnemyObject = transform.GetChild(iIndex).gameObject;
        iIndex++;
        m_ShowStatus = transform.GetChild(iIndex).gameObject;
        iIndex++;
        m_SkillATK = transform.GetChild(iIndex).GetComponent<UILabel>();
        iIndex++;
        m_HPUIObject = transform.GetChild(iIndex).gameObject;
        m_PlayerObject.SetActive(false);
        m_EnemyObject.SetActive(false);
    }

    public void UISetting(CHARACTER_TYPE eType, int iIndex)
    {
        //공격 가능자의 UI를 셋팅
        if(eType == CHARACTER_TYPE.CHAR_PLAYER)
        {
            m_CharSprite.spriteName = Util.ConvertToString(GameManager.instance.PlayerParty[iIndex].CharData.ReturnData(CHARACTER_DATA.CHAR_NAME));
            m_PlayerObject.SetActive(true);
            m_EnemyObject.SetActive(false);

            m_PlayerObject.transform.GetChild(0).GetComponent<SkillIcon>().Setting(iIndex);
            m_PlayerObject.transform.GetChild(1).GetComponent<SkillIcon>().Setting(iIndex);

            m_SkillATK.text = "공격력 * " + Util.ConvertToString(GameManager.instance.PlayerParty[iIndex].CharData.ReturnData(CHARACTER_DATA.CHAR_SKILL_DEMAGE));
            //스킬 데미지 쇼
        }
        else if(eType != CHARACTER_TYPE.CHAR_NONE)
        {
            //m_CharSprite
            m_PlayerObject.SetActive(false);
            m_EnemyObject.SetActive(true);
            m_SkillATK.gameObject.SetActive(false);
            CHARACTER_TYPE eBattleType = GameManager.instance.WhoBattleType;
            if (eBattleType == CHARACTER_TYPE.CHAR_ENEMY)
                m_EnemyObject.transform.GetChild(0).GetComponent<UILabel>().text = "일반 적";
            else if (eBattleType == CHARACTER_TYPE.CHAR_DANGEROUS)
                m_EnemyObject.transform.GetChild(0).GetComponent<UILabel>().text = "강한 적";
            else if (eBattleType == CHARACTER_TYPE.CHAR_BOSS)
            {
                m_EnemyObject.transform.GetChild(0).GetComponent<UILabel>().text = "보스";
                m_SkillATK.gameObject.SetActive(true);

                //보스는 스킬 데미지를 적어준다.
            }
        }
        m_ShowStatus.GetComponent<ShowStatus>().ShowPartyStatus(iIndex, eType, true);
        //스테이터스 쇼
        m_HPUIObject.GetComponent<GageUI>().Setting(iIndex, eType);
        //UI셋팅
    }

    public void HPRefresh()
    {
        //HP 리프레쉬
        m_HPUIObject.GetComponent<GageUI>().SliderSetting();
    }
    
}
