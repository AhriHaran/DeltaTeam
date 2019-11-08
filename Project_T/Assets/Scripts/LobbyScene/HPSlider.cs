using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSlider : MonoBehaviour
{
    private UISprite m_HPSprite;

    private void Awake()
    {
        m_HPSprite = transform.GetComponent<UISprite>();
    }

    public void ShowHP(object oMax, object oCur)
    {
        if(m_HPSprite == null)
            m_HPSprite = transform.GetComponent<UISprite>();
        m_HPSprite.fillAmount = Util.FillValue(oMax, oCur);
    }
}
