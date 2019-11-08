using System.Collections;
using UnityEngine;

public enum MISSION_STATUS
{
    MISSION_START,          //초기 미션 스타트
    MISSION_CONTINUE,       //미션 컨티뉴
    MISSION_FAILED,         //미션 실패
    MISSION_COMPLETE,       //미션 성공
    MISSION_TIME_OUT,       //타임아웃
    MISSION_BATTLE_WIN,     //배틀 윈
    MISSION_BATTLE_LOSE,    //배틀 루즈
    MISSION_BOSS_WIN,       //보스 윈
}

/*게임 필드에서 돌아다니다가, 배틀 씬으로 넘어가서, 배틀씬의 승패 조건에 따라서 시간이 늘어나거나 줄어들고
 * 게임 패배 조건에 따라서 패배하고
 * 게임 승리 조건에 따라서 승리한다.
 */

/// <summary>
/// 맵 별로 시간, 스포트 라이트 시간,
/// </summary>
/// 
/// <summary>
/// 여기서 스테이지 클리어 조건을 저장한다.
/// </summary>


public class GameScene : MonoBehaviour
{
    public UILabel m_TimeLabel;
    public GameObject m_Result;

    private StageData m_StageData = new StageData();
    
    private void Awake()
    {
        var StageTable = EXCEL.ExcelLoad.Read("Excel/Map_Table");
        int iStage = GameManager.instance.StageIndex;
        m_StageData.Init(StageTable, iStage);
        //스테이지 데이터 셋팅
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //플레이어 파티 셋팅 이벤트 콜
        //맵 세팅
        ////에너미 임시 데이터 셋팅
        StartCoroutine("TimeCheck");
        StartCoroutine("MissionStateCheck");
    }

    IEnumerator MissionStateCheck()
    {
        //게임 미션 체크
        while(true)
        {
            switch (GameManager.instance.MissionStatus)
            {
                case MISSION_STATUS.MISSION_BATTLE_WIN:
                    float UpTime = 0.0f;
                    if (GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_ENEMY)
                        UpTime = m_StageData.StageNormalTimeUP;
                    else if(GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_DANGEROUS)
                        UpTime = m_StageData.StageDangerTimeUP;
                    GameManager.instance.CurTime += UpTime;
                    //업타임
                    break;
                case MISSION_STATUS.MISSION_BATTLE_LOSE:
                    float DownTime = 0.0f;
                    if (GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_ENEMY)
                        DownTime = m_StageData.StageNormalTimeDown;
                    else if (GameManager.instance.WhoBattleType == CHARACTER_TYPE.CHAR_DANGEROUS)
                        DownTime = m_StageData.StageDangerTimeDown;
                    GameManager.instance.CurTime -= DownTime;
                    //다운 타임
                    break;
                case MISSION_STATUS.MISSION_COMPLETE:
                    //미션 성공(패널 띄워줌)
                    m_Result.gameObject.SetActive(true);
                    m_Result.GetComponent<MissionResult>().Mission(MISSION_STATUS.MISSION_COMPLETE, m_StageData.StageMission);
                    StartCoroutine("TimeCheck");
                    break;
                case MISSION_STATUS.MISSION_FAILED:
                    m_Result.gameObject.SetActive(true);
                    m_Result.GetComponent<MissionResult>().Mission(MISSION_STATUS.MISSION_FAILED, m_StageData.StageMission);
                    StartCoroutine("TimeCheck");
                    //미션 실패
                    break;
                case MISSION_STATUS.MISSION_TIME_OUT:
                    m_Result.gameObject.SetActive(true);
                    m_Result.GetComponent<MissionResult>().Mission(MISSION_STATUS.MISSION_TIME_OUT, m_StageData.StageMission);
                    StartCoroutine("TimeCheck");
                    break;
                case MISSION_STATUS.MISSION_BOSS_WIN:
                    //보스 킬
                    if (m_StageData.StageMission == MAP_MISSION.NONE_MISSION)
                        GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_COMPLETE;
                    else if(m_StageData.StageMission == MAP_MISSION.RESCUE_MISSION ) //구출 미션
                    {
                        if (GameManager.instance.ifRescue)
                            GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_COMPLETE;
                        else
                            GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_FAILED;
                    }
                    //보스 킬을 하면 우선 결과가 뜬다.
                    StartCoroutine("TimeCheck");
                    break;
            }
            yield return new WaitForSeconds(0.8f);
        }
    }


    IEnumerator TimeCheck()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            
            float fTime = GameManager.instance.CurTime;
            fTime -= 1.0f;

            if (fTime <= 0.0f)
                fTime = 0.0f;
            m_TimeLabel.text = Util.TimeCheck(fTime);

            if (fTime <= 0.0f)
            {
                //게임 오버
                GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_TIME_OUT;
                fTime = 0.0f;
                //게임 리셋?
                StopCoroutine("TimeCheck");
            }
            GameManager.instance.CurTime = fTime;
        }
    }
}
