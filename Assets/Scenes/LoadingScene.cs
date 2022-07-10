using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

// 출처: https://wergia.tistory.com/183 [베르의 프로그래밍 노트:티스토리]

public class LoadingScene : MonoBehaviour {
    public static string nextScene;

    [SerializeField]
    Image progressBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone) {
            yield return null;

            timer += Time.deltaTime; 
            if (op.progress < 0.9f) { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); 
                if (progressBar.fillAmount >= op.progress) { 
                    timer = 0f; 
                } 
            } 
            else { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); 
                if (progressBar.fillAmount == 1.0f) { 
                    op.allowSceneActivation = true; 
                    yield break; 
                } 
            }
        }
    }
}
