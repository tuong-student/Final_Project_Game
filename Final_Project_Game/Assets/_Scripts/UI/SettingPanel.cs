using DG.Tweening;
using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button exitBtn; 
    [SerializeField] private Image musicImg;
    [SerializeField] private Image soundImg;
    [SerializeField] private List<Sprite> listSprite;
    private bool isMuteMusic = GameManager.instance.gamestatus.isMusicMute;
    private bool isMuteSound = GameManager.instance.gamestatus.isSoundMute;
    private DOTween tweenl;

    private void Start()
    {
        transform.DOMoveY(0, 2f);
        musicBtn.onClick.AddListener(AdjustMusic);
        soundBtn.onClick.AddListener(AdjustSound);
        confirmBtn.onClick.AddListener(OnConfirm);
        exitBtn.onClick.AddListener(OnExit);
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        soundImg.sprite = isMuteSound ? listSprite[0] : listSprite[1];
        SoundManager.PlayMusic(NOOD.Sound.MusicEnum.Theme,isMuteMusic);
    }

    private void AdjustMusic()
    {
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked, isMuteSound);
        isMuteMusic = !isMuteMusic;
        musicImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
        SoundManager.AdjustMusicTemporary(isMuteMusic);
    }

    private void AdjustSound()
    {
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked, isMuteSound);
        isMuteSound = !isMuteSound;
        soundImg.sprite = isMuteMusic ? listSprite[0] : listSprite[1];
    }

    private void OnConfirm()
    {
        GameManager.instance.gamestatus.isMusicMute = isMuteMusic;
        GameManager.instance.gamestatus.isSoundMute = isMuteSound;
        SoundManager.AdjustMusicTemporary(GameManager.instance.gamestatus.isMusicMute);
        this.gameObject.transform.DOMoveY(-2000, 1f);
        this.gameObject.SetActive(false);
        
    }

    private void OnExit()
    {
        this.gameObject.transform.DOMoveY(-2000, 1f);
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.ButtonClicked, GameManager.instance.gamestatus.isMusicMute);
        this.gameObject.SetActive(false);
    }

}
