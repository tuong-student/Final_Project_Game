using MoreMountains.Feedbacks;
using NOOD.Sound;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using UnityEngine.SceneManagement;
using EasyTransition;

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
    private bool isMuteMusic;
    private bool isMuteSound;
    private bool isShow;
    private GameObject panel;
    private float oldMusicVolume, oldSoundVolume;
    #endregion

    #region Unity functions
    private void OnEnable()
    {
        isMuteMusic = gameStatus.isMusicMute;
        isMuteSound = gameStatus.isSoundMute;
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;
        GameInput.onPlayerPressEscape += OnRequestShowHide;
    }
    private void Awake()
    {
        Time.timeScale = 1;
    }
    private void Start()
    {
        musicBtn.onClick.AddListener(OnMusicButtonPressHandler);
        soundBtn.onClick.AddListener(OnSoundButtonPressHandler);
        confirmBtn.onClick.AddListener(OnConfirmButtonHandler);
        exitBtn.onClick.AddListener(OnExitButtonHandler);
        if(_homeBtn != null)
            _homeBtn.SetAction(OnHomeBtnHandler);

        musicSlider.onValueChanged.AddListener(OnMusicSliderChangeHandler);
        soundSlider.onValueChanged.AddListener(OnSoundSliderChangeHandler);

        panel = this.transform.GetChild(0).gameObject;
        showFB.Events.OnPlay.AddListener(() =>
        {
            panel.SetActive(true);
            GetComponent<Image>().raycastTarget = true;
        });
        showFB.Events.OnComplete.AddListener(() =>
        {
            Time.timeScale = 0;
        });
        hideFB.Events.OnPlay.AddListener(() =>
        {
            Time.timeScale = 1;
        });
        hideFB.Events.OnComplete.AddListener(() =>
        {
            panel.SetActive(true);
            GetComponent<Image>().raycastTarget = false;
        });

        if(SoundManager.IsMusicPlaying(NOOD.Sound.MusicEnum.Theme) == false)
        {
            SoundManager.PlayMusic(NOOD.Sound.MusicEnum.Theme);
        }

        isShow = false;
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;
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

    #region Volume Button
    private void OnMusicButtonPressHandler()
    {
        isMuteMusic = !isMuteMusic;
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];

        if(isMuteMusic)
        {
            oldMusicVolume = gameStatus.musicVolume;
            gameStatus.musicVolume = 0;
        }
        else
        {
            gameStatus.musicVolume = oldMusicVolume;
        }

        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
    }
    private void OnSoundButtonPressHandler()
    {
        isMuteSound = !isMuteSound;
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];

        if(isMuteSound)
        {
            oldSoundVolume = gameStatus.soundVolume;
            gameStatus.soundVolume = 0;
        }
        else
        {
            gameStatus.soundVolume = oldSoundVolume;
        }

        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;
    }
    #endregion

    #region Volume Slider
    private void OnMusicSliderChangeHandler(float volume)
    {
        gameStatus.musicVolume = volume;
        if (volume > 0)
        {
            musicImg.sprite = listSprite[1];
            isMuteMusic = false;
        }
        else
        {
            musicImg.sprite = listSprite[0];
            isMuteMusic = true;
        }
        
        SoundManager.GlobalMusicVolume = volume;
    }
    private void OnSoundSliderChangeHandler(float volume)
    {
        gameStatus.soundVolume = volume;
        if(volume > 0)
        {
            soundImg.sprite = listSprite[1];
            isMuteSound = false;
        }
        else
        {
            soundImg.sprite = listSprite[0];
            isMuteSound = true;
        }

        SoundManager.GlobalSoundVolume = volume;
    }
    #endregion

    #region Button functions
    private void OnConfirmButtonHandler()
    {
        // Apply changes
        gameStatus.isMusicMute = isMuteMusic;
        gameStatus.isSoundMute = isMuteSound;
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;

        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        Hide();
    }
    private void OnExitButtonHandler()
    {
        // Discard change and return to old value
        gameStatus.musicVolume = oldMusicVolume;
        gameStatus.soundVolume = oldSoundVolume;
        SoundManager.GlobalMusicVolume = oldMusicVolume;
        SoundManager.GlobalSoundVolume = oldSoundVolume;

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
        //UIManager.Instance.AddToUIList(this);
        isShow = true;
        if (showFB != null)
            showFB.PlayFeedbacks();

        oldMusicVolume = gameStatus.musicVolume;
        oldSoundVolume = gameStatus.soundVolume;
    }
    public void Hide()
    {
        //UIManager.Instance.RemoveToUIList(this);
        isShow = false;
        if (hideFB != null)
            hideFB.PlayFeedbacks();
    }
    #endregion
}
