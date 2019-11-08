using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionScroll : MonoBehaviour
{
    public GameObject m_Grid;
    private UIPanel         m_Panel;
    private UIScrollView    m_ScrollView;

    private void Awake()
    {
        m_Panel = transform.GetComponent<UIPanel>();
        m_ScrollView = transform.GetComponent<UIScrollView>();
    }

    //스크롤 뷰에 셋팅
    private void OnEnable()
    {
        //on off 될 때 셋팅
        int iMax = UserInfo.instance.CompanionList.Count;
        
        if(m_Grid.transform.childCount > 0) //그리드 초기화
        {
            while (m_Grid.transform.childCount != 0)
            {
                GameObject game = m_Grid.transform.GetChild(0).gameObject;
                game.transform.SetParent(null);
                NGUITools.Destroy(game);
            }
            m_Grid.transform.DetachChildren();
        }
        
        for(int i = 0; i < iMax; i++)
        {
            GameObject CharInfo = ResourceLoader.CreatePrefab("Prefabs/UI/CharInfomation");
            CharInfo.transform.SetParent(m_Grid.transform, false);
            CharInfo.GetComponent<CharInfomation>().Setting(i); //컴패니언 리스트 기준의 인덱스
        }

        m_Grid.GetComponent<UIGrid>().Reposition(); //리 포지셔닝으로 그리드 재정렬
        m_ScrollView.ResetPosition();
        m_Panel.Refresh();
    }

    public void OnClickSetting(CallBackInt call)
    {
        //캐릭터 정보창에서 클릭 셋팅
        int iMax = UserInfo.instance.CompanionList.Count;
        for (int i = 0; i < iMax; i++)
        {
            m_Grid.transform.GetChild(i).GetComponent<CharInfomation>().OnClickSetting(call);
        }
    }

    public void OnDragSetting(CallBackInt call)
    {
        //캐릭터 정보창에서 클릭 셋팅
        int iMax = UserInfo.instance.CompanionList.Count;
        for (int i = 0; i < iMax; i++)
        {
            m_Grid.transform.GetChild(i).GetComponent<CharInfomation>().OnDragSetting(call);
        }
    }
}
