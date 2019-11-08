using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelect : MonoBehaviour
{
    //배틀씬에서 에너미 선택 할 떄
    private int m_iIndex;   //에너미 인덱스
    public int EnemyIndex
    {
        get { return m_iIndex; }
        set { m_iIndex = value; }
    }

    public void Awake()
    {
        EnemyIndex = -1;
    }

    public void OnClick()
    {
        if (GameManager.instance.WhatPhase == GAME_PHASE.PHASE_TARGET_SELECT && 
            GameManager.instance.WhoAttakerType == CHARACTER_TYPE.CHAR_PLAYER)
        {
            GameManager.instance.WhoTargetIndex = m_iIndex; //타겟 셋팅
            transform.parent.GetComponent<SpineManager>().TargetEffect(m_iIndex);
        }
    }
}
