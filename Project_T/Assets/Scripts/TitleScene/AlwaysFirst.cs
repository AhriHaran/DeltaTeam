using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AlwaysFirst : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Awake()
    {
        if (SceneManager.GetActiveScene().name.CompareTo("TitleScene") != 0)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
