using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class GameEndManager : MonoBehaviour
{
    [SerializeField] private string _nextScene;
    [SerializeField] private string _nameSceneMenu;
    [SerializeField] private string _nameSceneLoading;
    public void NextLevelButton()
    {
        if (SceneManager.GetSceneByName(_nextScene) != null)
        {
            LoadingGame.LoadScene = _nextScene;
            SceneManager.LoadScene(_nameSceneLoading);
        }
        else
            Debug.LogError($"{GetType().Name} Level {_nextScene} dont find!");
    }
    public void ResetGameButton()
    {
        string thisScene = SceneManager.GetActiveScene().name;
        LoadingGame.LoadScene = thisScene;
        SceneManager.LoadScene(_nameSceneLoading);
    }
    public void GoToMenuButton()
    {
        SceneManager.LoadScene(_nameSceneMenu);
    }
}
