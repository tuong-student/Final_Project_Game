using Game;
using ImpossibleOdds.Http;
using MoreMountains.Feedbacks;
using NOOD.Sound;
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
    [SerializeField] private GameObject settingPanel;
    #endregion

    private bool isMuteMusic;
    private bool isMuteSound;
    private bool isShow;

    private void OnEnable()
    {
        isMuteMusic = gameStatus.isMusicMute;
        isMuteSound = gameStatus.isSoundMute;
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];

        SoundManager.GlobalMusicVolume = gameStatus.isMusicMute? 0:1;
        SoundManager.GlobalSoundVolume = gameStatus.isSoundMute? 0:1;
    }
    private void Start()
    {
        musicBtn.onClick.AddListener(AdjustMusic);
        soundBtn.onClick.AddListener(AdjustSound);
        confirmBtn.onClick.AddListener(OnConfirm);
        exitBtn.onClick.AddListener(OnExit);
        hideFB.Events.OnComplete.AddListener(() => settingPanel.SetActive(false));
        showFB.Events.OnPlay.AddListener(() => settingPanel.SetActive(true));

        GameInput.onPlayerPressEscape += OnRequestShowHide;
        if(SoundManager.IsMusicPlaying(NOOD.Sound.MusicEnum.Theme) == false)
        {
            SoundManager.PlayMusic(NOOD.Sound.MusicEnum.Theme, alwaysPlay: false);
        }
    }

    private void AdjustMusic()
    {
        isMuteMusic = !isMuteMusic;
        GlobalConfig._isMusicMute = isMuteMusic;

        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        
        float volume = isMuteMusic ? 0 : 1;
        SoundManager.ChangeMusicVolume(NOOD.Sound.MusicEnum.Theme, volume);
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
    }

    private void AdjustSound()
    {
        isMuteSound = !isMuteSound;
        GlobalConfig._isSoundMute = isMuteSound;
        if(isMuteSound == false)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];
    }

    private void OnConfirm()
    {
        if (isMuteSound == false)
            SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked);
        gameStatus.isMusicMute = isMuteMusic;
        gameStatus.isSoundMute = isMuteSound;
        SoundManager.GlobalSoundVolume = isMuteSound ? 0 : 1;
        SoundManager.GlobalMusicVolume = isMuteMusic ? 0 : 1;
        Hide();
    }

    private void OnExit()
    {
        if(isMuteSound == false)
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
        Debug.Log("Show");
        isShow = true;
        if (showFB != null)
            showFB.PlayFeedbacks();
    }
    public void Hide()
    {
        Debug.Log("Hide");
        isShow = false;
        if (hideFB != null)
            hideFB.PlayFeedbacks();
    }
    #endregion
}
