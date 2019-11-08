using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CHARACTER_DATA
{
    CHAR_NAME,          //캐릭터 이름 -> Excel
    CHAR_LEVEL,         //캐릭터 레벨 -> JSON
    CHAR_INDEX,         //캐릭터 인덱스 -> JSON

    CHAR_MAX_HP,        //캐릭터 Max HP -> Excel
    CHAR_MAX_EXP,       //캐릭터 현재 레벨 당 EXP -> Excel
    CHAR_CUR_EXP,       //캐릭터 현재 EXP -> JSON

    CHAR_ATK,           //캐릭터 ATK -> Excel
    CHAR_SPEED,         //캐릭터 SPEED -> Excel -> float

    CHAR_CLASS_TYPE,    //캐릭터 클래스 타입 -> Excel
    CHAR_SKILL_DEMAGE,  //캐릭터 스킬 데미지
}

public enum CLASS
{
    CLASS_WARRIOR,
    CLASS_THIEF,
    CLASS_ARCHER,
    CLASS_WIZARD,
    CLASS_NONE,
}

public class CharacterData
{
    private Dictionary<CHARACTER_DATA, object> m_CharStatus = new Dictionary<CHARACTER_DATA, object>();  //캐릭터 스테이터스
    /// <summary>
    /// 첫 데이터 셋팅
    /// </summary>
    /// <param name="ExcelData"></param>
    /// <param name="JsonData"></param>
    public void Init(List<Dictionary<string, object>> ExcelClass, List<Dictionary<string, object>> ExcelEXP,
       List<Dictionary<string, object>> ExcelChar, CharData JsonData)
    {
        //클래스 엑셀, 경험치 엑셀, 캐릭터 엑셀, 캐릭터 json
        int iLevel = JsonData.m_Level - 1;
        NodeSetting(CHARACTER_DATA.CHAR_NAME, ExcelChar[0][CHARACTER_DATA.CHAR_NAME.ToString()]);  //이름
        NodeSetting(CHARACTER_DATA.CHAR_LEVEL, JsonData.m_Level);   //레벨
        NodeSetting(CHARACTER_DATA.CHAR_INDEX, JsonData.m_iIndex);   //레벨

        int TmpData = Util.ConvertToInt(ExcelClass[iLevel][CHARACTER_DATA.CHAR_MAX_HP.ToString()]) +
            Util.ConvertToInt(ExcelChar[0][CHARACTER_DATA.CHAR_MAX_HP.ToString()]);
        //Data Setting
        NodeSetting(CHARACTER_DATA.CHAR_MAX_HP, TmpData);   //Max HP
        NodeSetting(CHARACTER_DATA.CHAR_MAX_EXP, ExcelEXP[iLevel][CHARACTER_DATA.CHAR_MAX_EXP.ToString()]);//Max EXP
        NodeSetting(CHARACTER_DATA.CHAR_CUR_EXP, JsonData.m_CurEXP);//Cur EXP

        TmpData = Util.ConvertToInt(ExcelClass[iLevel][CHARACTER_DATA.CHAR_ATK.ToString()]) +
            Util.ConvertToInt(ExcelChar[0][CHARACTER_DATA.CHAR_ATK.ToString()]);
        NodeSetting(CHARACTER_DATA.CHAR_ATK, TmpData);
        NodeSetting(CHARACTER_DATA.CHAR_SPEED, ExcelClass[iLevel][CHARACTER_DATA.CHAR_SPEED.ToString()]);
        NodeSetting(CHARACTER_DATA.CHAR_CLASS_TYPE, ExcelChar[0][CHARACTER_DATA.CHAR_CLASS_TYPE.ToString()]);
        NodeSetting(CHARACTER_DATA.CHAR_SKILL_DEMAGE, ExcelClass[iLevel][CHARACTER_DATA.CHAR_SKILL_DEMAGE.ToString()]);
    }
    public void Init(List<Dictionary<string, object>> ExcelTable, List<Dictionary<string, object>> ExcelClass, int Count)
    {
        NodeSetting(CHARACTER_DATA.CHAR_NAME, ExcelTable[Count][CHARACTER_DATA.CHAR_NAME.ToString()]);
        NodeSetting(CHARACTER_DATA.CHAR_LEVEL, ExcelTable[Count][CHARACTER_DATA.CHAR_LEVEL.ToString()]);
        int iLevel = Util.ConvertToInt(ExcelTable[Count][CHARACTER_DATA.CHAR_LEVEL.ToString()]) - 1;

        NodeSetting(CHARACTER_DATA.CHAR_MAX_HP, ExcelClass[iLevel][CHARACTER_DATA.CHAR_MAX_HP.ToString()]);
        //hp
        NodeSetting(CHARACTER_DATA.CHAR_MAX_EXP, ExcelTable[Count][CHARACTER_DATA.CHAR_MAX_EXP.ToString()]);
        //exp
        NodeSetting(CHARACTER_DATA.CHAR_ATK, ExcelClass[iLevel][CHARACTER_DATA.CHAR_ATK.ToString()]);
        //atk
        NodeSetting(CHARACTER_DATA.CHAR_SPEED, ExcelClass[iLevel][CHARACTER_DATA.CHAR_SPEED.ToString()]);
        //speed
        NodeSetting(CHARACTER_DATA.CHAR_CLASS_TYPE, ExcelTable[Count][CHARACTER_DATA.CHAR_CLASS_TYPE.ToString()]);
       //NodeSetting(CHARACTER_DATA.CHAR_SKILL_DEMAGE, ExcelTable[iLevel][CHARACTER_DATA.CHAR_SKILL_DEMAGE.ToString()]);
    }
    private void NodeSetting(CHARACTER_DATA eData, object Data)
    {
        m_CharStatus.Add(eData, Data);
    }

    /// <summary>
    /// 데이터 업데이트
    /// </summary>
    /// <param name="eData"></param>
    /// <param name="Data"></param>
    public void Update(CHARACTER_DATA eData, object Data)
    {
        m_CharStatus[eData] = Data;
    }

    public bool ifLevelUp(List<Dictionary<string, object>> ExcelClass, List<Dictionary<string, object>> ExcelEXP, List<Dictionary<string, object>> ExcelChar)
    {
        //레벨업 데이터랑 클래스 데이터
        int iCurEXP = Util.ConvertToInt(ReturnData(CHARACTER_DATA.CHAR_CUR_EXP));
        int iLevel = Util.ConvertToInt(ReturnData(CHARACTER_DATA.CHAR_LEVEL));
        int iTableLevel = 0;
        bool bLevelUp = false;

        while (true)
        {
            iTableLevel = iLevel - 1;
            int iMaxEXP = int.Parse(ExcelEXP[iTableLevel][CHARACTER_DATA.CHAR_MAX_EXP.ToString()].ToString());
            //현재 레벨당 최대 경험치 대비
            if (iCurEXP >= iMaxEXP)
            {
                iLevel++;
                iCurEXP -= iMaxEXP;
                bLevelUp = true;
            }
            else
                break;
        }

        iTableLevel = iLevel - 1;
        Update(CHARACTER_DATA.CHAR_LEVEL, iLevel);//현재 레벨
        int TmpData = Util.ConvertToInt(ExcelClass[iTableLevel][CHARACTER_DATA.CHAR_MAX_HP.ToString()]) +
            Util.ConvertToInt(ExcelChar[0][CHARACTER_DATA.CHAR_MAX_HP.ToString()]);
        Update(CHARACTER_DATA.CHAR_MAX_HP, TmpData); //현재 레벨 HP
        Update(CHARACTER_DATA.CHAR_MAX_EXP, ExcelEXP[iTableLevel][CHARACTER_DATA.CHAR_MAX_EXP.ToString()]); //현재 레벨 max EXP
        Update(CHARACTER_DATA.CHAR_CUR_EXP, iCurEXP); //현재 경험치
        TmpData = Util.ConvertToInt(ExcelClass[iTableLevel][CHARACTER_DATA.CHAR_ATK.ToString()]) +
            Util.ConvertToInt(ExcelChar[0][CHARACTER_DATA.CHAR_ATK.ToString()]);
        Update(CHARACTER_DATA.CHAR_ATK, TmpData); //현재 레벨ATK
        //레벨 업 체크
        return bLevelUp;
    }
    
    /// <summary>
    /// 리턴 데이터
    /// </summary>
    /// <param name="eData"></param>
    public object ReturnData(CHARACTER_DATA eData)
    {
        return m_CharStatus[eData];
    }

}
