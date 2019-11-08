using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyUI : MonoBehaviour
{
    public GameObject m_CharScroll;
    private GameObject[] m_PartyList = new GameObject[4];

    private void Awake()
    {
        for(int i = 0; i < 4; i++)
        {
            m_PartyList[i] = transform.GetChild(i).gameObject;
        }

        m_CharScroll.GetComponent<CompanionScroll>().OnDragSetting(DragEnd);//스크롤 뷰 드래그 콜백
    }

    private void OnEnable()
    {
        PartyListShow();
    }

    public void PartyListShow()
    {
        var PartyList = UserInfo.instance.PartyList;

        for(int i = 0; i < PartyList.Count; i++)
        {
            m_PartyList[i].GetComponent<PartyPanel>().ShowPartyInfo(i);
        }
    }

    public void DragEnd(int iIndex)
    {
        //드래그 엔드
        Ray ray = UICamera.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.tag == "Party")
            {
                //해당 인덱스가 파티에 임 있는가 그렇다면 자리 교환해라

                //우선 히트 당한 애의 인덱스를 넘겨 받아라

                int iHitCol = hit.collider.gameObject.GetComponent<PartyPanel>().m_iPartyPanelIndex;
                //몇번 파티 자리인가?

                //파티 자리 교체는 두 명 이상 존재할 때 부터 가능

                var PartyList = UserInfo.instance.PartyList;
                if (iHitCol < PartyList.Count)
                {
                    //드래그 엔드시 파티 인덱스 보다 작다.
                    //옮기려는 캐릭터가 현재 파티에 소속 되어 있는가?
                    //파티에 소속이 되어 있다면 자리 교체고 아니면 교환
                    //파티에 소속끼리 자리 교체 혹은 
                    //외부 컴패니언에서 교체
                    //
                    
                }
                else
                {
                    //파티의 추가 구성
                    //파티 추가시 해당 캐릭터가 현재 파티에 있는가 중복 확인

                }

                hit.collider.gameObject.GetComponent<PartyPanel>().ShowPartyInfo(iIndex);
                //Take hit point and move to hit position on terrain
            }
        }

    }
}
