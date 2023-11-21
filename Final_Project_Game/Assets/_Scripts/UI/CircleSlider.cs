using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
    #region Events
    public Action onComplete;
    public Action onValueChange;
    #endregion

    [SerializeField] private Image _sliderContent;
    [SerializeField] private MMF_Player _popAnimation;

    private Color _originalColor;
    private float _value, _maxValue;
    public float Value => _value;
    public float MaxValue => _maxValue;
    public float AnimationTime 
    {
        get
        {
            return _popAnimation.TotalDuration;
        }
    }

    void Awake()
    {
        _originalColor = _sliderContent.color;
    }

    void OnDestroy()
    {
        _popAnimation.Events.OnComplete.RemoveAllListeners();
    }

    public void Init(float value, float maxValue)
    {
        _value = value;
        _maxValue = maxValue;
        UpdateUI();
    }

    public void SetValue(float value)
    {
        _value = value;
        UpdateUI();
    }
    public void SetMaxValue(float value)
    {
        _maxValue = value;
        UpdateUI();
    }
    public void IncreaseValue(float value)
    {
        _value += value;
        UpdateUI();
        if(_value >= _maxValue)
        {
            onComplete?.Invoke();
        }
        onValueChange?.Invoke();
    }
    public void ChangeColor(Color color)
    {
        _sliderContent.color = color;
    }
    
    private void UpdateUI()
    {
        _sliderContent.fillAmount = _value / _maxValue;
        _popAnimation.PlayFeedbacks();
        Debug.Log("Circle Update");
    }
    public void ReturnOldColor()
    {
        _sliderContent.color = _originalColor;
    }
}