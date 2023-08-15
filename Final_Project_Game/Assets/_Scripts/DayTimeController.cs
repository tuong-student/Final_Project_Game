using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{
    const float secondsinday = 86000;
    [SerializeField] private Color nightLightColor;
    [SerializeField] private Color dayLightColor = Color.white;
    [SerializeField] private AnimationCurve nightTimeCurve;
    
    float time;

    [SerializeField] float timeScale = 60f;
    [SerializeField] private Text text;
    [SerializeField] Light2D globalLight;
    private int day;

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
        int hh = (int)hours;
        int mm = (int)minutes;
        text.text = hh.ToString("00") + ":" + mm.ToString("00");
        float v = nightTimeCurve.Evaluate(hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
        if (time > secondsinday)
        {
            NextDay();
        }
    }

    private void NextDay()
    {
        time = 0;
        day += 1;
     }
}
