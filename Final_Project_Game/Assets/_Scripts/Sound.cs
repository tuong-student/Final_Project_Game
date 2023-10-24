using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public sound soundType;
}
public enum sound
{
    mainMusic,
    plow,
    buttonClick,
    pickUp,
    hitEnemy,
}
