using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionResult : MonoBehaviour
{
    public UISprite m_ResultSprite;
    public UILabel m_TimeLabel;
    public GameObject m_ImageButton;
    public GameObject m_ScrollObject;

    private UIPanel m_Panel;
    private UIScrollView m_ScrollView;
    private GameObject m_Grid;

    private MAP_MISSION m_eMission;
    private bool m_bCompanion;

    private void Awake()
    {
        m_Panel = m_ScrollObject.GetComponent<UIPanel>();
        m_ScrollView = m_ScrollObject.GetComponent<UIScrollView>();
        m_Grid = m_ScrollObject.transform.GetChild(0).gameObject;
    }

    public void Mission(MISSION_STATUS eStatus, MAP_MISSION eMission)
    {
        m_ScrollObject.SetActive(true);
        if (m_Grid.transform.childCount > 0) //그리드 초기화
        {
            while (m_Grid.transform.childCount != 0)
            {
                GameObject game = m_Grid.transform.GetChild(0).gameObject;
                game.transform.SetParent(null);
                NGUITools.Destroy(game);
            }
            m_Grid.transform.DetachChildren();
        }

        if (eStatus == MISSION_STATUS.MISSION_COMPLETE)
        {
            //미션 성공
            m_ImageButton.gameObject.SetActive(true);

            int iCOunt = GameManager.instance.PlayerParty.Count;

            m_eMission = eMission;
            m_bCompanion = false;

            for (int i = 0; i < iCOunt; i++)
            {
                GameObject CharInfo = ResourceLoader.CreatePrefab("Prefabs/UI/PlayerCard");
                CharInfo.transform.SetParent(m_Grid.transform, false);
                CharInfo.GetComponent<PlayerCard>().Setting(i);
            }
            //결과창 캐릭터 그리드 셋팅

            m_Grid.GetComponent<UIGrid>().Reposition(); //리 포지셔닝으로 그리드 재정렬
            m_ScrollView.ResetPosition();
            m_Panel.Refresh();

            float fTime = GameManager.instance.MaxTime;
            string strMax = Util.TimeCheck(fTime);
            fTime = GameManager.instance.CurTime;
            string strCur = Util.TimeCheck(fTime);

            m_TimeLabel.text = strCur + "/" + strMax;
        }
        else if (eStatus == MISSION_STATUS.MISSION_TIME_OUT)
        {
            m_ScrollObject.SetActive(false);
            m_TimeLabel.gameObject.SetActive(false);
            //타임 아웃
        }
        else if (eStatus == MISSION_STATUS.MISSION_FAILED)
        {
            m_ScrollObject.SetActive(false);

            float fTime = GameManager.instance.MaxTime;
            string strMax = Util.TimeCheck(fTime);
            fTime = GameManager.instance.CurTime;
            string strCur = Util.TimeCheck(fTime);

            m_TimeLabel.text = strCur + "/" + strMax;
        }

        m_ResultSprite.spriteName = eStatus.ToString();
        m_ImageButton.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void OnClick()
    {
        if (GameManager.instance.MissionStatus == MISSION_STATUS.MISSION_COMPLETE)
        {
            if (m_eMission == MAP_MISSION.RESCUE_MISSION)
            {
                //구출 미션 시에는 버튼을 누르고 난 뒤에 새로운 동료창을 업데이트
                if (!m_bCompanion)
                {
                    //현재         
                    if (m_Grid.transform.childCount > 0) //그리드 초기화
                    {
                        while (m_Grid.transform.childCount != 0)
                        {
                            GameObject game = m_Grid.transform.GetChild(0).gameObject;
                            game.transform.SetParent(null);
                            NGUITools.Destroy(game);
                        }
                        m_Grid.transform.DetachChildren();
                    }

                    GameObject Resque = ResourceLoader.CreatePrefab("Prefabs/UI/ResquePlayerCard");
                    Resque.transform.SetParent(m_Grid.transform, false);
                    Resque.GetComponent<PlayerResqueCard>().Setting(GameManager.instance.WhoRescueName, GameManager.instance.WhoRescueClass);

                    //컴패니언 정보 세이브

                    CharData Node = new CharData();
                    Node.m_CurEXP = 0;
                    Node.m_eClass = GameManager.instance.WhoRescueClass;
                    Node.m_iIndex = GameManager.instance.WhoRescueIndex;
                    Node.m_Level = 1;
                    UserInfo.instance.CompanionSetting(Node);
                    //동료 셋팅

                    m_bCompanion = true;
                }
                else
                {
                    //동료 패널 나오고 제 클릭시
                    //로비신으로\
                    UserInfo.instance.UserCompanionSave();
                    UserInfo.instance.UserPartySave();
                    //세이브 후 로비신으로
                    Time.timeScale = 1.0f;
                    LoadScene.SceneLoad("LobbyScene");
                }
            }
            else
            {   
                //로비신으로\
                UserInfo.instance.UserCompanionSave();
                UserInfo.instance.UserPartySave();
                //세이브 후 로비신으로
                Time.timeScale = 1.0f;
                LoadScene.SceneLoad("LobbyScene");
            }

        }
        else
        {
            //로비신으로\
            UserInfo.instance.UserCompanionSave();
            UserInfo.instance.UserPartySave();
            //세이브 후 로비신으로
            Time.timeScale = 1.0f;
            LoadScene.SceneLoad("LobbyScene");
        }
    }
}
