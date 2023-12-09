using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/GameStatus")]
public class GameStatusSO : ScriptableObject
{
    public bool isNewGame;
    public string nameScene;
    public bool isMusicMute;
    public bool isSoundMute;
}
