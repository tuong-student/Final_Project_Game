using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Text numText;
    [SerializeField] private Slider bar;


    public void SetSlideValue(int cur, int max)
    {
        bar.maxValue = max;
        bar.value = cur;

        numText.text = cur.ToString() + "/" + max.ToString();
    }
}
