using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NOOD.Sound;
using System.Collections.Generic;
using DG.Tweening;
using EasyTransition;

public class MainMenuUI : MonoBehaviour
{
    #region SerializeField
    [SerializeField] private Button startNewBtn; 
    [SerializeField] private Button loadGameBtn;
    [SerializeField] private Button settingGameBtn;
    [SerializeField] private Button creditsGameBtn;
    [SerializeField] private Button exitGameBtn;
    [SerializeField] private SettingPanel settingPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private string nameEssentialScene;
    [SerializeField] private string nameNewGameStartScene;
    [Tooltip("Find a TransitionSettings type by search t:TransitionSettings in project tab")]
    [SerializeField] private TransitionSettings _transitionSettings;
    #endregion

    #region public
    public GameStatusSO gameStatus;
    #endregion

    #region private
    #endregion


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
        
        SoundManager.PlayMusic(NOOD.Sound.MusicEnum.Theme);
    }

    public void StartNewGame()
    {
        Debug.Log("New Game");
        gameStatus.isNewGame = true;
        TransitionManager.Instance().onTransitionCutPointReached = () => 
        {
            SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
            SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Additive);
        };
        TransitionManager.Instance().Transition(_transitionSettings, 0);
    }

    private void LoadGame()
    {
        Debug.Log("Load Game");
        
        TransitionManager.Instance().onTransitionCutPointReached = () =>
        {
            SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Single);
            SceneManager.LoadScene(gameStatus.nameScene, LoadSceneMode.Additive);
        };
        TransitionManager.Instance().Transition(_transitionSettings, 0);
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
