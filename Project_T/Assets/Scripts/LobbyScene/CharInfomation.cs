using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfomation : MonoBehaviour
{
    private UISprite m_CharSprite;
    private UILabel m_CharName;
    private UILabel m_CharStatus;
    private CallBackInt m_OnClick;
    private int m_iIndex;   //컴패니언 리스트 순
    private bool m_bClick;
    private bool m_bDrag;
    private GameObject m_PartySprite;

    private void Awake()
    {
        m_CharSprite = transform.GetChild(0).GetComponent<UISprite>();
        m_CharName = transform.GetChild(1).GetComponent<UILabel>();
        m_CharStatus = transform.GetChild(2).GetComponent<UILabel>();

        m_PartySprite = ResourceLoader.CreatePrefab("Prefabs/UI/PartySprite");
    }

    public void Setting(int iIndex)
    {
        m_iIndex = iIndex;
        string Name = Util.ConvertToString(UserInfo.instance.CompanionList[iIndex].ReturnData(CHARACTER_DATA.CHAR_NAME));
        m_CharSprite.spriteName = Name;
        m_CharName.text = Name;
        string MaxHP = Util.ConvertToString(UserInfo.instance.CompanionList[iIndex].ReturnData(CHARACTER_DATA.CHAR_MAX_HP));
        string ATK = Util.ConvertToString(UserInfo.instance.CompanionList[iIndex].ReturnData(CHARACTER_DATA.CHAR_ATK));
        m_CharStatus.text = "HP: " + MaxHP + "\t" + "ATK: " + ATK;
        m_bClick = false;
        m_bDrag = false;

        m_PartySprite.GetComponent<UISprite>().spriteName = Name;
        m_PartySprite.SetActive(false);
    }
    
    public void OnClickSetting(CallBackInt call)
    {
        m_OnClick = call;
        //클릭 이벤트 셋팅
        m_bClick = true;
    }
    public void OnDragSetting(CallBackInt call)
    {
        m_OnClick = call;
        //클릭 이벤트 셋팅
    }

    public void OnClick()
    {
        if (m_bClick)
        {
            Debug.Log("ClickCallback");
            Debug.Log(m_iIndex);
            m_OnClick?.Invoke(m_iIndex);
        }
    }

    void OnDragStart()
    {
        //처음 드래그 시작, 이때 sprite 이미지를 만들어준다.
        //컴패니언의 인덱스 번호를 임시 저장한다.
        Debug.Log("Start");

        //캐릭터 스프라이트를 생성해서 마우스 포인트로 위치 시킨다.
        m_PartySprite.SetActive(true);
        m_PartySprite.transform.localPosition = UICamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        m_bDrag = true;
    }

    void OnDragEnd()
    {
        //파티 패널에 맞으면 확인

        Debug.Log("end");

        m_PartySprite.SetActive(false);//스프라이트를 꺼주고
        m_bDrag = false;

        m_OnClick?.Invoke(m_iIndex);    //해당 인덱스를 드래그 아웃    //컴패니언 리스트의 순서를 반환한다.
    }


    private void OnDrag()
    {
        if(m_bDrag)
        {
            m_PartySprite.transform.localPosition = UICamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        //첫 클릭 후 드래그 시 해당 이미지가 마우를 따라 간다. 놓을 시에는 
    }

}
