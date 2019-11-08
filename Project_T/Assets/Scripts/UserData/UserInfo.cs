using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : GSingleton<UserInfo>
{
    private List<CharacterData> m_CompanionList = new List<CharacterData>();
    public List<CharacterData> CompanionList
    {
        get { return m_CompanionList; }
        set { m_CompanionList = value; }
    }
    private List<CharPartyData> m_PartyList = new List<CharPartyData>();   //유저 파티 리스트
    public List<CharPartyData> PartyList   //로비에서 파티 데이터 용
    {
        get { return m_PartyList; }
        set { m_PartyList = value; }
    }//내부 데이터가 안변하는 리스트랑 게임 파티용 리스트 둘다 필요

    public void Init()
    {
        if (JSON.JsonUtil.FileCheck("UserData")) //유저 메인 캐릭터 JSON
        {
            CharData [] JsonData = JSON.JsonUtil.LoadArrJson<CharData>("UserData");
            for(int i = 0; i< JsonData.Length; i++)
            {
                CompanionSetting(JsonData[i]);
            }
        }
        //유저 동료 셋팅
    }

    public void CompanionSetting(CharData Data)
    {
        var ExcelEXP = EXCEL.ExcelLoad.Read("Excel/Class/CLASS_EXP");   //경험치 테이블(공용)
        string ExcelRoute = "Excel/Class/" + Data.m_eClass.ToString();
        var ExcelClass = EXCEL.ExcelLoad.Read(ExcelRoute);  //클래스 EXCEL
        ExcelRoute = "Excel/Character/" + Data.m_eClass.ToString() + Util.ConvertToString(Data.m_iIndex);
        var ExcelChar = EXCEL.ExcelLoad.Read(ExcelRoute);   //캐릭터 EXCEL
        CharacterData CharData = new CharacterData();
        CharData.Init(ExcelClass, ExcelEXP, ExcelChar, Data);
        CompanionList.Add(CharData);
    }
    
    public bool ifCompanionExist(CLASS eClass, int iIndex)
    {
        for(int i = 0; i < CompanionList.Count; i++)
        {
            CLASS CurClass = (CLASS)Util.EnumChange<CLASS>(Util.ConvertToString(CompanionList[i].ReturnData(CHARACTER_DATA.CHAR_CLASS_TYPE)));
            int CurIndex = Util.ConvertToInt(CompanionList[i].ReturnData(CHARACTER_DATA.CHAR_INDEX));
            if (CurClass == eClass && CurIndex == iIndex)
                return true;//해당 캐릭터가 존재함
        }
        //없음
        return false;
    }

    public void PartySetting()
    {
        if (JSON.JsonUtil.FileCheck("UserPartyData"))
        {
            CharPartyList JsonData = JSON.JsonUtil.LoadJson<CharPartyList>("UserPartyData");
            for (int i = 0; i < 4; i++)
            {
                if (JsonData.m_PartyListIndex[i] != -1)
                {
                    int iCount = JsonData.m_PartyListIndex[i];
                    //플레이어 파티 셋팅
                    PartySetting(iCount);
                }
            }
            //파티 데이터 셋팅(유저의 인덱스 순서)
        }
    }

    public void PartySetting(int iIndex)
    {
        if(iIndex < m_CompanionList.Count)
        {
            CharPartyData Node = new CharPartyData();
            PartySetting(Node, iIndex);
            PartyList.Add(Node);
        }
    }

    public void PartySetting(CharPartyData Node, int iIndex)
    {
        if (iIndex < m_CompanionList.Count)
        {
            Node.PartyIndex = iIndex;
            Node.CharType = CHARACTER_TYPE.CHAR_PLAYER;
            Node.CharData = CompanionList[iIndex];

            Node.MaxHP = float.Parse(Util.ConvertToString(CompanionList[iIndex].ReturnData(CHARACTER_DATA.CHAR_MAX_HP)));
            Node.CurHP = float.Parse(Util.ConvertToString(CompanionList[iIndex].ReturnData(CHARACTER_DATA.CHAR_MAX_HP)));

            Node.MaxStemina = 3;  //3칸으로 설정

            string[] arr = Util.ConvertToString(CompanionList[iIndex].ReturnData(CHARACTER_DATA.CHAR_SPEED)).Split(';');
            Node.FillStemina = Util.ConvertToInt(arr[0]);   //턴당
            Node.CurFillStemina = 1;    //캐릭터 현재턴
            Node.SteminaCurFill = Util.ConvertToInt(arr[1]);  //이러한 게이지가 찬다.
            Node.CurStemina = Node.SteminaCurFill;
            Node.Stay = false;
        }
    }



    public void UserPartySave()
    {
        //파티 데이터 셋팅
        CharPartyList Node = new CharPartyList();
        Node.m_PartyListIndex = new int[4];

        for(int i = 0; i < 4; i++)
        {
            if (i < PartyList.Count)
                Node.m_PartyListIndex[i] = PartyList[i].PartyIndex;
            else
                Node.m_PartyListIndex[i] = -1;
        }

        string jsonData = JSON.JsonUtil.ToJson(Node);
        Debug.Log(jsonData);
        JSON.JsonUtil.CreateJson("UserPartyData", jsonData);
        //플레이어 파티 리스트
    }

    public void PartySwapToCompanion(int iPartyIndex, int iCompanionIndex)
    {
        //1번 인수의 파티 캐릭터를 2번 인수의 컴패니언 리스트로 교체
        PartySetting(PartyList[iPartyIndex], iCompanionIndex);
    }

    public void PartySwapToParty(int iPartyIndex0, int iPartyIndex1)
    {
        //0번과 1번을 교환
        CharPartyData SwapTmp = new CharPartyData(PartyList[iPartyIndex0]);
        PartyList[iPartyIndex0] = PartyList[iPartyIndex1];
        PartyList[iPartyIndex1] = SwapTmp;
        //스왑
    }

    public void UserCompanionSave()
    {
        CharData[] JsonData = new CharData[CompanionList.Count];

        for(int i = 0; i < JsonData.Length; i++)
        {
            JsonData[i] = new CharData();
            JsonData[i].m_CurEXP = Util.ConvertToInt(CompanionList[i].ReturnData(CHARACTER_DATA.CHAR_CUR_EXP));
            JsonData[i].m_eClass = (CLASS)Util.EnumChange<CLASS>(CompanionList[i].ReturnData(CHARACTER_DATA.CHAR_CLASS_TYPE).ToString());
            JsonData[i].m_iIndex = Util.ConvertToInt(CompanionList[i].ReturnData(CHARACTER_DATA.CHAR_INDEX));
            JsonData[i].m_Level = Util.ConvertToInt(CompanionList[i].ReturnData(CHARACTER_DATA.CHAR_LEVEL));
        }

        string jsonData = JSON.JsonUtil.ToJson<CharData>(JsonData);
        Debug.Log(jsonData);
        JSON.JsonUtil.CreateJson("UserData", jsonData);
        //플레이어 JSON  데이터
    }

    public void UserPartyCopy()
    {
        //유저 파티 카피
        GameManager.instance.PlayerParty.Clear();
        int iMax = PartyList.Count;
        for (int i = 0; i < iMax; i++)
        {
            CharPartyData Node = new CharPartyData(PartyList[i]);
            GameManager.instance.PlayerParty.Add(Node);
        }
        //복사생성자로 카피   
    }
}
