using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private RectTransform _optionPref;
    [SerializeField] private List<RectTransform> _optionGameObjects = new();
    [SerializeField] private GameObject _pointer;

    private void OnDisable()
    {
        foreach(RectTransform option in _optionGameObjects)
        {
            option.gameObject.SetActive(false);
        }
        _optionGameObjects.Clear();
    }

    public void DisplayOption(List<OptionDataSO> optionDataSOs)
    {   
        foreach(OptionDataSO optionDataSO in optionDataSOs)
        {
            RectTransform optionObject;
            if(_optionGameObjects.Count == 0 || !_optionGameObjects.Any(x => x.gameObject.activeInHierarchy == false))
            {
                optionObject = Instantiate<RectTransform>(_optionPref, this.transform);
                optionObject.GetComponent<TextMeshProUGUI>().text = optionDataSO._displayText;
            }
            else
            {
                optionObject = _optionGameObjects.First(x => x.gameObject.activeInHierarchy == false);
            }
            optionObject.gameObject.SetActive(true);
            optionObject.GetComponent<TextMeshProUGUI>().text = optionDataSO._displayText;
            _optionGameObjects.Add(optionObject);
        }
    }

    public void ChooseOption(int index)
    {
        RectTransform chosenObj = _optionGameObjects[index];
        Debug.Log("option count: " + _optionGameObjects.Count);
        _pointer.transform.DOMoveY(chosenObj.position.y + 20f, 1f);
    }
}
