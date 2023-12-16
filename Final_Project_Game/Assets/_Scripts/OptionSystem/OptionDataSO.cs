using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    Shop,
    Close,
    Sell,
    SellOne,
    SellHalf,
    SellAll,
    Buy,
    Buy1,
    Buy10,
    Buy100,
    Craft
}

[CreateAssetMenu(menuName = "Data/Dialogue/OptionData")]
public class OptionDataSO : ScriptableObject
{
    public string _displayText;
    public ActionType _actionToActive;
}
