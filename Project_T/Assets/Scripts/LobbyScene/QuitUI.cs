using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitUI : MonoBehaviour
{
    private GameObject m_Quit;

    private void Awake()
    {
        m_Quit = transform.GetChild(0).gameObject;
        m_Quit.SetActive(false);
    }
    
    public void OnClick()
    {
        string Button = UIButton.current.name;

        if(Button == "Exit")
        {
            Application.Quit();
        }
        else if(Button == "None")
        {
            m_Quit.SetActive(false);
        }
    }

    private void Update()
    {
        OnControll();
    }

    private void OnControll()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_Quit.activeSelf)
            {
                m_Quit.SetActive(false);
            }
            else
            {
                m_Quit.SetActive(true);
            }
        }
    }
}
