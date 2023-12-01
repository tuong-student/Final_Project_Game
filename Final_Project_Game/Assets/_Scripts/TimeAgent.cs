using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    public Action onTimeTick;
    public Action<int, int> onDayTimer;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        GameManager.instance.timeController.Subscribe(this);
    }
    public void Invoke(int hour, int minute)
    {
        onTimeTick?.Invoke();
        onDayTimer?.Invoke(hour, minute);
    }

    private void OnDestroy()
    {
        GameManager.instance.timeController.Unsubscribe(this);
    }
}
