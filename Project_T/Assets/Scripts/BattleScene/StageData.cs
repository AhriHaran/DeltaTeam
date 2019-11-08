using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MAP_TABLE_DATA
{
    MAP_INDEX,          //맵 인덱스
    MAP_CLEAR_TIME,     //맵 클리어 타임(맥스타임)
    MAP_MISSION,        //맵 미션
    MAP_STAR,           //맵 별 조건(시간)
    MAP_TIME_UP_NORMAL,        //맵 타임 업 조건(일반 몹, 정예몹)
    MAP_TIME_UP_DANGER,
    MAP_TIME_DOWN_NORMAL,
    MAP_TIME_DOWN_DANGER,
}

public enum MAP_MISSION
{
    RESCUE_MISSION,
    NONE_MISSION,
}

public class StageData
{
    private MAP_MISSION m_eMission;
    public MAP_MISSION StageMission
    {
        get { return m_eMission; }
        set { m_eMission = value; }
    }

    private float m_fStageTime;
    public float StageTime
    {
        get { return m_fStageTime; }
        set { m_fStageTime = value; }
    }

    private float[] m_StarCase = new float[3];  //별조건
    public float[] StageStar
    {
        get { return m_StarCase; }
        set { m_StarCase = value; }
    }
    
    private float m_NormalTimeUP;   //노멀 타임 업 조건
    public float StageNormalTimeUP
    {
        get { return m_NormalTimeUP; }
        set { m_NormalTimeUP = value; }
    }
    private float m_DangerTimeUP;   //데인저러스 타임 업 조건
    public float StageDangerTimeUP
    {
        get { return m_DangerTimeUP; }
        set { m_DangerTimeUP = value; }
    }

    private float m_NormalTimeDown; //노멀 타임 다운 조건
    public float StageNormalTimeDown
    {
        get { return m_NormalTimeDown; }
        set { m_NormalTimeDown = value; }
    }
    private float m_DangerTimeDown; //데인저러스 타임 다운 조건
    public float StageDangerTimeDown
    {
        get { return m_DangerTimeDown; }
        set { m_DangerTimeDown = value; }
    }

    public void Init(List<Dictionary<string,object>> MapTable, int iIndex)
    {
        StageTime = float.Parse(MapTable[iIndex][MAP_TABLE_DATA.MAP_CLEAR_TIME.ToString()].ToString());

        //게임 시간 설정
        StageMission = (MAP_MISSION)Util.EnumChange<MAP_MISSION>(Util.ConvertToString(MapTable[iIndex][MAP_TABLE_DATA.MAP_MISSION.ToString()]));
        //미션 설정

        string[] splitdata = Util.ConvertToString(MapTable[iIndex][MAP_TABLE_DATA.MAP_STAR.ToString()]).Split(';');
        for(int i = 0; i < 3; i++)
        {
            StageStar[i] = float.Parse(splitdata[i]);
        }

        //별 조건 셋팅
        StageNormalTimeUP = float.Parse(Util.ConvertToString(MapTable[iIndex][MAP_TABLE_DATA.MAP_TIME_UP_NORMAL.ToString()]));
        StageDangerTimeUP = float.Parse(Util.ConvertToString(MapTable[iIndex][MAP_TABLE_DATA.MAP_TIME_UP_DANGER.ToString()]));
        //시간 오르는 조건

        StageNormalTimeDown = float.Parse(Util.ConvertToString(MapTable[iIndex][MAP_TABLE_DATA.MAP_TIME_DOWN_NORMAL.ToString()]));
        StageDangerTimeDown = float.Parse(Util.ConvertToString(MapTable[iIndex][MAP_TABLE_DATA.MAP_TIME_DOWN_DANGER.ToString()]));
        //시간 다운 되는 조건
    }
}
