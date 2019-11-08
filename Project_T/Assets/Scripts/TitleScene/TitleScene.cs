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
            if(!JSON.JsonUtil.FileCheck("UserNameData"))
            {
                LoadScene.SceneLoad("PrologueScene");   //프롤로그 씬으로 로딩
            }
            else
            {
                //모든 데이터가 존재한다.
                UserInfo.instance.Init();
                UserInfo.instance.PartySetting();
                GameManager.instance.Init();

                //플레이어 캐릭터 정보가 존재한다면 로딩
                LoadScene.SceneLoad("LobbyScene");
                //게임 씬 로딩
            }
        }
    }
}
