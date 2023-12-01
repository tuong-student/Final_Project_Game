using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSubscribe : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private bool _isOnWhenMorning, _isOffWhenMorning, _isOnWhenEvening, _isOffWhenEvening;

    void OnEnable()
    {
        if(_isOnWhenMorning)
        {
            LightController.Instance.AddToLightList(_light, LightType.OnWhenMorning);
        }
        if(_isOffWhenMorning)
        {
            LightController.Instance.AddToLightList(_light, LightType.OffWhenMorning);
        }
        if(_isOnWhenEvening)
        {
            LightController.Instance.AddToLightList(_light, LightType.OnWhenEvening);
        }
        if(_isOffWhenEvening)
        {
            LightController.Instance.AddToLightList(_light, LightType.OffWhenEvening);
        }
    }
}
