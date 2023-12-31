using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using NOOD.SerializableDictionary;
using NOOD;

public enum FeedbackType
{
    CameraShake,
    PlayerHurt,
    PlayerDead,
    PlayerGetItem,
    Shoot,
    EnemyComing,
    NextDay,
}

public class FeedbackManager : MonoBehaviorInstance<FeedbackManager>
{
    [SerializeField] private SerializableDictionary<FeedbackType, MMF_Player> _feedbackDic = new SerializableDictionary<FeedbackType, MMF_Player>();

    private FeedbackType _feedbackType;

    public void PlayPlayerHurtFeedback()
    {
        _feedbackType = FeedbackType.PlayerHurt;
        PlayFeedback();
    }

    public void PlayPlayerShootFB()
    {
        _feedbackType = FeedbackType.Shoot;
        PlayFeedback();
    }

    public void PlayPlayerDeadFB()
    {
        Debug.Log("PlayerDeadFB");
        _feedbackType = FeedbackType.PlayerDead;
        PlayFeedback();
    }

    private void PlayFeedback()
    {
        Debug.Log("PlayFB");
        MMF_Player fb = _feedbackDic.Dictionary[_feedbackType];
        if(fb != null)
            fb.PlayFeedbacks();
    }
}
