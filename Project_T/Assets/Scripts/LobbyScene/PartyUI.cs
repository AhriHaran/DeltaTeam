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

                //무조건 외부 스크롤에서 오므로

                var PartyList = UserInfo.instance.PartyList;
                int iMax = PartyList.Count;
                if (iHitCol < iMax)
                {
                    //드래그 엔드시 파티 인덱스 보다 작다.
                    //옮기려는 캐릭터가 현재 파티에 소속 되어 있는가?
                    //파티에 소속이 되어 있다면 자리 교체고 아니면 교환
                    //파티에 소속끼리 자리 교체 혹은 
                    //외부 컴패니언에서 교체
                    //
                    //우선 해당 인덱스 닝겐이 파티 리스트에 존재하는가?

                    //외부
                    bool bOverLap = false;
                    int iOverLap = -1;
                    for (int i = 0; i < iMax; i++)
                    {
                        if (PartyList[i].PartyIndex == iIndex)
                        {
                            //해당 파티 인덱스가 존재한다.
                            bOverLap = true;
                            iOverLap = i;
                        }
                    }

                    if (!bOverLap)   //중복이 없다. 그렇다면 해당 패널의 정보와 교체
                    {
                        //해당 파티 자리와 컴패니언의 캐릭터를 교환
                        UserInfo.instance.PartySwapToCompanion(iHitCol, iIndex);
                    }
                    else
                    {
                        UserInfo.instance.PartySwapToParty(iHitCol, iOverLap);
                        //자리교환
                    }
                }
                else
                {
                    //파티의 추가 구성
                    //파티 추가시 해당 캐릭터가 현재 파티에 있는가 중복 확인wj

                    //파티 추가

                    //새로운 파티 추가
                    bool bOverLap = false;
                    for(int i = 0; i < iMax; i++)
                    {
                        if(PartyList[i].PartyIndex == iIndex)
                        {
                            //해당 파티 인덱스가 존재한다.
                            bOverLap = true;
                        }
                    }

                    if(!bOverLap)   //중복 추가를 막아줌
                    {
                        UserInfo.instance.PartySetting(iIndex);
                    }
                }

                PartyListShow();
                //Take hit point and move to hit position on terrain
                UserInfo.instance.UserPartySave();
            }
        }

    }
}
