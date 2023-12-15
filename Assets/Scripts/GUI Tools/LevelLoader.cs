using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelLoader : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] protected Animator effect;
    [SerializeField, AllowNull] protected Slider slider;

    protected bool canExit = false;

    public bool CanExit { set => canExit = value; }

    protected IEnumerator SceneChangeCoroutine(string sceneName)
    {
        if (slider != null) slider.gameObject.SetActive(true);
        effect.SetTrigger("Effect");
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        sceneLoading.allowSceneActivation = false;
        while (!sceneLoading.isDone && !canExit) {
            if (slider != null) slider.value = sceneLoading.progress;
            yield return new WaitForEndOfFrame();
        }
        sceneLoading.allowSceneActivation = true;
    }

    protected void LoadNextLevel(string sceneName)
    {
        StartCoroutine(SceneChangeCoroutine(sceneName));
    }

    public void LevelChange(string sceneName)
    {
        LoadNextLevel(sceneName);
    }
}
