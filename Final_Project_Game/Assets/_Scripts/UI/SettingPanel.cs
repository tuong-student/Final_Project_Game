using MoreMountains.Feedbacks;
using NOOD.Sound;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using NOOD;
using UnityEngine.SceneManagement;
using EasyTransition;
using UnityEditor.ShortcutManagement;

public class SettingPanel : MonoBehaviour
{
    #region SerializeField
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private CustomButton _homeBtn;
    [SerializeField] private Image musicImg;
    [SerializeField] private Image soundImg;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private List<Sprite> listSprite;
    [SerializeField] private GameStatusSO gameStatus;
    [SerializeField] private MMF_Player showFB;
    [SerializeField] private MMF_Player hideFB;
    [SerializeField] private TransitionSettings _transitionSetting;
    #endregion

    #region Private
    private bool _isMuteMusic;
    private bool _isMuteSound;
    private bool isShow;
    private GameObject _panel;
    private float _oldMusicVolume, _oldSoundVolume;
    #endregion

    #region Unity functions
    private void OnEnable()
    {
        _isMuteMusic = gameStatus.isMusicMute;
        _isMuteSound = gameStatus.isSoundMute;
        musicImg.sprite = _isMuteMusic ? listSprite[0] : listSprite[1];
        soundImg.sprite = _isMuteSound ? listSprite[0] : listSprite[1];
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;
        GameInput.onPlayerPressEscape += OnRequestShowHide;
    }
    private void Start()
    {

        musicBtn.onClick.AddListener(OnMusicButtonPressHandler);
        soundBtn.onClick.AddListener(OnSoundButtonPressHandler);
        confirmBtn.onClick.AddListener(OnConfirm);
        exitBtn.onClick.AddListener(OnExit);
        if(_homeBtn != null)
            _homeBtn.SetAction(OnHomeBtnHandler);

        musicSlider.onValueChanged.AddListener(OnMusicSliderChangeHandler);
        soundSlider.onValueChanged.AddListener(OnSoundSliderChangeHandler);

        _panel = this.transform.GetChild(0).gameObject;
        showFB.Events.OnPlay.AddListener(() =>
        {
            _panel.SetActive(true);
            GetComponent<Image>().raycastTarget = true;
        });
        hideFB.Events.OnComplete.AddListener(() =>
        {
            _panel.SetActive(true);
            GetComponent<Image>().raycastTarget = false;
        });

        if(SoundManager.IsMusicPlaying(NOOD.Sound.MusicEnum.Theme) == false)
        {
            SoundManager.PlayMusic(NOOD.Sound.MusicEnum.Theme);
        }

        isShow = false;
    }
    void Update()
    {
        musicSlider.value = gameStatus.musicVolume;
        soundSlider.value = gameStatus.soundVolume;
    }
    void OnDisable()
    {
        GameInput.onPlayerPressEscape -= OnRequestShowHide;
    }
    #endregion

    #region Button
    private void OnMusicButtonPressHandler()
    {
        _isMuteMusic = !_isMuteMusic;
        musicImg.sprite = _isMuteMusic ? listSprite[0] : listSprite[1];
        
        if(_isMuteMusic)
        {
            _oldMusicVolume = gameStatus.musicVolume;
            gameStatus.musicVolume = 0;
        }
        else
        {
            gameStatus.musicVolume = _oldMusicVolume;
        }

        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
    }
    private void OnSoundButtonPressHandler()
    {
        _isMuteSound = !_isMuteSound;
        soundImg.sprite = _isMuteSound ? listSprite[0] : listSprite[1];

        if(_isMuteSound)
        {
            _oldSoundVolume = gameStatus.soundVolume;
            gameStatus.soundVolume = 0;
        }
        else
        {
            gameStatus.soundVolume = _oldSoundVolume;
        }

        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;
    }
    #endregion

    #region Slider
    private void OnMusicSliderChangeHandler(float volume)
    {
        gameStatus.musicVolume = volume;
        if (volume > 0)
        {
            musicImg.sprite = listSprite[1];
            _isMuteMusic = false;
        }
        else
        {
            musicImg.sprite = listSprite[0];
            _isMuteMusic = true;
        }
        SoundManager.GlobalMusicVolume = volume;
    }
    private void OnSoundSliderChangeHandler(float volume)
    {
        gameStatus.soundVolume = volume;
        if(volume > 0)
        {
            soundImg.sprite = listSprite[1];
            _isMuteSound = false;
        }
        else
        {
            soundImg.sprite = listSprite[0];
            _isMuteSound = true;
        }
        SoundManager.GlobalSoundVolume = volume;

    }
    #endregion

    #region Button functions
    private void OnConfirm()
    {
        // Apply setting
        gameStatus.isMusicMute = _isMuteMusic;
        gameStatus.isSoundMute = _isMuteSound;
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;

        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        Hide();
    }
    private void OnExit()
    {
        // Don't apply the setting and reset to old volume
        gameStatus.musicVolume = _oldMusicVolume;
        gameStatus.soundVolume = _oldSoundVolume;
        SoundManager.GlobalMusicVolume = _oldMusicVolume;
        SoundManager.GlobalSoundVolume = _oldSoundVolume;

        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        Hide();
    }
    private void OnHomeBtnHandler()
    {
        TransitionManager.Instance().onTransitionCutPointReached = () =>
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        TransitionManager.Instance().Transition(_transitionSetting, 0);
    }
    #endregion

    #region Show Hide
    public void OnRequestShowHide()
    {
        isShow = !isShow;
        if (isShow)
            Show();
        else
            Hide();
    }
    public void Show()
    {
        if(UIManager.Instance != null)
            UIManager.Instance.AddToUIList(this);
        isShow = true;
        if (showFB != null)
            showFB.PlayFeedbacks();

        _oldMusicVolume = SoundManager.GlobalMusicVolume;
        _oldSoundVolume = SoundManager.GlobalSoundVolume;
    }
    public void Hide()
    {
        if(UIManager.Instance != null) // If at menu, UIManager == null
            UIManager.Instance.RemoveToUIList(this);
        isShow = false;
        if (hideFB != null)
            hideFB.PlayFeedbacks();
    }
    #endregion
}
