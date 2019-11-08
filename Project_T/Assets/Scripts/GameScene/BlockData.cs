using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum MAP_BLOCK_CLASS
{
    BLOCK_START,        //블럭 스타트 포인트
    BLOCK_NONE,         //아무것도 없는 블럭
    BLOCK_MONSTER,      //몬스터 있는 블럭
    BLOCK_COMPANION,    //동료가 있는 블럭
    BLOCK_BOSS,         //보스가 있는 블럭
    BLOCK_DANGEROUS,    //위험한 적이 있는 블럭
    BLOCK_END,
    BLOCK_PLAYER,       //플레이어가 있는 블럭
}

public class BlockData
{
    private MAP_BLOCK_CLASS[] m_eClass;  //현재 블럭의 속성들
    public MAP_BLOCK_CLASS[] BlockClass
    {
        get { return m_eClass; }
        set { m_eClass = value; }
    }
    private int[] m_eLinkedList;        //현재 방과 연결된 방 번호
    public int[] BlockLinkedList
    {
        get { return m_eLinkedList; }
        set { m_eLinkedList = value; }
    }

    public BlockData(string strClass, string strLinkedList)
    {
        string[] SplitData = strClass.Split(';');
        int iMax = SplitData.Length;
        BlockClass = new MAP_BLOCK_CLASS[iMax];

        for (int i = 0; i < iMax; i++)
        {
            BlockClass[i] = (MAP_BLOCK_CLASS)Util.EnumChange<MAP_BLOCK_CLASS>(SplitData[i]);
        }

        SplitData = strLinkedList.Split(';');
        iMax = SplitData.Length;
        BlockLinkedList = new int[iMax];

        for (int i = 0; i < iMax; i++)
        {
            BlockLinkedList[i] = Util.ConvertToInt(SplitData[i]);
        }
    }

    ~BlockData() { }
    //각 블럭의 데이터들
}
