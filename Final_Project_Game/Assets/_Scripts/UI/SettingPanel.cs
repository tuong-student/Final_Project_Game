using DG.Tweening;
using MoreMountains.Feedbacks;
using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private List<Sprite> listSprite;
    [SerializeField] private GameStatusSO gameStatus;
    [SerializeField] private MMF_Player showFB;
    [SerializeField] private MMF_Player hideFB;
    #endregion
    private bool isMuteMusic;
    private bool isMuteSound;

    void Awake()
    {
        isMuteMusic = gameStatus.isMusicMute;
        isMuteSound = gameStatus.isSoundMute;
    }

    private void Start()
    {
        if (showFB != null)
            showFB.PlayFeedbacks();
        musicBtn.onClick.AddListener(AdjustMusic);
        soundBtn.onClick.AddListener(AdjustSound);
        confirmBtn.onClick.AddListener(OnConfirm);
        exitBtn.onClick.AddListener(OnExit);
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];
        if(isMuteSound == false)
            SoundManager.PlayMusic(NOOD.Sound.MusicEnum.Theme);
    }

    private void AdjustMusic()
    {
        isMuteMusic = !isMuteMusic;
        if(isMuteMusic)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, isMuteMusic?0:1);
    }

    private void AdjustSound()
    {
        isMuteSound = !isMuteSound;
        if(isMuteSound == false)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        soundImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
    }

    private void OnConfirm()
    {
        gameStatus.isMusicMute = isMuteMusic;
        gameStatus.isSoundMute = isMuteSound;
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, gameStatus.isMusicMute?0:1);
        if (hideFB != null)
            hideFB.PlayFeedbacks();
    }

    private void OnExit()
    {
        if (hideFB != null)
            hideFB.PlayFeedbacks();
        if(isMuteSound == false)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        this.gameObject.SetActive(false);
    }

}
