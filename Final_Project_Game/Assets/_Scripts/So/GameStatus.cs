using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/GameStatus")]
public class GameStatus : ScriptableObject
{
    public bool isNewGame;
    public string nameScene;
}
