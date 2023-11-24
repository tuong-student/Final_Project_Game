using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.Events;

public class OptionLogic : MonoBehaviour
{
    [SerializeField] private List<OptionDataSO> optionDataSOs = new List<OptionDataSO>();
     
    public void DisplayOption()
    {
        OptionUI.Instance.MoveToPosition(this.transform.position);
        OptionUI.Instance.Open();
        OptionUI.Instance.DisplayOptions(optionDataSOs, this);
    }

    public void PlayerAccept(int index)
    {
        OptionDataSO chosenOption = optionDataSOs[index];
        switch (chosenOption._actionToActive)
        {
            case ActionType.Shop:
                break;
            case ActionType.Close:
                break;
            case ActionType.Sell:
                break;
        }
    }
}
