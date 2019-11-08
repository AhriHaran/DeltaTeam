using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour
{
    public UILabel m_UILabel;
    private GameObject m_ExitUI;
    private int m_iBlink = 255;
    private int m_iAlpha = 5;

    private void Start()
    {
        StartCoroutine("LabelBlink");
    }

    IEnumerator LabelBlink()
    {
        while(true)
        {
            if (m_iBlink == 255)
                m_iAlpha = -5;
            else if (m_iBlink == 0)
                m_iAlpha = 5;

            m_iBlink += m_iAlpha;
            float Alpha = ((float)(m_iBlink) / 255.0f);
            m_UILabel.color = new Color(m_UILabel.color.r, m_UILabel.color.g, m_UILabel.color.b, Alpha);
            yield return null;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        OnClick();
    }

    void OnClick()
    {
        if (Input.anyKey)
        {
            if (!JSON.JsonUtil.FileCheck("UserData")) //현재 파일이 있는가?
            {
                //없으면 데이터 생성
                //메인 캐릭터 라벨
                CharData [] Node = new CharData[1];
                Node[0] = new CharData();
                Node[0].m_Level = 1;
                Node[0].m_iIndex = 0;
                Node[0].m_CurEXP = 0;
                Node[0].m_eClass = CLASS.CLASS_THIEF;

                string jsonData = JSON.JsonUtil.ToJson<CharData>(Node);
                Debug.Log(jsonData);
                JSON.JsonUtil.CreateJson("UserData", jsonData);
                //플레이어 캐릭터 JSON
            }

            if(!JSON.JsonUtil.FileCheck("UserPartyData"))
            {
                CharPartyList Node = new CharPartyList();
                Node.m_PartyListIndex = new int[4];
                Node.m_PartyListIndex[0] = 0;
                Node.m_PartyListIndex[1] = -1;
                Node.m_PartyListIndex[2] = -1;
                Node.m_PartyListIndex[3] = -1;

                string jsonData = JSON.JsonUtil.ToJson(Node);
                Debug.Log(jsonData);
                JSON.JsonUtil.CreateJson("UserPartyData", jsonData);
                //플레이어 캐릭터 JSON
            }

            if (!JSON.JsonUtil.FileCheck("UserMapData"))    //유저 맵 데이터
            {
                MapSaveData [] Node = new MapSaveData[3];

                for(int i = 0; i < 3; i++)
                {
                    Node[i].m_iClearStage = -1;
                    Node[i].m_iClearStar = -1;
                }
                //현재는 맵이 세가지

                string jsonData = JSON.JsonUtil.ToJson<MapSaveData>(Node);
                Debug.Log(jsonData);
                JSON.JsonUtil.CreateJson("UserMapData", jsonData);
            }

            UserInfo.instance.Init();
            UserInfo.instance.PartySetting();
            GameManager.instance.Init();

            //플레이어 캐릭터 정보가 존재한다면 로딩
            LoadScene.SceneLoad("LobbyScene");
            //게임 씬 로딩
        }
    }
}
