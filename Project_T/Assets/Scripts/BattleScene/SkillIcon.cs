using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SkillIconCallBack(CHARACTER_SKILL_TYPE eType);

public class SkillIcon : MonoBehaviour
{
    public UISprite m_SkillIcon;
    //스킬 아이콘 셋팅 및 스킬 아이콘 클릭 체크
    public CHARACTER_SKILL_TYPE m_eType;    //해당 버튼

    private int m_iIndex;  //인덱스
    private SkillIconCallBack m_CallBack = null;

    public void Setting(int IList)
    {
        //가데이터
        m_iIndex = IList;
        string Class =Util.ConvertToString(UserInfo.instance.CompanionList[IList].ReturnData(CHARACTER_DATA.CHAR_CLASS_TYPE));
        string Index = Util.ConvertToString(UserInfo.instance.CompanionList[IList].ReturnData(CHARACTER_DATA.CHAR_INDEX));
        
        if(m_eType == CHARACTER_SKILL_TYPE.CHAR_ATTACK)
            m_SkillIcon.spriteName = Class + Index + "_Normal";
        else
            m_SkillIcon.spriteName = Class + Index + "_Skill";
        m_CallBack = null;
    }

    public void Setting(SkillIconCallBack Call)
    {
        m_CallBack = Call;
    }

    public void OnClick()
    {
        if(m_CallBack == null)
        {
            //클릭하면 현재 이러한 스킬 타입을 실행할 것이라고 게임 매니저에저장
            if (GameManager.instance.WhatPhase == GAME_PHASE.PHASE_TARGET_SELECT &&
                GameManager.instance.WhoAttackerIndex == m_iIndex && GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_PLAYER)
            {
                //현재 스킬을 선택할 수 있는 캐릭터만 선택 가능
                GameManager.instance.HowAttacker = m_eType; //무슨 버튼을 눌렀나
                                                            //스킬 선택 페이즈만 선택가능
                GameManager.instance.WhatPhase = GAME_PHASE.PHASE_ATTACK;    //어택으로 전환
                                                                             //에너미 타겟 선택 페이즈
                Debug.Log(m_eType.ToString());
                //스킬 아이콘 클릭
            }
        }
        else
        {
            Debug.Log("SkillIconCallBack");
            m_CallBack?.Invoke(m_eType);
        }
    }
}
