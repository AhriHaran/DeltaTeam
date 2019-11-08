using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInput : MonoBehaviour
{
    public int m_iIndex;
    private MAP_BLOCK_CLASS m_eClass;
    private bool m_bClickOK = false;
    private CallBackInt m_CallBacK = null;
    private GameObject m_SelectObject;
    private UISprite m_BlockSprite;

    private void Awake()
    {
        m_SelectObject = transform.GetChild(0).gameObject;
        m_BlockSprite = transform.GetComponent<UISprite>();
        m_eClass = (MAP_BLOCK_CLASS)Util.EnumChange<MAP_BLOCK_CLASS>(m_BlockSprite.spriteName);
    }

    public void CallBackSetting(CallBackInt call)
    {
        m_CallBacK = call;
    }

    public void SelectOK(bool bSelect)
    {
        m_bClickOK = bSelect;
        m_SelectObject.SetActive(bSelect);
    }

    public void OnClick()
    {
        //클릭시
        //콜백
        if(m_bClickOK)
        {
            m_CallBacK?.Invoke(m_iIndex);
            Debug.Log(m_iIndex);
        }
    }

    public void CurPlayer(bool bTrue)
    {
        if (bTrue)
            m_BlockSprite.spriteName = Util.ConvertToString(MAP_BLOCK_CLASS.BLOCK_PLAYER);
        else
            m_BlockSprite.spriteName = Util.ConvertToString(m_eClass);
    }
}
