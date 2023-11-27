using System.Collections;
using System.Collections.Generic;
using ImpossibleOdds.Http;
using MoreMountains.Feedbacks;
using NOOD;
using NOOD.SerializableDictionary;
using TMPro;
using UnityEngine;

public enum WarningType
{
    NotEnoughMoney,
}

public class WarningUI : MonoBehaviorInstance<WarningUI>
{
    [SerializeField] private MMF_Player _waringActiveFB, _warningShakeFB;
    [SerializeField] private TextMeshProUGUI _warningText;
    [SerializeField] private SerializableDictionary<WarningType, string> _warningTextDic = new SerializableDictionary<WarningType, string>();
    private CanvasGroup _canvasGroup;
    private bool _isShowing;
    private WarningType _currentWarningType;

    #region Unity Functions
    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _waringActiveFB.Events.OnComplete.AddListener(IsShowFalse);
    }
    void Start()
    {
        _canvasGroup.alpha = 0;
    }
    #endregion

    #region Public functions
    public void ShowWarning(WarningType warningType)
    {
        _canvasGroup.alpha = 1;
        if(_isShowing == false)
        {
            _isShowing = true;
            _waringActiveFB.PlayFeedbacks();
        }
        switch (warningType)
        {
            case WarningType.NotEnoughMoney:
                DisplayNotEnoughMoney();
                break;
        }
    }
    #endregion

    #region Warning function
    private void DisplayNotEnoughMoney()
    {
        if(_currentWarningType == WarningType.NotEnoughMoney)
        {
            _warningShakeFB.PlayFeedbacks();
        }
        else
        {
            _warningText.text = _warningTextDic.Dictionary[WarningType.NotEnoughMoney];
        }
    }
    #endregion

    #region SupportFunctions
    private void IsShowFalse()
    {
        _isShowing = false;
    }
    #endregion
}
