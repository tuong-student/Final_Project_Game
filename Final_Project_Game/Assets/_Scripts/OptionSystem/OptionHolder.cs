using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionHolder : MonoBehaviour
{
    public List<OptionDataSO> _optionDataSOs = new List<OptionDataSO>();

    public void DisplayOptions(Vector2 positionOnScreen)
    {
        GameObject currentSelectedObj = CustomEventSystem.Instance.LastSelectedObject;
        if (currentSelectedObj == null)
            return;
        OptionUI.Instance.MoveToPosition(positionOnScreen);
        OptionUI.Instance.Open();
        OptionUI.Instance.DisplayOptions(_optionDataSOs, this);
    }

    public void PlayerChooseOption(int index)
    {
        OptionDataSO chosenOption = _optionDataSOs[index];
        OptionLogic.PerformOption(chosenOption);
    }
}
