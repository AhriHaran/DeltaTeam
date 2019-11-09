using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueScene : MonoBehaviour
{
    // Start is called before the first frame update

    private UISprite m_PrologueScne;
    private int m_iPrologueNumber;
    private bool m_bInput;
    private GameObject m_InputFiled;

    private void Awake()
    {
        m_PrologueScne = GameObject.Find("UI Root").transform.GetChild(1).GetComponent<UISprite>();
        m_iPrologueNumber = 0;
        m_InputFiled = GameObject.Find("UI Root").transform.GetChild(2).gameObject;
        m_InputFiled.SetActive(false);
    }

    private void Start()
    {
        m_PrologueScne.spriteName = "Prologue" + m_iPrologueNumber;
        m_bInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
    }
    private void InputKey()
    {
        if (m_bInput)
        {
            if (Input.anyKeyDown)
            {
                m_iPrologueNumber++;
                if (m_iPrologueNumber < 4)
                {
                    m_PrologueScne.spriteName = "Prologue" + m_iPrologueNumber;
                }
                else
                {
                    m_PrologueScne.spriteName = "LoadScene";
                    //바꾸고
                    m_InputFiled.SetActive(true);
                    m_bInput = false;
                    //이후부터는 그거다. 이름 입력
                }
            }
        }
    }

    public void OnClick()
    {
        string strValue = m_InputFiled.transform.GetChild(1).GetComponent<UIInput>().value;

        if(strValue.Length < 8 && strValue.Length > 1)
        {
            //8자리 미만이다.
            if (!JSON.JsonUtil.FileCheck("UserNameData")) //현재 파일이 있는가?
            {
                UserNameData Node = new UserNameData();
                Node.m_strUserName = strValue;
                string jsonData = JSON.JsonUtil.ToJson(Node);
                Debug.Log(jsonData);
                JSON.JsonUtil.CreateJson("UserNameData", jsonData);
            }

            if (!JSON.JsonUtil.FileCheck("UserData")) //현재 파일이 있는가?
            {
                //없으면 데이터 생성
                //메인 캐릭터 라벨
                CharData[] Node = new CharData[1];
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

            if (!JSON.JsonUtil.FileCheck("UserPartyData"))
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
                MapSaveData[] Node = new MapSaveData[3];

                for (int i = 0; i < 3; i++)
                {
                    Node[i] = new MapSaveData();
                    Node[i].m_bStageClear = false;
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
