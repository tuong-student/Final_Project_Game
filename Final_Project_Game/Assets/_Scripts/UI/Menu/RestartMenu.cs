using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using Game;
using ImpossibleOdds.Http;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{
    #region SerializeField
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _panel;
    [SerializeField] private CustomButton _mainMenuBtn, _retryBtn;
    [SerializeField] private MMF_Player _showFB, _hideFB;
    [SerializeField] private GameStatusSO _gameStatus;
    [SerializeField] private TransitionSettings _transitionSettings;
    #endregion

    #region Unity functions
    void Awake()
    {
        _mainMenuBtn.SetAction(ReturnToMainMenu);
        _retryBtn.SetAction(Retry);
        _canvasGroup.alpha = 0;
        Hide();
    }
    void OnDisable()
    {
        Hide();
    }
    #endregion

    #region ButtonFunctions
    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }
    private void Retry()
    {
        _gameStatus.isNewGame = true;
        TransitionManager.Instance().onTransitionCutPointReached = () => 
        {
            SceneManager.LoadScene("Essential", LoadSceneMode.Single);
            SceneManager.LoadScene("2DMainGame", LoadSceneMode.Additive);
        };
        TransitionManager.Instance().Transition(_transitionSettings, 0);
        Hide();
    }
    #endregion

    #region Show Hide functions
    public void Show()
    {
        _canvasGroup.alpha = 1;
        if (_showFB != null)
            _showFB.PlayFeedbacks();

        UIManager.Instance.AddToUIList(this);
    }
    public void Hide()
    {
        if (_hideFB != null)
            _hideFB.PlayFeedbacks();

        UIManager.Instance.RemoveToUIList(this);
    }
    #endregion
}
