using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public UISlider LoadSlider;
    public static string nextScene; 
    AsyncOperation async_operation;

    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(SceneLoad());
    }

    public static void SceneLoad(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadScene", LoadSceneMode.Single);
    }

    private IEnumerator SceneLoad()
    {
        yield return null;

        async_operation = SceneManager.LoadSceneAsync(nextScene);   //씬 매니저로 로딩
        async_operation.allowSceneActivation = false;
        float timer = 0.0f;

        while(!async_operation.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (async_operation.progress >= 0.9f)
            {
                LoadSlider.value = Mathf.Lerp(LoadSlider.value, 1.0f, timer);

                if (LoadSlider.value == 1.0f)
                    async_operation.allowSceneActivation = true;
            }
            else
            {
                LoadSlider.value = Mathf.Lerp(LoadSlider.value, async_operation.progress, timer);
                if (LoadSlider.value >= async_operation.progress)
                {
                    timer = 0f;
                }
            }
        }
    }
}
