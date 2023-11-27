using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Game;
using NOOD;
using System;

public class DialogueOptionUI : MonoBehaviour, IOptionUIBase
{
    #region Events
    public Action OnPlayerChoose;
    #endregion

    [SerializeField] private GameObject _optionPointer;
    [SerializeField] private GameObject _optionPref;
    private List<GameObject> _optionObjects = new List<GameObject>();
    private int _maxIndex;
    private int _currentIndex;
    private OptionHolder _currentOptionHolder;
    private DialogueSystem _parentDialogueSystem;

    #region Unity Events
    void Awake()
    {
        _optionPref.gameObject.SetActive(false);
        _optionPointer.gameObject.SetActive(false);
        OnPlayerChoose += Close;
    }
    void OnDisable()
    {
        Close();
    }
    #endregion

    public void MoveToPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    #region Open Close
    public void Open()
    {
        SubscribeEvents();
        _optionPointer.gameObject.SetActive(true);
    }
    public void Close()
    {
        UnSubscribeEvents();
        this.gameObject.SetActive(false);
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
        OnPlayerChoose?.Invoke();
    }
    #endregion
}
