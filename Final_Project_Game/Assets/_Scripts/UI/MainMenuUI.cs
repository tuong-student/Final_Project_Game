using Game;
using NOOD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NOOD;
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
    public GameStatus gamestatus;

    private void Start()
    {
        startNewBtn.onClick.AddListener(StartNewGame);
        loadGameBtn.onClick.AddListener(LoadGame);
        settingGameBtn.onClick.AddListener(SettingGame);
        creditsGameBtn.onClick.AddListener(Creadits);
        exitGameBtn.onClick.AddListener(ExitGame);
        if (gamestatus.isNewGame)
        {
            loadGameBtn.gameObject.SetActive(false);
        }
        else
            loadGameBtn.gameObject.SetActive(true);
    }

    public void StartNewGame()
    {
        Debug.Log("New Game");
        SoundManager.PlaySound(SoundEnum.ClickButton);
        gamestatus.isNewGame = true;
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Additive);
    }

    private void LoadGame()
    {
        Debug.Log("Load Game");
        SoundManager.PlaySound(SoundEnum.ClickButton);
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
        SceneManager.LoadScene(gamestatus.nameScene, LoadSceneMode.Additive);
    }

  

    public void SettingGame()
    {
        SoundManager.PlaySound(SoundEnum.ClickButton);
    }

    public void Creadits()
    {
        SoundManager.PlaySound(SoundEnum.ClickButton);
    }

 

    public void ExitGame()
    {
        SoundManager.PlaySound(SoundEnum.ClickButton);
        Application.Quit();
    }
}
