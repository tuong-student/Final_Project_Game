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
    private void Start()
    {

        musicBtn.onClick.AddListener(AdjustMusic);
        soundBtn.onClick.AddListener(AdjustSound);
        confirmBtn.onClick.AddListener(OnConfirm);
        exitBtn.onClick.AddListener(OnExit);
        if(_homeBtn != null)
            _homeBtn.SetAction(OnHomeBtnHandler);

        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);

        panel = this.transform.GetChild(0).gameObject;
        showFB.Events.OnPlay.AddListener(() =>
        {
            panel.SetActive(true);
            GetComponent<Image>().raycastTarget = true;
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

    private void AdjustMusic()
    {
        isMuteMusic = !isMuteMusic;
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
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
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
    }


    #region Sound
    private void AdjustSound()
    {
        isMuteSound = !isMuteSound;
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];
        gameStatus.soundVolume = isMuteSound? 0:gameStatus.soundVolume;
        if(isMuteSound)
        {
            oldSoundVolume = gameStatus.soundVolume;
            gameStatus.soundVolume = 0;
        }
        else
        {
            gameStatus.soundVolume = oldSoundVolume;
        }
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;
    }
    #endregion

    #region Music
    private void ChangeMusicVolume(float volume)
    {
        gameStatus.musicVolume = volume;
        if (volume > 0)
            musicImg.sprite = listSprite[1];
        else
            musicImg.sprite = listSprite[0];
        
        SoundManager.GlobalMusicVolume = volume;
    }
    private void ChangeSoundVolume(float volume)
    {
        gameStatus.soundVolume = volume;
        if(volume > 0)
            soundImg.sprite = listSprite[1];
        else
            soundImg.sprite = listSprite[0];

        SoundManager.GlobalSoundVolume = volume;
    }
    #endregion

    #region Button functions
    private void OnConfirm()
    {
        gameStatus.isMusicMute = isMuteMusic;
        gameStatus.isSoundMute = isMuteSound;
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        Hide();
    }
    private void OnExit()
    {
        if (hideFB != null)
            hideFB.PlayFeedbacks();
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        SoundManager.GlobalMusicVolume = oldMusicVolume;
        SoundManager.GlobalSoundVolume = oldSoundVolume;

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
