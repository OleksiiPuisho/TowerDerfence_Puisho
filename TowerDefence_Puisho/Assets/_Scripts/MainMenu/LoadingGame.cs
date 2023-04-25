using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingGame : MonoBehaviour
{
    public static string LoadScene;
    [SerializeField] private Slider LoadSlider;
    private void Start()
    {
        if (LoadScene != null)
        {
            StartCoroutine(AsyncLoad());
        }
    }
    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadScene);
        while (!operation.isDone)
        {
            float progres = operation.progress / 0.9f;
            LoadSlider.value = progres;
            yield return null;
        }
    }
}

