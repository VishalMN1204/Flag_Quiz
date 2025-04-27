using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage Instance;
    static int currentSceneIndex = 0;
    [SerializeField] TextMeshProUGUI progressText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene()
    {
        StartCoroutine(LoadAsynchronously());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadAsynchronously()
    {
        currentSceneIndex = currentSceneIndex == (SceneManager.sceneCountInBuildSettings - 1) ? -1 : currentSceneIndex;
        currentSceneIndex++;
        AsyncOperation operation = SceneManager.LoadSceneAsync(currentSceneIndex);
        if (currentSceneIndex == 1)
        {
            progressText.gameObject.SetActive(true);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                progressText.text = $"{(progress * 100):0}%";
                yield return null;
            }
        }
    }
}
