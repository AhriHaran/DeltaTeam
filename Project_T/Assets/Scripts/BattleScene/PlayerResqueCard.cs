using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResqueCard : MonoBehaviour
{
    private UISprite m_CharSprite;
    private UILabel m_CharLevel;
    private UILabel m_CharName;
    private UILabel m_CharClass;

    private void Awake()
    {
        m_CharSprite = transform.GetChild(0).GetComponent<UISprite>();
        m_CharLevel = transform.GetChild(1).GetComponent<UILabel>();
        m_CharName = transform.GetChild(2).GetComponent<UILabel>();
        m_CharClass = transform.GetChild(3).GetComponent<UILabel>();
    }

    public void Setting(string strName, CLASS eClass)
    {
        m_CharLevel.text = "Lv.1";
        m_CharSprite.spriteName = strName;

        if (eClass == CLASS.CLASS_WARRIOR)
            m_CharClass.text = "워리어";
        else if (eClass == CLASS.CLASS_THIEF)
            m_CharClass.text = "도적";
        else if (eClass == CLASS.CLASS_ARCHER)
            m_CharClass.text = "궁수";
        else if (eClass == CLASS.CLASS_WIZARD)
            m_CharClass.text = "마법사";

        m_CharName.text = strName;
    }
}
