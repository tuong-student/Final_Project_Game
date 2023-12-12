using MoreMountains.Feedbacks;
using NOOD.Sound;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;

public class SettingPanel : MonoBehaviour
{
    #region SerializeField
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button exitBtn; 
    [SerializeField] private Image musicImg;
    [SerializeField] private Image soundImg;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private List<Sprite> listSprite;
    [SerializeField] private GameStatusSO gameStatus;
    [SerializeField] private MMF_Player showFB;
    [SerializeField] private MMF_Player hideFB;
    #endregion
    private bool isMuteMusic;
    private bool isMuteSound;
    private bool isShow;
    private GameObject panel;

    private void OnEnable()
    {
        isMuteMusic = gameStatus.isMusicMute;
        isMuteSound = gameStatus.isSoundMute;
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];
        musicSlider.value = gameStatus.musicVolume;
        soundSlider.value = gameStatus.soundVolume;
    }
    private void Start()
    {
        musicBtn.onClick.AddListener(AdjustMusic);
        soundBtn.onClick.AddListener(AdjustSound);
        confirmBtn.onClick.AddListener(OnConfirm);
        exitBtn.onClick.AddListener(OnExit);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        musicSlider.onValueChanged.AddListener(ChangeSoundVolume);

        GameInput.onPlayerPressEscape += OnRequestShowHide;

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
    }

    private void AdjustMusic()
    {
        isMuteMusic = !isMuteMusic;
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, isMuteMusic?0:1);
    }

    private void AdjustSound()
    {
        isMuteSound = !isMuteSound;
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];
    }

    private void ChangeMusicVolume(float volume)
    {
        gameStatus.musicVolume = volume;
        if (volume > 0)
            musicImg.sprite = listSprite[1];
        else
            musicImg.sprite = listSprite[0];
        
        SoundManager.ChangeMusicVolumeAll(volume);
    }

    private void ChangeSoundVolume(float volume)
    {
        gameStatus.soundVolume = volume;
        if(volume > 0)
            soundImg.sprite = listSprite[1];
        else
            soundImg.sprite = listSprite[0];
    }

    private void OnConfirm()
    {
        gameStatus.isMusicMute = isMuteMusic;
        gameStatus.isSoundMute = isMuteSound;
        SoundManager.GlobalMusicVolume = gameStatus.musicVolume;
        SoundManager.GlobalSoundVolume = gameStatus.soundVolume;
        SoundManager.ChangeMusicVolumeAll( gameStatus.isMusicMute?0:gameStatus.musicVolume);
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        Hide();
    }

    private void OnExit()
    {
        if (hideFB != null)
            hideFB.PlayFeedbacks();
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);

        Hide();
    }

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
        isShow = true;
        if (showFB != null)
            showFB.PlayFeedbacks();
    }
    public void Hide()
    {
        isShow = false;
        if (hideFB != null)
            hideFB.PlayFeedbacks();
    }
    #endregion
}
