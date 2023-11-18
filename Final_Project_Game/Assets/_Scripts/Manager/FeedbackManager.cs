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
    PlayerGetItem,
    BulletExplode,
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

    public void PlayPlayerBulletExplodeFB()
    {
        _feedbackType = FeedbackType.BulletExplode;
        PlayFeedback();
    }

    private void PlayFeedback()
    {
        MMF_Player fb = _feedbackDic.Dictionary[_feedbackType];
        fb.PlayFeedbacks();
    }
}
