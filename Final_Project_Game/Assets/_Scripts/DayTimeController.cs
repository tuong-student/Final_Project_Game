using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{
    const float secondsinday = 86000f;
    const float phaseLength = 900f; // 15 minutes chunk of time

    [SerializeField] private Color nightLightColor;
    [SerializeField] private Color dayLightColor = Color.white;
    [SerializeField] private AnimationCurve nightTimeCurve;
    
    float time;

    [SerializeField] float timeScale = 60f;
    [SerializeField] private float startAtTime = 28800f; // in second
    [SerializeField] private Text text;
    [SerializeField] Light2D globalLight;
    private int day;

    private List<TimeAgent> agents;
    private void Awake()
    {
        agents = new List<TimeAgent>();
    }
    private void Start()
    {
        time = startAtTime;
    }

    public void Subcribe(TimeAgent timeAgent)
    {
        agents.Add(timeAgent);
    }

    public void Unsubcribe(TimeAgent timeAgent)
    {
        agents.Remove(timeAgent);
    }
    private float hours
    {
        get { return time / 3600f; }
    }

    private float minutes
    {
        get { return time % 3600f / 60f; }
    }

    private void Update()
    {
        time += Time.deltaTime *timeScale;

        TimeValueCalculation();
        DayLight();

        if (time > secondsinday)
        {
            NextDay();
        }
        TimeAgents();
    }

    private void TimeValueCalculation()
    {
        int hh = (int)hours;
        int mm = (int)minutes;
        text.text = hh.ToString("00") + ":" + mm.ToString("00");
    }

    private int oldPhase = 0;
    private void TimeAgents()
    {
        int currentPhase = (int) (time / phaseLength);
        if(oldPhase != currentPhase)
        {
            oldPhase = currentPhase;
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Invoke();
            }
        }
    }
    private void DayLight()
    {
        float v = nightTimeCurve.Evaluate(hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
    }
    private void NextDay()
    {
        time = 0;
        day += 1;
     }
}
