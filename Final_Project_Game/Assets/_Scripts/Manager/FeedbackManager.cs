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
}

public class FeedbackManager : MonoBehaviorInstance<FeedbackManager>
{
    [SerializeField] private SerializableDictionary<FeedbackType, MMF_Player> _feedbackDic = new NOOD.SerializableDictionary.SerializableDictionary<FeedbackType, MMF_Player>();
}
