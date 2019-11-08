using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_PHASE
{
    PHASE_FILED,            //필드에서 돌아다님
    PHASE_TURN_START,       //턴 시작 = 현재 공격 가능한 캐릭터가 있는 지 확인
    PHASE_TARGET_SELECT,    //타겟 셀렉트 = 타겟 설정 가능, 여기서 스킬을 선택하면 다음 페이즈
    PHASE_ATTACK,           //타겟 어택 = 타겟 어택 애니메이션 수행
    PHASE_ATTACK_END,       //어택 엔드
    PHASE_TURN_END,         //턴 엔드 = 턴을 상승시키고, 더 공격 가능한 캐릭터가 있는 지 확인
    PHASE_RESULT_WIN,
    PHASE_RESULT_LOSE,
}

public class GameManager : MonoSingleton<GameManager>
{
    /// <summary>
    /// 파티 관련 인자
    /// </summary>
    private List<CharPartyData> m_PlayerParty = new List<CharPartyData>();
    //파티 캐릭터 리스트(최대 4개) //0번째는 주인공
    private List<CharPartyData> m_EnemyParty = new List<CharPartyData>();
    //에너미 파티 리스트(최대 4개)
    public List<CharPartyData> PlayerParty
    {
        get {return m_PlayerParty;}
        set {m_PlayerParty = value;}
    }
    public List<CharPartyData> EnemyParty
    {
        get {return m_EnemyParty;}
        set {m_EnemyParty = value;}
    }

    /// <summary>
    /// 턴과 선공, 공격 타입에 대한 인자
    /// </summary>
    private int m_GlobalTurn;   //현재 턴
    public int GlobalTurn
    {
        get { return m_GlobalTurn; }
        set { m_GlobalTurn = value; }
    }
    private CHARACTER_TYPE m_FirstAttack;  //선공자
    public CHARACTER_TYPE FirstAttacker
    {
        get { return m_FirstAttack; }
        set { m_FirstAttack = value; }
    }

    /// <summary>
    /// 현재 때리는 타입(공격자)
    /// </summary>
    private CHARACTER_TYPE m_AttakerType;  //어떤 타입이 때리는가?
    public CHARACTER_TYPE WhoAttakerType
    {
        get { return m_AttakerType; }
        set { m_AttakerType = value; }
    }
    private int m_AttakerIndex;             //누가 공격하는가(파티 인덱스)
    public int WhoAttackerIndex
    {
        get { return m_AttakerIndex; }
        set { m_AttakerIndex = value; }
    }
    private CHARACTER_SKILL_TYPE m_AttackerSkillType;
    public CHARACTER_SKILL_TYPE HowAttacker //어떻게 공격하는가
    {
        get { return m_AttackerSkillType; }
        set { m_AttackerSkillType = value; }
    }
    private bool m_bAttackAnimeEnd;
    public bool AttackAnimeEnd
    {
        get { return m_bAttackAnimeEnd; }
        set { m_bAttackAnimeEnd = value; }
    }

    /// <summary>
    /// 현재 맞는 캐릭터
    /// </summary>
    private CHARACTER_TYPE m_TargetType;  //어떤 타입이 맞는가?
    public CHARACTER_TYPE WhoTargetType
    {
        get { return m_TargetType; }
        set { m_TargetType = value; }
    }
    private int m_TargetIndex;
    public int WhoTargetIndex    //누가 타겟인가
    {
        get { return m_TargetIndex; }
        set { m_TargetIndex = value; }
    }
    private bool m_bTargetAnimeEnd;
    public bool TargetAnimeEnd
    {
        get { return m_bTargetAnimeEnd; }
        set { m_bTargetAnimeEnd = value; }
    }

    /// <summary>
    /// 누가 죽었는가?
    /// </summary>
    private CHARACTER_TYPE m_DeathType;  //어떤 타입이 맞는가?
    public CHARACTER_TYPE WhoDeathType
    {
        get { return m_DeathType; }
        set { m_DeathType = value; }
    }
    private int m_DeathIndex;
    public int WhoDeathIndex    //누가 타겟인가
    {
        get { return m_DeathIndex; }
        set { m_DeathIndex = value; }
    }

    /// <summary>
    /// 현재 페이즈 관련 함수, 게임의 진행에 관련됨
    /// </summary>
    private GAME_PHASE m_ePhase;
    public GAME_PHASE WhatPhase
    {
        get { return m_ePhase; }
        set { m_ePhase = value; }
    }

    /// <summary>
    /// 캐릭터 현재위치
    /// </summary>
    private float m_CurXPos;
    public float CurXPosition
    {
        get { return m_CurXPos; }
        set { m_CurXPos = value; }
    }

    /// <summary>
    /// 현재 시간
    /// </summary>
    private float m_fMaxTime;
    public float MaxTime
    {
        get { return m_fMaxTime; }
        set { m_fMaxTime = value; }
    }
    private float m_fCurTime;
    public float CurTime
    {
        get { return m_fCurTime; }
        set { m_fCurTime = value; }
    }

    /// <summary>
    /// 스테이지 관련인수
    /// </summary>
    private int m_iStageIndex;
    public int StageIndex
    {
        get { return m_iStageIndex; }
        set { m_iStageIndex = value; }
    }
    private int m_iBlockIndex;
    public int BlockIndex
    {
        get { return m_iBlockIndex; }
        set { m_iBlockIndex = value; }
    }
    private List<BlockType> m_BlockType = new List<BlockType>(); //몬스터가 있는 블럭 
    public List<BlockType> BlockList
    {
        get { return m_BlockType; }
        set { m_BlockType = value; }
    }

    /// <summary>
    /// 누구랑 싸우는가?
    /// </summary>
    private CHARACTER_TYPE m_eBattleType;   
    public CHARACTER_TYPE WhoBattleType
    {
        get { return m_eBattleType; }
        set { m_eBattleType = value; }
    }
    
    /// <summary>
    /// 미션 여부
    /// </summary>
    private MISSION_STATUS m_eMissionStatus;    //미션여부
    public MISSION_STATUS MissionStatus
    {
        get { return m_eMissionStatus; }
        set { m_eMissionStatus = value; }
    }
    private bool m_bRescue; //구출 하였는가?
    public bool ifRescue
    {
        get { return m_bRescue; }
        set { m_bRescue = value; }
    }
    private int m_iRescueIndex; //구출하려는 동료 인덱스
    public int WhoRescueIndex
    {
        get { return m_iRescueIndex; }
        set { m_iRescueIndex = value; }
    }
    private CLASS  m_eRescueClass; //구출하려는 동료 클래스
    public CLASS WhoRescueClass
    {
        get { return m_eRescueClass; }
        set { m_eRescueClass = value; }
    }
    private string m_strRescueName;
    public string WhoRescueName
    {
        get { return m_strRescueName; }
        set { m_strRescueName = value; }
    }

    /// <summary>
    /// 경험치
    /// </summary>
    private int m_iEnemyEXP;
    public int EnemyEXP
    {
        get { return m_iEnemyEXP; }
        set { m_iEnemyEXP = value; }
    }
    
    /// <summary>
    /// 초기화
    /// </summary>
    public void Init()
    {
        GlobalTurn = 1;
        ResetBattleData();
        ResetFiledData();
    }

    public void ResetFiledData()
    {
        StageIndex = -1;
        BlockIndex = -1;
        BlockList.Clear();
        MaxTime = 0.0f;

        WhoBattleType = CHARACTER_TYPE.CHAR_NONE;
        MissionStatus = MISSION_STATUS.MISSION_START;
        ifRescue = false;
        WhoRescueIndex = -1;
        WhoRescueClass = CLASS.CLASS_NONE;
    }

    public void ResetBattleData()
    {
        WhoAttackerIndex = -1;  //누가(인덱스)
        WhoAttakerType = CHARACTER_TYPE.CHAR_NONE;  //누가(타입)
        HowAttacker = CHARACTER_SKILL_TYPE.CHAR_SKILL_NONE; //어떻게(스킬 타입)

        WhoTargetIndex = -1;    //누구를(인덱스)
        WhoTargetType = CHARACTER_TYPE.CHAR_NONE;   //누구를(타입)

        WhoDeathIndex = -1;
        WhoDeathType = CHARACTER_TYPE.CHAR_NONE;    

        WhatPhase = GAME_PHASE.PHASE_TURN_START;    //현재 페이즈
    }
}
