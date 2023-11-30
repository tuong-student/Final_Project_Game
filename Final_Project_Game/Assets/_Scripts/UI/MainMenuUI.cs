using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startNewBtn; 
    [SerializeField] private Button loadGameBtn;
    [SerializeField] private Button exitGameBtn;
    [SerializeField] private string nameEssentialScene;
    [SerializeField] private string nameNewGameStartScene;
    private void Start()
    {
        startNewBtn.onClick.AddListener(StartNewGame);
        loadGameBtn.onClick.AddListener(LoadGame);
        exitGameBtn.onClick.AddListener(ExitGame);
    }

    private void LoadGame()
    {
        Debug.Log("Load Game");
        SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartNewGame()
    {
        Debug.Log("New Game");
        ResetToDefault();
        SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }

    private void ResetToDefault()
    {
      
    }
}
