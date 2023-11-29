using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WeatherStates
{
    Clear,
    Rain,
    HeavyRain,
    RainAndThunder
}


public class WeatherManager : TimeAgent
{
    [Range(0f, 1f)][SerializeField] private float chanceToChangeWeather = 0.02f;

    WeatherStates currentWeatherState = WeatherStates.Clear;

    [SerializeField] private ParticleSystem rainObject;
    [SerializeField] private ParticleSystem heavyRainObject;
    [SerializeField] private ParticleSystem rainAndThunderObject;

    private void Start()
    {
        Init();
        onTimeTick += RandomWeatherChangeCheck;
        UpdateWeather();
    }

    public void RandomWeatherChangeCheck()
    {
        if (UnityEngine.Random.value < chanceToChangeWeather)
        {
            RandomWeatherChange();
        }
    }

    private void RandomWeatherChange()
    {
        WeatherStates newWeatherState = (WeatherStates)UnityEngine.Random.Range(0, Enum.GetNames(typeof(WeatherStates)).Length);
        ChangeWeather(newWeatherState);
    }

    private void ChangeWeather(WeatherStates newWeatherStates)
    {
        currentWeatherState = newWeatherStates;
        Debug.Log(currentWeatherState);
        UpdateWeather();
    }

    private void UpdateWeather()
    {
        switch (currentWeatherState)
        {
            case WeatherStates.Clear:
                rainObject.gameObject.SetActive(false);
                heavyRainObject.gameObject.SetActive(false);
                rainAndThunderObject.gameObject.SetActive(false);
                break;
            case WeatherStates.Rain:
                rainObject.gameObject.SetActive(true);
                heavyRainObject.gameObject.SetActive(false);
                rainAndThunderObject.gameObject.SetActive(false);
                break;
            case WeatherStates.HeavyRain:
                rainObject.gameObject.SetActive(false);
                heavyRainObject.gameObject.SetActive(true);
                rainAndThunderObject.gameObject.SetActive(false);
                break;
            case WeatherStates.RainAndThunder:
                rainObject.gameObject.SetActive(false);
                heavyRainObject.gameObject.SetActive(false);
                rainAndThunderObject.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
