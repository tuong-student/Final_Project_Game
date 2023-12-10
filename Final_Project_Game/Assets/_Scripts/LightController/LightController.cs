using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum LightType
{
    OnWhenMorning,
    OffWhenMorning,
    OnWhenEvening,
    OffWhenEvening,
}   

public class LightController : MonoBehaviorInstance<LightController>
{
    [SerializeField] private List<Light2D> _onWhenMorning = new List<Light2D>();
    [SerializeField] private List<Light2D> _offWhenMorning = new List<Light2D>();
    [SerializeField] private List<Light2D> _onWhenEvening = new List<Light2D>();
    [SerializeField] private List<Light2D> _offWhenEvening = new List<Light2D>();

    void Start()
    {
        DayTimeController.Instance.onDayTime += OffWhenMorning;
        DayTimeController.Instance.onNightTime += OnWhenEvening;
    }

    void OnDisable()
    {
        NoodyCustomCode.UnSubscribeAllEvent<DayTimeController>(this);
    }

    public void AddToLightList(Light2D light, LightType lightType)
    {
        switch (lightType)
        {
            case LightType.OnWhenMorning:
                _onWhenMorning.Add(light);
                break;
            case LightType.OffWhenMorning:
                _offWhenMorning.Add(light);
                break;
            case LightType.OnWhenEvening:
                _onWhenEvening.Add(light);
                break;
            case LightType.OffWhenEvening:
                _offWhenEvening.Add(light);
                break;
        }
    }

    private void OffWhenMorning()
    {
        foreach(Light2D light in _offWhenMorning)
        {
            if(light == true)
            {
                NoodyCustomCode.StartUpdater(() =>
                {
                    float intensity = light.intensity;
                    intensity -= Time.deltaTime;
                    light.intensity = intensity;

                    return intensity <= 0;
                });
            }
        }
    }

    private void OnWhenEvening()
    {
        foreach(Light2D light in _onWhenEvening)
        {
            if(light == true)
            {
                light.enabled = true;
                light.intensity = 0;
                NoodyCustomCode.StartUpdater(() =>
                {
                    float intensity = light.intensity;
                    intensity += Time.deltaTime;
                    light.intensity = intensity;

                    return intensity >= 1;
                });
            }
        }
    }
}
