using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteminaSlider : MonoBehaviour
{
    public void ShowStemina(int CurSpeed)
    {
        int iCur = Util.ConvertToInt(CurSpeed);
        for (int i = 0; i < 3; i++)
        {
            if (i < iCur)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
