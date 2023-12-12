using MoreMountains.Feedbacks;
using NOOD.Sound;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Rendering;

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
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
        Hide();
    }

    private void AdjustMusic()
    {
        isMuteMusic = !isMuteMusic;
        musicSlider.value = isMuteMusic ? 0 : 1;
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked, gameStatus.soundVolume);
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, gameStatus.musicVolume);
    }

    private void AdjustSound()
    {
        isMuteSound = !isMuteSound;
        soundSlider.value = soundSlider ? 0 : 1;
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked, gameStatus.soundVolume);
    }

    private void ChangeMusicVolume(float volume)
    {
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, volume);
        if (volume > 0)
            musicImg.sprite = listSprite[1];
        else
            musicImg.sprite = listSprite[0];
    }

    private void ChangeSoundVolume(float volume)
    {
        if(volume > 0)
            soundImg.sprite = listSprite[1];
        else
            soundImg.sprite = listSprite[0];
    }

    private void OnConfirm()
    {
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked, gameStatus.soundVolume);
        gameStatus.isMusicMute = isMuteMusic;
        gameStatus.isSoundMute = isMuteSound;
        gameStatus.musicVolume = musicSlider.value;
        gameStatus.soundVolume = soundSlider.value;
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, musicSlider.value);
        this.gameObject.SetActive(false);
        Hide();
    }
    private void OnExit()
    {
        if (hideFB != null)
            hideFB.PlayFeedbacks();
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked, gameStatus.soundVolume);
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, gameStatus.musicVolume);
        this.gameObject.SetActive(false);
        Hide();
    }

    #region Show Hide
    public void Show()
    {
        if (showFB != null)
            showFB.PlayFeedbacks();
    }
    public void Hide()
    {
        if (hideFB != null)
            hideFB.PlayFeedbacks();
    }
    #endregion
}
