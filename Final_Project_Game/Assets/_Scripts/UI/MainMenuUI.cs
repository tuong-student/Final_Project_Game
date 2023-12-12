using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NOOD.Sound;
using System.Collections.Generic;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startNewBtn; 
    [SerializeField] private Button loadGameBtn;
    [SerializeField] private Button settingGameBtn;
    [SerializeField] private Button creditsGameBtn;
    [SerializeField] private Button exitGameBtn;
    [SerializeField] private SettingPanel settingPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private string nameEssentialScene;
    [SerializeField] private string nameNewGameStartScene;
    public GameStatusSO gameStatus;
    private bool isMute;
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
        
        isMute = gameStatus.isSoundMute;
        if(isMute == false)
            SoundManager.PlayMusic(NOOD.Sound.MusicEnum.Theme);
    }

    public void StartNewGame()
    {
        Debug.Log("New Game");
        gameStatus.isNewGame = true;
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Additive);
        
    }

    private void LoadGame()
    {
        Debug.Log("Load Game");
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
        SceneManager.LoadScene(gameStatus.nameScene, LoadSceneMode.Additive);
    }

  

    public void SettingGame()
    {
        // Play sound will run in Parent class
        settingPanel.OnRequestShowHide();
    }

    public void Credits()
    {
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
