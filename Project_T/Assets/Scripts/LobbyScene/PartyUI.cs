using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyUI : MonoBehaviour
{
    private GameObject[] m_PartyList = new GameObject[4];

    private void Awake()
    {
        for(int i = 0; i < 4; i++)
        {
            m_PartyList[i] = transform.GetChild(i).gameObject;
        }
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
            m_PartyList[i].GetComponent<PartyPanel>().ShowPartyInfo(PartyList[i].PartyIndex);
        }
    }
}
