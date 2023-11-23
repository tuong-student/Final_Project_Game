using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.Events;

public class OptionSystem : MonoBehaviour
{
    [SerializeField] private OptionUI _optionUI;
    [SerializeField] private List<OptionDataSO> _optionDatas = new List<OptionDataSO>();
    private int _chosenIndex;

    private void OnDisable()
    {
        ClearListener();
    }

    public void ActiveOptionPanel()
    {
        _optionUI.gameObject.SetActive(true);
        _optionUI.DisplayOption(_optionDatas);
        GlobalConfig._isBlockInput = true;
        GameInput.onPlayerPressMoveVector2 += ChooseOption;
        GameInput.onPlayerAccept += PlayerAccept;
    }

    private void ChooseOption(Vector2 playerInput)
    {
        if(playerInput.y == 0) return;

        if(playerInput.y > 0)
        {
            _chosenIndex -= 1;
        }
        if(playerInput.y < 0)
        {
            _chosenIndex += 1;
        }

        _chosenIndex = _chosenIndex > _optionDatas.Count - 1 ? 0 : _chosenIndex;
        _chosenIndex = _chosenIndex < 0 ? _optionDatas.Count - 1 : _chosenIndex;
        Debug.Log("index " + _chosenIndex);
        _optionUI.ChooseOption(_chosenIndex);
    }

    private void PlayerAccept()
    {
        OptionDataSO chooseOption = _optionDatas[_chosenIndex];
        switch (chooseOption._actionToActive)
        {
            case ActionType.Shop:
                Debug.Log("Shop");
                break;
            case ActionType.Close:
                Debug.Log("Close");
                GlobalConfig._isBlockInput = false;
                break;
        }
        Close();
        ClearListener();
    }

    private void ClearListener()
    {
        GameInput.onPlayerPressMoveVector2 -= ChooseOption;
        GameInput.onPlayerAccept -= PlayerAccept;
    }

    private void Close()
    {
        this.gameObject.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBounce);
    }
}
