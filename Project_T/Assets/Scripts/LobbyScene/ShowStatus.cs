using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStatus : MonoBehaviour
{
    private int m_iIndex;

    public UILabel m_CharClass;
    public UILabel m_NameLable;
    public UILabel m_HpLabel;
    public UILabel m_ATKLabel;
    public UILabel m_LevelLabel;
    public UILabel m_SteminaLabel;

    public void ShowCompanionStatus(int iIdnex)
    {
        if (iIdnex != -1)
        {
            CharacterData Node = UserInfo.instance.CompanionList[iIdnex];
            CLASS eClass = (CLASS)Util.EnumChange<CLASS>(Util.ConvertToString(Node.ReturnData(CHARACTER_DATA.CHAR_CLASS_TYPE)));
            if (eClass == CLASS.CLASS_WARRIOR)
                m_CharClass.text = "워리어";
            else if (eClass == CLASS.CLASS_THIEF)
                m_CharClass.text = "도적";
            else if (eClass == CLASS.CLASS_ARCHER)
                m_CharClass.text = "궁수";
            else if (eClass == CLASS.CLASS_WIZARD)
                m_CharClass.text = "마법사";

            //스테이터스
            string Max = Util.ConvertToString(Node.ReturnData(CHARACTER_DATA.CHAR_MAX_HP));
            m_HpLabel.text = Max;
            m_ATKLabel.text = Util.ConvertToString(Node.ReturnData(CHARACTER_DATA.CHAR_ATK));

            string[] Split = Util.ConvertToString(Node.ReturnData(CHARACTER_DATA.CHAR_SPEED)).Split(';');
            m_SteminaLabel.text = Split[0] + " 턴당 " + Split[1] + "스테미너";
            m_LevelLabel.text = "Lv." + Util.ConvertToString(Node.ReturnData(CHARACTER_DATA.CHAR_LEVEL));
            m_iIndex = iIdnex;
            m_NameLable.text = Util.ConvertToString(Node.ReturnData(CHARACTER_DATA.CHAR_NAME));
        }
    }
    
    public void ShowPartyStatus(int iIdnex, CHARACTER_TYPE eType, bool bBattle)
    {
        //파티 원 
        CharPartyData Node = null;

        if(bBattle)
        {
            if (eType == CHARACTER_TYPE.CHAR_PLAYER)
                Node = GameManager.instance.PlayerParty[iIdnex];
            else if(eType != CHARACTER_TYPE.CHAR_NONE)
                Node = GameManager.instance.EnemyParty[iIdnex];
        }
        else
        {
            Node = UserInfo.instance.PartyList[iIdnex];
        }
        
        CLASS eClass = (CLASS)Util.EnumChange<CLASS>(Util.ConvertToString(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_CLASS_TYPE)));
        if (eClass == CLASS.CLASS_WARRIOR)
            m_CharClass.text = "워리어";
        else if (eClass == CLASS.CLASS_THIEF)
            m_CharClass.text = "도적";
        else if (eClass == CLASS.CLASS_ARCHER)
            m_CharClass.text = "궁수";
        else if (eClass == CLASS.CLASS_WIZARD)
            m_CharClass.text = "마법사";

        //스테이터스
        string Max = Util.ConvertToString(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_MAX_HP));
        m_HpLabel.text = Max;
        m_ATKLabel.text = Util.ConvertToString(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_ATK));

        string[] Split = Util.ConvertToString(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_SPEED)).Split(';');
        m_SteminaLabel.text = Split[0] + " 턴당 " + Split[1] + "스테미너";
        m_LevelLabel.text = "Lv." + Util.ConvertToString(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_LEVEL));
        m_iIndex = iIdnex;
        m_NameLable.text = Util.ConvertToString(Node.CharData.ReturnData(CHARACTER_DATA.CHAR_NAME));
    }
}
