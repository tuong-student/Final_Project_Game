using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using NOOD;
using TMPro;
using Game;
using UnityEngine.PlayerLoop;
using System.Security.Cryptography;
using ImpossibleOdds;
using System;
using Unity.VisualScripting;

public class OptionUI : MonoBehaviorInstance<OptionUI>
{
    [SerializeField] private GameObject _optionPointer;
    [SerializeField] private GameObject _optionPref;
    private List<GameObject> _optionObjects = new List<GameObject>();
    private int _maxIndex;
    private int _currentIndex;
    private OptionHolder _currentOptionHolder;
    public bool IsOpen {get; set;}

    void Awake()
    {
        _optionPref.gameObject.SetActive(false);
    }

    void Start()
    {
        Close();
    }

    void OnDisable()
    {  
        foreach(var obj in _optionObjects)
        {
            obj.SetActive(false);
        }
        UnSubscribeEvents();
    }

    public void MoveToPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    #region Open Close
    public void Open()
    {
        this.transform.DOKill();
        this.transform.DOScaleY(1, 0.5f).SetEase(Ease.OutCirc).OnComplete(UpdateUI);
        if(IsOpen == false)
        {
            IsOpen = true;
            SubscribeEvents();
        }

    }
    public void Close()
    {
        foreach(var obj in _optionObjects)
        {
            obj.SetActive(false);
        }
        this.transform.DOScaleY(0, 0.5f).SetEase(Ease.InCirc);
        UnSubscribeEvents();
        IsOpen = false;
    }
    #endregion

    #region Events Function
    private void SubscribeEvents()
    {
        Debug.Log("Subscribe");
        GameInput.onPlayerPressMoveVector2 += PlayerInputHandler;
        GameInput.onPlayerAccept += PlayerAcceptHandler;
    }
    private void UnSubscribeEvents()
    {
        NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
    }
    #endregion

    #region DisplayOption
    public void DisplayOptions(List<OptionDataSO> optionDataSOs, OptionHolder sender)
    {
        _currentIndex = 0;
        _currentOptionHolder = sender;
        _maxIndex = optionDataSOs.Count - 1;
        foreach(GameObject go in _optionObjects)
        {
            go.SetActive(false);
        }

        for(int i = 0; i < optionDataSOs.Count; i++)
        {
            GameObject optionObject = GetOptionObject(i);
            optionObject.GetComponentInChildren<TextMeshProUGUI>().text = optionDataSOs[i]._displayText;
        }
    }

    private GameObject GetOptionObject(int index)
    {
        GameObject child;
        if(_optionObjects.Count > index)
        {
            child = _optionObjects[index];
            child.SetActive(true);
            return child;
        }
        child = Instantiate(_optionPref, this.transform);
        child.SetActive(true);
        _optionObjects.Add(child);
        return child;
    }
    #endregion


    #region MovePointer
    public void SelectOptionObject(GameObject selectObject)
    {
        _currentIndex = _optionObjects.IndexOf(selectObject);
        UpdateUI();
    }
    private void UpdateUI()
    {
        // Update base on index
        GameObject optionObject = _optionObjects[_currentIndex];
        _optionPointer.transform.DOMoveY(optionObject.transform.position.y, 0.3f);
    }
    private void PlayerInputHandler(Vector2 playerInput)
    {
        if(playerInput.y > 0) // Move up
        {
            _currentIndex--;
        }
        else if(playerInput.y < 0) // Move down
        {
            _currentIndex++;
        }
        
        if(playerInput.y == 0) return;
        _currentIndex = Mathf.Clamp(_currentIndex, 0, _maxIndex);
        UpdateUI();
    }
    #endregion

    #region PlayerAccept
    private void PlayerAcceptHandler()
    {
        _currentOptionHolder.PlayerChooseOption(_currentIndex);
    }
    #endregion
}
