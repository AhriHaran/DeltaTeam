using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BLOCK_DATA
{
    BLOCK_INDEX,          //블럭 인덱스(몇 번째 맵인가)
    BLOCK_LINKED_LIST,    //각 블럭이 연결된 방의 수
    BLOCK_CLASS,          //각 블럭의 클래스
    BLOCK_TYPE,           //각 블럭의 타입
}

public enum BLOCK_COMPONENT
{
    COMPONENT_AREA,
    COMPONENT_MARK,
    COMPONENT_HIT_POINT,
    COMPONENT_ENEMY_MANAGER,
    COMPONENT_SPOT_LIGHT,
}


//현재 방넘버,연결된방;,방속성;
public class MapManager : MonoBehaviour
{
    [Header("SpotLightSpeed")]
    public float m_fNormalSpeed;
    public float m_fNormalDelayTime;
    public float m_fDangerSpeed;
    public float m_fDangerDelayTime;

    [Header("UI")]
    public GameObject m_MiniMapUI;  //미니맵
    public GameObject m_ResqueObject;   //레스큐

    private List<BlockData> m_BlockList = new List<BlockData>();
    private GameObject m_BlockNode;
    private GameObject m_PlayerObject;

    /// <summary>
    /// 포인트 도달 체크
    /// </summary>
    private bool m_bEnd = false;
    private bool m_bCompanion = false;
    private bool m_bBoss = false;
    private GameObject m_MiniMap;
    //미니 맵도 여기서 관리
    
    private void Start()
    {
        //유저 맵 데이터 기반으로 맵 불러오기
        m_BlockNode = ResourceLoader.CreatePrefab("Prefabs/Map/Block");
        m_BlockNode.transform.SetParent(transform, false);
        m_PlayerObject = GameObject.Find("GameObject").transform.GetChild(1).transform.Find("Player").gameObject;
        //유저가 현재 선택한 맵 데이터 기반

        int iStageIndex = GameManager.instance.StageIndex;
        MapSaveData Node = JSON.JsonUtil.LoadJson<MapSaveData>("UserMapData");

        string Route = "Prefabs/Map/" + iStageIndex + "_StageMiniMap";
        m_MiniMap = ResourceLoader.CreatePrefab(Route);
        m_MiniMap.transform.SetParent(m_MiniMapUI.transform, false);
        //미니맵 셋팅

        MapSetting(iStageIndex);    //맵 데이터 셋팅  
        MapClassSetting(GameManager.instance.BlockIndex);   //각각의 맵의 블럭 클래스를 각각 설정

        EndPoint(false);
        GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_CONTINUE;
    }

    public void MapSetting(int iMapIndex)//현재 맵 셋팅
    {
        GameManager.instance.StageIndex = iMapIndex;
        //현재 맵 인덱스 기반
        string Route = "Excel/MapData/" + Util.ConvertToInt(iMapIndex) + "/MapTable";
        //맵 테이블
        var MapTable = EXCEL.ExcelLoad.Read(Route);

        for (int i = 0; i < MapTable.Count;i++)
        {
            string strClass = Util.ConvertToString(MapTable[i][BLOCK_DATA.BLOCK_CLASS.ToString()]);
            string strLinked = Util.ConvertToString(MapTable[i][BLOCK_DATA.BLOCK_LINKED_LIST.ToString()]);
            BlockData Node = new BlockData(strClass, strLinked);
            m_BlockList.Add(Node);
        }

        //현재 맵 데이터 셋팅
        Debug.Log("MapSetting");
        int iMax = m_BlockList.Count;
        for (int i = 0; i < iMax; i++)
        {
            m_MiniMap.transform.GetChild(i).GetComponent<BlockInput>().CallBackSetting(MapClassSetting);
        }//미니맵 콜백
        //미니맵 콜백
    }

    public void MapClassSetting(int iCurBlock)  //현재 맵의 클래스 셋팅(블록)
    {
        //현재 블럭에 맞는 클래스들을 각각 설정
        GameManager.instance.BlockIndex = iCurBlock;
        int iMax = m_BlockList[iCurBlock].BlockClass.Length;

        for(int i = 1; i < 4; i++)
        {
            //초기화 부분
            m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_AREA).GetComponent<AreaPoint>()?.CoroutineStart(false);//에리어 포인트 스탑
            m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_MARK).gameObject.SetActive(false);//마크 삭제
            if(i == 1 || i == 2)
            {
                //0->에리어 포인트 1->마크(Companion, boss), 2->스포트라이트 3-<히트포인트 4-<에너미매니저
                GameObject Node = m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_SPOT_LIGHT).gameObject;
                if (Node != null)
                {
                    Node.GetComponent<SpotLight>().CoroutineStart(false);   //코루틴 스탑
                    Node.SetActive(false);
                }

                Node = m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_HIT_POINT).gameObject;
                Node.SetActive(false);  //히트 포인트 체크

                Node = m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_ENEMY_MANAGER).gameObject;
                if (Node != null)
                {
                    Node.GetComponent<EnemyManager>().EnemyListClear();   //코루틴 스탑
                    Node.SetActive(false);
                }
            }
        }

        for (int i = 0; i < iMax; i++)
        {
            switch (m_BlockList[iCurBlock].BlockClass[i])
            {
                case MAP_BLOCK_CLASS.BLOCK_START:
                    float fX = 0.0f;
                    fX = m_BlockNode.transform.GetChild(i).transform.position.x;
                    //if (GameManager.instance.MissionStatus == MISSION_STATUS.MISSION_START ||
                    //    GameManager.instance.MissionStatus == MISSION_STATUS.MISSION_BATTLE_LOSE)
                    //{
                    //    //미션 스타트나, 배틀 루즈시 블럭의 처음 위치로
                    //    fX = m_BlockNode.transform.GetChild(i).transform.position.x;
                    //}
                    //else
                    //{
                    //    fX = GameManager.instance.CurXPosition;
                    //}
                    m_PlayerObject.SetActive(false);
                    m_PlayerObject.transform.position = new Vector3(fX, (float)1.6);
                    Camera.main.GetComponent<FollowCharacter>().m_PlayerTR = m_PlayerObject.transform;
                    break;
                case MAP_BLOCK_CLASS.BLOCK_MONSTER:
                    //해당 몬스터를 잡은 상태라면
                    //일반 몬스터는 노란색 1,1,0
                    //0->에리어 포인트 1->마크(Companion, boss), 2->스포트라이트 3-<히트포인트 4-<에너미매니저
                    if(GameManager.instance.BlockList[iCurBlock].m_bActive)
                    {
                        m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_SPOT_LIGHT)?.gameObject.SetActive(true);
                        m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_HIT_POINT)?.gameObject.SetActive(true);
                        SpotLight Spot = m_BlockNode.transform.GetChild(i).GetComponentInChildren<SpotLight>();

                        Spot.OnOffSpeed = m_fNormalSpeed;
                        Spot.OnOffTime = m_fNormalDelayTime;

                        Spot.CoroutineStart(true);
                        Spot.BlockIndex = i;    //블록 인덱스
                        Spot.ColorSetting(1.0f, 1.0f, 0.0f);
                        EnemyManager Enemy = m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_ENEMY_MANAGER).GetComponent<EnemyManager>();
                        Enemy.Init(GameManager.instance.StageIndex, GameManager.instance.BlockIndex, CHARACTER_TYPE.CHAR_ENEMY);
                        //해당 몬스터를 잡으면 더이 상 활성화 X
                    }
                    break;
                case MAP_BLOCK_CLASS.BLOCK_COMPANION:
                    if (GameManager.instance.BlockList[iCurBlock].m_bActive)
                    {
                        m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_MARK)?.gameObject.SetActive(true);
                        m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_AREA)?.GetComponent<AreaPoint>().CallBackSetting(CompanionBlock);   //엔드포인트 셋팅
                        m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_AREA)?.GetComponent<AreaPoint>().CoroutineStart(true);   //코루틴 스탑
                    }
                    //동료 블럭
                    break;
                case MAP_BLOCK_CLASS.BLOCK_BOSS:
                    if (GameManager.instance.BlockList[iCurBlock].m_bActive)
                    {
                        m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_MARK)?.gameObject.SetActive(true);
                        m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_HIT_POINT)?.gameObject.SetActive(true);
                        //히트 체크 계속 켜줌
                        
                        EnemyManager Enemy = m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_ENEMY_MANAGER).GetComponent<EnemyManager>();
                        Enemy.Init(GameManager.instance.StageIndex, GameManager.instance.BlockIndex, CHARACTER_TYPE.CHAR_BOSS);
                        //해당 몬스터를 잡으면 더이 상 활성화 X
                    }
                    //보스 블럭
                    break;
                case MAP_BLOCK_CLASS.BLOCK_DANGEROUS:
                    //일반 몬스터는 노란색 1,0,0
                    if (GameManager.instance.BlockList[iCurBlock].m_bActive)
                    {
                        m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_SPOT_LIGHT)?.gameObject.SetActive(true);
                        m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_HIT_POINT)?.gameObject.SetActive(true);

                        SpotLight Spot = m_BlockNode.transform.GetChild(i).GetComponentInChildren<SpotLight>();

                        Spot.OnOffSpeed = m_fDangerSpeed;
                        Spot.OnOffTime = m_fDangerDelayTime;

                        Spot.CoroutineStart(true);
                        Spot.BlockIndex = i;    //블록 인덱스
                        Spot.ColorSetting(1.0f, 0.0f, 0.0f);

                        EnemyManager Enemy = m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_ENEMY_MANAGER).GetComponent<EnemyManager>();
                        Enemy.Init(GameManager.instance.StageIndex, GameManager.instance.BlockIndex, CHARACTER_TYPE.CHAR_DANGEROUS);
                        //해당 몬스터를 잡으면 더이 상 활성화 X
                    }
                    break;
                case MAP_BLOCK_CLASS.BLOCK_END:
                    m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_AREA).GetComponent<AreaPoint>().CallBackSetting(EndPoint);   //엔드포인트 셋팅
                    m_BlockNode.transform.GetChild(i).transform.GetChild((int)BLOCK_COMPONENT.COMPONENT_AREA).GetComponent<AreaPoint>().CoroutineStart(true);   //코루틴 스탑
                    break;
            }
        }

        m_PlayerObject.SetActive(true);
        for (int i =0; i < m_BlockList.Count; i++)
        {
            if(i == iCurBlock)
                m_MiniMap.transform.GetChild(i).GetComponent<BlockInput>().CurPlayer(true);
            else
                m_MiniMap.transform.GetChild(i).GetComponent<BlockInput>().CurPlayer(false);
        }
        //그리고 미니맵에 현재 위치 표시
    }

    public void CompanionBlock(bool bPoint)
    {
        //플레이어가 엔드 포인트에 도달하면 이쪽으로
        if (bPoint)
        {
            //플레이어가 컴패니언 블럭에 도달
            if(m_bCompanion)
            {
                //해당 블럭의 컴패니언파일을 작성해서 새롭게 업데이트
                if(GameManager.instance.BlockList[GameManager.instance.BlockIndex].m_bActive)
                {
                    //우선 유저가 해당 컴패니언을 가지고 있는가와 더불어서, 해당 패널을 밟으면 GameManager에 해당 패널을 밟았다는 것을 표시
                    
                    //유저에게 해당 클래스의 컴패니언이 있는가?
                    string Route = "Excel/MapData/" + GameManager.instance.StageIndex + "/" + GameManager.instance.BlockIndex +"_" + CHARACTER_TYPE.CHAR_PLAYER.ToString();
                    var ComTable = EXCEL.ExcelLoad.Read(Route);
                    CLASS eClass = (CLASS)Util.EnumChange<CLASS>(Util.ConvertToString(ComTable[0][CHARACTER_DATA.CHAR_CLASS_TYPE.ToString()]));
                    int iIndex = Util.ConvertToInt(ComTable[0][CHARACTER_DATA.CHAR_INDEX.ToString()]);
                    string strName = Util.ConvertToString(ComTable[0][CHARACTER_DATA.CHAR_NAME.ToString()]);
                    //맵 데이터 셋팅
                    if (!UserInfo.instance.ifCompanionExist(eClass, iIndex))
                    {
                        //존재하는가?
                        //존재하지 않는다면, 보상으로서 획득가능 하니, 게임 매니저에 해당 캐릭터의 인덱스의 기타 등등을 저장해놓자.
                        GameManager.instance.WhoRescueIndex = iIndex;   //해당 블럭에서 구출 가능한 인덱스
                        GameManager.instance.WhoRescueClass = eClass;   //해당 블럭에서 구출 가능한 클래스
                        Debug.Log("CompanionSave");
                        Debug.Log(iIndex + eClass + strName);

                        m_ResqueObject.SetActive(true);
                        m_ResqueObject.GetComponent<ResqueObject>().SettingText(strName);
                    }
                    GameManager.instance.ifRescue = true;   //구출 완료
                    Debug.Log("Companion");
                    GameManager.instance.BlockList[GameManager.instance.BlockIndex].m_bActive = false;
                }
                m_bCompanion = false;
            }
        }
        else
        {
            //플레이어가 엔드 포인트에 도달 못함
            //미니맵 선택을 막아주고 전체적으로 리셋
            m_bCompanion = true;
        }
    }


    public void EndPoint(bool bPoint)
    {
        //플레이어가 엔드 포인트에 도달하면 이쪽으로
        if(bPoint)
        {
            //플레이어가 엔드 포인트에 도달
            //현재 위치를 토대로 미니맵에서 갈수 있는 블럭을 표시
            if(m_bEnd)
            {
                int iMax = m_BlockList.Count;
                int iBlockIndex = GameManager.instance.BlockIndex;
                
                int[] iarr = m_BlockList[iBlockIndex].BlockLinkedList;
                for (int i = 0; i < iMax; i++)
                    m_MiniMap.transform.GetChild(i).GetComponent<BlockInput>().SelectOK(false);

                for (int i = 0; i < iarr.Length; i++)
                    m_MiniMap.transform.GetChild(iarr[i]).GetComponent<BlockInput>().SelectOK(true);

                Debug.Log("EndPoint");
                m_bEnd = false;
            }
        }
        else
        {
            //플레이어가 엔드 포인트에 도달 못함
            //미니맵 선택을 막아주고 전체적으로 리셋
            m_bEnd = true;
            int iMax = m_BlockList.Count;
            for (int i = 0; i < iMax; i++)
                m_MiniMap.transform.GetChild(i).GetComponent<BlockInput>().SelectOK(false);
        }
    }



}
