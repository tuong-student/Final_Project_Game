using MoreMountains.Feedbacks;
using NOOD.Sound;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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
        musicSlider.onValueChanged.AddListener(ChangeSoundVolume);

        Hide();
    }

    private void AdjustMusic()
    {
        isMuteMusic = !isMuteMusic;
        if(isMuteSound)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, isMuteMusic?0:1);
    }

    private void AdjustSound()
    {
        isMuteSound = !isMuteSound;
        if(isMuteSound == false)
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
        if (isMuteSound == false)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        gameStatus.isMusicMute = isMuteMusic;
        gameStatus.isSoundMute = isMuteSound;
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, gameStatus.isMusicMute?0:1);
        Hide();
    }

    private void OnExit()
    {
        if (hideFB != null)
            hideFB.PlayFeedbacks();
        if(isMuteSound == false)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);

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
