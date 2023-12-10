using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NOOD;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviorInstance<DayTimeController>
{
    private List<TimeAgent> agents;

    #region Events
    public Action<int> onNextDay;
    public Action onNightTime;
    public Action onDayTime;
    #endregion

    #region Const
    const float secondsInDay = 86000f;
    const float phaseLength = 900f; // 15 minutes chunk of time
    const float phaseInDay = 96f ;//secondsInDay divided by phaseLength
    #endregion

    #region SerializeField
    [SerializeField] private Color nightLightColor;
    [SerializeField] private Color dayLightColor = Color.white;
    [SerializeField] private AnimationCurve nightTimeCurve;
    [SerializeField] float timeScale = 60f;
    [SerializeField] private float startAtTime = 28800f; // in second
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] Light2D globalLight;
    #endregion

    #region Private 
    private int days;
    private float time;
    private bool _isNight, _isDay = true; // _isDay = true because game start at morning
    #endregion

    private void Awake()
    {
        agents = new List<TimeAgent>();
    }
    private void Start()
    {
        time = startAtTime;
    }

    public void Subscribe(TimeAgent timeAgent)
    {
        agents.Add(timeAgent);
    }

    public void Unsubscribe(TimeAgent timeAgent)
    {
        agents.Remove(timeAgent);
    }
    private float Hours
    {
        get { return time / 3600f; }
    }

    private float Minutes
    {
        get { return time % 3600f / 60f; }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.C))
        {
            time += Time.deltaTime * timeScale * 100;
        }
        else
        {
            time += Time.deltaTime * timeScale;
        }

        TimeValueCalculation();
        DayLight();

        if (time > secondsInDay)
        {
            NextDay();
        }
        TimeAgents();
    }

    private void TimeValueCalculation()
    {
        int hh = (int)Hours;
        int mm = (int)Minutes;
        text.text = hh.ToString("00") + ":" + mm.ToString("00");
    }

    private int oldPhase = -1;
    private void TimeAgents()
    {
        if(oldPhase == 1)
        {
            oldPhase = CalculatePhase();
        }

        int currentPhase = CalculatePhase();
        while (oldPhase < currentPhase)
        {
            oldPhase += 1;
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Invoke((int)Hours, (int)Minutes);
            }
        }
    }

    private int CalculatePhase()
    {
        return (int)(time / phaseLength) + (int)(days*phaseInDay);
    }

    private void DayLight()
    {
        float v = nightTimeCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
        if(Hours > 4 && _isDay == false)
        {
            Debug.Log("OnDay");
            _isDay = true;
            onDayTime?.Invoke();
        }
        if(Hours > 18 && _isNight == false)
        {
            Debug.Log("OnNight");
            _isNight = true;
            onNightTime?.Invoke();
        }
        // float intensity = Mathf.Lerp(1, 0.5f, v);
        //globalLight.intensity = intensity;
    }

    private void NextDay()
    {
        time = 0;
        days += 1;
        _isDay = false;
        _isNight = false;
        onNextDay?.Invoke(days);
    }
}
