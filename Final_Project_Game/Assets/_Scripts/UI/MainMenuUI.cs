using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NOOD.SerializableDictionary;
using TMPro;
using NOOD.Sound;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startNewBtn; 
    [SerializeField] private Button loadGameBtn;
    [SerializeField] private Button settingGameBtn;
    [SerializeField] private Button creditsGameBtn;
    [SerializeField] private Button exitGameBtn;
    [SerializeField] private string nameEssentialScene;
    [SerializeField] private string nameNewGameStartScene;
    public GameStatus gameStatus;

    private void Start()
    {
        startNewBtn.onClick.AddListener(StartNewGame);
        loadGameBtn.onClick.AddListener(LoadGame);
        settingGameBtn.onClick.AddListener(SettingGame);
        creditsGameBtn.onClick.AddListener(Credits);
        exitGameBtn.onClick.AddListener(ExitGame);
        if (gameStatus.isNewGame)
        {
            loadGameBtn.gameObject.SetActive(false);
        }
        else
            loadGameBtn.gameObject.SetActive(true);
    }

    public void StartNewGame()
    {
        Debug.Log("New Game");
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        gameStatus.isNewGame = true;
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Additive);
    }

    private void LoadGame()
    {
        Debug.Log("Load Game");
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
        SceneManager.LoadScene(gameStatus.nameScene, LoadSceneMode.Additive);
    }

  

    public void SettingGame()
    {
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
    }

    public void Credits()
    {
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
    }

    public void ExitGame()
    {
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        Application.Quit();
    }
}
