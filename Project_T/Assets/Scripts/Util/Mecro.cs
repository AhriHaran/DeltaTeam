using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CHARACTER_TYPE     //캐릭터 타입
{
    CHAR_PLAYER,
    CHAR_ENEMY,
    CHAR_DANGEROUS,
    CHAR_BOSS,
    CHAR_NONE,
}

public enum CHARACTER_SKILL_TYPE
{
    CHAR_ATTACK,    //기본공격
    CHAR_SKILL,     //스킬 공격
    CHAR_STAY,
    CHAR_SKILL_NONE,    //초기
}

[System.Serializable]
public class CharData               //캐릭터 데이터(JSON)
{
    public int m_Level;             //캐릭터 레벨
    public int m_iIndex;            //캐릭터 인덱스
    public int m_CurEXP;            //캐릭터 현재 EXP
    public CLASS m_eClass;          //캐릭터 클래스
}

[System.Serializable]
public class CharPartyList
{
    //파티 리스트, 인덱스 순서
    public int[] m_PartyListIndex;
}

[System.Serializable]
public class UserNameData
{
    public string m_strUserName;
}

[System.Serializable]
public class MapSaveData
{
    public int m_iClearStage;
    public int m_iClearStar;
}

[System.Serializable]
public class CharPartyData                 //파티 구성용 데이터(구조체)
{
    public int PartyIndex
    {
        get { return this.m_iPartyIndex; }
        set { this.m_iPartyIndex = value; }
    }
    //파티 번호
    public CHARACTER_TYPE CharType
    {
        get { return this.m_eType; }
        set { this.m_eType = value; }
    }
    //캐릭터 타입
    public CharacterData CharData
    {
        get { return this.m_CharData; }
        set { m_CharData = value; }
    }

    public float MaxHP
    {
        get { return this.m_fMaxHP; }
        set { this.m_fMaxHP = value; }
    }
    //캐릭터 Max HP
    public float CurHP
    {
        get { return this.m_fCurHP; }
        set { this.m_fCurHP = value; }
    }
    //캐릭터 Max HP

    public int MaxStemina
    {
        get { return this.m_iMaxStemina; }
        set { this.m_iMaxStemina = value; }
    }   //맥스 스테미너
    //캐릭터 Max 스피드
    public int CurStemina
    {
        get { return this.m_iCurStemina; }
        set { this.m_iCurStemina = value; }
    }   //현재 스테미너
    //캐릭터 cur 스피드

    public int FillStemina
    {
        get { return this.m_iFillStemina; }
        set { this.m_iFillStemina = value; }
    }   //캐릭터 차는 스테미너(몇 턴 당)
    //캐릭터 차는 스테미나
    public int CurFillStemina
    {
        get { return this.m_iCurFillStemina; }
        set { this.m_iCurFillStemina = value; }
    }   //현재 캐릭터 턴
    //캐릭터현재 턴
    public int SteminaCurFill
    {
        get { return this.m_iSteminaFill; }
        set { this.m_iSteminaFill = value; }
    }   //얼마나 차는가?
    //해당 턴이 되면 얼마나 차는가? 

    public bool Stay
    {
        get { return m_bStay; }
        set { m_bStay = value; }
    }

    private int              m_iPartyIndex;  //캐릭터 인덱스
    private CHARACTER_TYPE   m_eType;        //캐릭터 타입
    private CharacterData    m_CharData;        //캐릭터 데이터

    private float            m_fMaxHP;       //캐릭터 MAX HP
    private float            m_fCurHP;       //캐릭터 Cur HP

    private int              m_iMaxStemina;    //캐릭터 스피드 맥스 스피드
    private int              m_iCurStemina;    //캐릭터 현재 속도

    private int              m_iFillStemina;    //몇 턴 당 차는가
    private int              m_iCurFillStemina; //현재 턴
    private int              m_iSteminaFill;   //해당 턴이 되면 얼마나 차는가?
    private bool             m_bStay;   //스테이

    public CharPartyData() { }
    public CharPartyData(CharPartyData Node)
    {
        //복사 생성자
        this.PartyIndex = Node.PartyIndex;
        this.CharType = Node.CharType;
        this.CharData = Node.CharData;

        this.MaxHP = Node.MaxHP;
        this.CurHP = Node.CurHP;

        this.MaxStemina = Node.MaxStemina;
        this.CurStemina = Node.CurStemina;

        this.FillStemina = Node.FillStemina;
        this.CurFillStemina = Node.CurFillStemina;
        this.SteminaCurFill = Node.SteminaCurFill;
        this.Stay = Node.Stay;
    }
}

public class BlockType
{
    public int m_iBlockIndex;
    public MAP_BLOCK_CLASS m_eBlocKClass;
    public bool m_bActive;
}

public delegate void CallBackInt(int iIndex);  //버튼 클릭 콜백
public delegate void CallBackBool(bool bPoint);
public delegate void CallBackUI(CHARACTER_TYPE eType, int iIndex);