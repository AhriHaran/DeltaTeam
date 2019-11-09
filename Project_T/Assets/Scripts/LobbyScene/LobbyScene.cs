using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UI_PANEL
{
    PANEL_START,
    COMPANION_PANEL = PANEL_START,
    PARTY_PANEL,
    BATTLE_PANEL,
    PANEL_END,
    PANEL_HOME,
}

public class LobbyScene : MonoBehaviour
{
    private GameObject[]m_arrUIPanel = new GameObject[(int)UI_PANEL.PANEL_END];
    private GameObject m_ScrollPanel;
    private GameObject m_HomeButton;
    private GameObject m_ExitUI;
    private UI_PANEL m_ePanel;
    public UI_PANEL CurPanel
    {
        get { return m_ePanel; }
        set { m_ePanel = value; }
    }
    public UILabel m_UserName;

    private void Awake()
    {
        GameObject UI = GameObject.Find("UI Root");

        for(int i = (int)UI_PANEL.PANEL_START; i < (int)UI_PANEL.PANEL_END; i++)
        {
            int iIndex = i + 1;
            m_arrUIPanel[i] = UI.transform.GetChild(iIndex).gameObject;
        }
        //UI Panel;
        m_ScrollPanel = UI.transform.GetChild(4).gameObject;
        m_HomeButton = UI.transform.Find("Home").gameObject;

        CurPanel = UI_PANEL.PANEL_END;
        GameObject Exit = ResourceLoader.CreatePrefab("Prefabs/QuitUI");
        Exit.transform.SetParent(UI.transform, false);
        m_UserName.text = UserInfo.instance.UserName;
    }

    public void OnClick()
    {
        string Button = UIButton.current.name;

        if (Button == "Companion")
        {
            //동료창
            PanelOnOff(UI_PANEL.COMPANION_PANEL);
        }
        else if (Button == "Party")
        {
            //파티창
            PanelOnOff(UI_PANEL.PARTY_PANEL);
        }
        else if (Button == "Battle")
        {
            //배틀창
            PanelOnOff(UI_PANEL.BATTLE_PANEL);
        }
        else if(Button == "Home")
        {
            PanelOnOff(UI_PANEL.PANEL_HOME);
        }
        //uipanelOnOff
    }

    private void PanelOnOff(UI_PANEL eIndex)
    {
        if(eIndex != CurPanel)
        {
            if (eIndex == UI_PANEL.BATTLE_PANEL || eIndex == UI_PANEL.PANEL_HOME)
            {
                m_ScrollPanel.SetActive(false);
            }
            else
            {
                m_ScrollPanel.SetActive(true);
            }

            for (int i = (int)UI_PANEL.PANEL_START; i < (int)UI_PANEL.PANEL_END; i++)
            {
                if (i == (int)eIndex)
                {
                    m_arrUIPanel[i].SetActive(true);
                }
                else
                {
                    m_arrUIPanel[i].SetActive(false);
                }
            }

            if (eIndex != UI_PANEL.PANEL_HOME)
                m_HomeButton.SetActive(true);
            else
                m_HomeButton.SetActive(false);
            
            CurPanel = eIndex;
        }
    }

    public void DegeonClick()
    {
        if(CurPanel == UI_PANEL.BATTLE_PANEL)
        {
            string Button = UIButton.current.name;

            int iStage = Util.ConvertToInt(Button);
            
            GameManager.instance.ResetFiledData();  //필드 데이터 리셋
            
            GameManager.instance.StageIndex = iStage;
            GameManager.instance.BlockIndex = 0;
            //게임 씬 

            UserInfo.instance.UserPartyCopy();  //유저파티 카피
            
            //플레이어 파티를 복사
            GameManager.instance.MissionStatus = MISSION_STATUS.MISSION_START;

            var StageTabel = EXCEL.ExcelLoad.Read("Excel/Map_Table");
            GameManager.instance.MaxTime = float.Parse(Util.ConvertToString(StageTabel[iStage][MAP_TABLE_DATA.MAP_CLEAR_TIME.ToString()]));
            GameManager.instance.CurTime = GameManager.instance.MaxTime;
            //시간 셋팅
            
            string Route = "Excel/MapData/" + Util.ConvertToInt(iStage) + "/MapTable";
            //맵 테이블
            var MapTable = EXCEL.ExcelLoad.Read(Route);
            for (int i =0; i < MapTable.Count; i++)
            {
                BlockType Node = new BlockType();
                Node.m_iBlockIndex = i;
                Node.m_eBlocKClass = (MAP_BLOCK_CLASS)Util.EnumChange<MAP_BLOCK_CLASS>(MapTable[i][BLOCK_DATA.BLOCK_TYPE.ToString()].ToString());
                Node.m_bActive = true;
                GameManager.instance.BlockList.Add(Node);
            }

            LoadScene.SceneLoad("GameScene");
        }
    }
}
