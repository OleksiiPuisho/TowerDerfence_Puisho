using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [Header("Select Level")]
    [SerializeField] private Button _startGameBautton;
    [SerializeField] private string _loadSceneName;
    public void LoadSceneButton(string sceneName)
    {
        LoadingGame.LoadScene = sceneName;
        _startGameBautton.interactable = true;
    }
    void Awake()
    {
        _startGameBautton.interactable = false;
        _startGameBautton.onClick.AddListener(StartGameButton);
    }
    private void StartGameButton()
    {
        SceneManager.LoadScene(_loadSceneName);
    }
}
