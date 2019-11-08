using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionUI : MonoBehaviour
{
    public GameObject m_CharScroll;
    private GameObject m_Status;    //스테이터스
   
    private UISprite m_CharImage;   //캐릭터 이미지

    private UISprite m_NormalSkillIcon;
    private UISprite m_SkillIcon;
    private UILabel m_SkillName;
    private UILabel m_SkillDemage;
    private UILabel m_SkillCost;
    private UILabel m_SkillIndex;
    private UILabel m_EXP;

    private int m_iCurIndex = -1;
    private CHARACTER_SKILL_TYPE m_eCurSkill = CHARACTER_SKILL_TYPE.CHAR_SKILL_NONE;

    // Start is called before the first frame update
    void Awake()
    {
        int iNum = 0;
        m_CharImage = transform.GetChild(iNum).GetComponent<UISprite>();
        iNum++;
        m_NormalSkillIcon = transform.GetChild(iNum).GetComponent<UISprite>();
        iNum++;
        m_SkillIcon = transform.GetChild(iNum).GetComponent<UISprite>();
        iNum++;
        m_SkillName = transform.GetChild(iNum).GetComponent<UILabel>();
        iNum++;
        m_SkillDemage = transform.GetChild(iNum).GetComponent<UILabel>();
        iNum++;
        m_SkillCost = transform.GetChild(iNum).GetComponent<UILabel>();
        iNum++;
        m_SkillIndex = transform.GetChild(iNum).GetComponent<UILabel>();
        iNum++;
        m_EXP = transform.GetChild(iNum).GetComponent<UILabel>();
        iNum++;
        m_Status = transform.GetChild(iNum).gameObject;
    }

    private void OnEnable()
    {
        //켜지면 무조건 0번쨰 인덱스 데이터
        m_CharScroll.GetComponent<CompanionScroll>().OnClickSetting(ShowInfoMation);//스크롤 뷰 콜백

        ShowInfoMation(0);

        m_NormalSkillIcon.GetComponent<SkillIcon>().Setting(SkillInfoMation);   //스킬 아이콘 콜백
        m_SkillIcon.GetComponent<SkillIcon>().Setting(SkillInfoMation); //스킬 아이콘 콜백

        SkillInfoMation(CHARACTER_SKILL_TYPE.CHAR_ATTACK);
    }

    public void SkillInfoMation(CHARACTER_SKILL_TYPE eIndex)
    {
        //해당 스킬의 설명 스킬 관련 설명
        int iNormal = 0, iSkill = 1;
        if(m_eCurSkill != eIndex)
        {
            var SkillTable = EXCEL.ExcelLoad.Read("Excel/SKILL_TABLE");

            iNormal += m_iCurIndex;
            iSkill += m_iCurIndex;
            int Atk = Util.ConvertToInt(UserInfo.instance.CompanionList[m_iCurIndex].ReturnData(CHARACTER_DATA.CHAR_ATK));
            if (eIndex == CHARACTER_SKILL_TYPE.CHAR_ATTACK)
            {
                m_SkillName.text = Util.ConvertToString(SkillTable[iNormal]["skillname"]);
                m_SkillDemage.text = Util.ConvertToString(Atk);
                m_SkillCost.text = "1 스테미너 소모";
                m_SkillIndex.text = Util.ConvertToString(SkillTable[iNormal]["skillindex"]);
            }
            else if (eIndex == CHARACTER_SKILL_TYPE.CHAR_SKILL)
            {
                m_SkillName.text = Util.ConvertToString(SkillTable[iSkill]["skillname"]);
                float fSkill = float.Parse(Util.ConvertToString(UserInfo.instance.CompanionList[m_iCurIndex].ReturnData(CHARACTER_DATA.CHAR_SKILL_DEMAGE)));
                fSkill *= (float)Atk;
                fSkill = Mathf.Round(fSkill);
                m_SkillDemage.text = Util.ConvertToString(fSkill);
                m_SkillCost.text = "3 스테미너 소모";
                m_SkillIndex.text = Util.ConvertToString(SkillTable[iSkill]["skillindex"]);
            }
            m_eCurSkill = eIndex;
        }
    }

    public void ShowInfoMation(int iIndex)
    {
        //클릭시 해당 캐릭터의 인포메이션 Show
        if(m_iCurIndex != iIndex)
        {
            CharacterData Node = UserInfo.instance.CompanionList[iIndex];
            //m_SpriteImage
            m_NormalSkillIcon.GetComponent<SkillIcon>().Setting(iIndex);   //스킬 아이콘
            m_SkillIcon.GetComponent<SkillIcon>().Setting(iIndex); //스킬 아이콘

            m_Status.GetComponent<ShowStatus>().ShowCompanionStatus(iIndex); //스테이터스 쇼
            
            string iMax = Util.ConvertToString(Node.ReturnData(CHARACTER_DATA.CHAR_MAX_EXP));
            string iCur = Util.ConvertToString(Node.ReturnData(CHARACTER_DATA.CHAR_CUR_EXP));
            m_EXP.text = iMax + " / " + iCur;

            m_iCurIndex = iIndex;
        }
    }
}
