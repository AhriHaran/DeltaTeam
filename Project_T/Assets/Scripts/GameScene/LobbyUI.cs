using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    private GameObject m_Lobby;

    private void Awake()
    {
        m_Lobby = transform.GetChild(0).gameObject;
        m_Lobby.SetActive(false);
    }

    public void OnHomeButton()
    {
        if (m_Lobby.activeSelf)
            m_Lobby.SetActive(false);
        else
            m_Lobby.SetActive(true);
        //패널껏다 켜주기
    }

    public void OnClick()
    {
        string Button = UIButton.current.name;

        if (Button == "Exit")
        {
            //게임 씬에서 뒤로 가는 것이므로 세이브 후 리턴
            UserInfo.instance.UserCompanionSave();
            UserInfo.instance.UserPartySave();
            LoadScene.SceneLoad("LobbyScene");//로비신으로
        }
        else if (Button == "None")
        {
            m_Lobby.SetActive(false);
        }
    }
}
