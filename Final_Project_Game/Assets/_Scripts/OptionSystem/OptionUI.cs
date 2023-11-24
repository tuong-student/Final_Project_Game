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

public class OptionUI : MonoBehaviorInstance<OptionUI>
{
    [SerializeField] private GameObject _optionPointer;
    [SerializeField] private GameObject _optionPref;
    private List<GameObject> _optionObjects = new List<GameObject>();
    private int _currentIndex;
    private OptionLogic _currentOptionLogic;

    void Awake()
    {
        Close();
    }

    void OnDisable()
    {
        foreach(var obj in _optionObjects)
        {
            obj.SetActive(false);
        }
        _optionObjects.Clear();
        UnSubscribeEvents();
    }

    public void MoveToPosition(Vector3 position)
    {
        this.transform.DOMove(position, 0);
    }

    #region Open Close
    public void Open()
    {
        this.transform.DOScaleY(1, 0.5f).SetEase(Ease.OutCirc);
    }
    public void Close()
    {
        this.transform.DOScaleY(0, 0.5f).SetEase(Ease.InCirc);
    }
    #endregion

    #region Events Function
    private void SubscribeEvents()
    {
        GameInput.onPlayerPressMoveVector2 += PlayerInputHandler;
        GameInput.onPlayerAccept += PlayerAcceptHandler;
    }
    private void UnSubscribeEvents()
    {
        GameInput.onPlayerPressMoveVector2 -= PlayerInputHandler;
        GameInput.onPlayerAccept -= PlayerAcceptHandler;
    }
    #endregion

    #region DisplayOption
    public void DisplayOptions(List<OptionDataSO> optionDataSOs, OptionLogic sender)
    {
        _currentOptionLogic = sender;
        foreach(OptionDataSO optionDataSO in optionDataSOs)
        {
            GameObject optionObject = GetOptionObject();
            optionObject.GetComponentInChildren<TextMeshProUGUI>().text = optionDataSO._displayText;
            _optionObjects.Add(optionObject);
        }
        _currentIndex = 0;
        UpdateUI();
        SubscribeEvents();
    }

    private GameObject GetOptionObject()
    {
        foreach(Transform transform in this.transform)
        {
            if(transform.gameObject.activeInHierarchy == false)
            {
                transform.gameObject.SetActive(true);
                return transform.gameObject;
            }
        }
        GameObject newObject = Instantiate(_optionPref, this.transform);
        newObject.SetActive(true);
        return newObject;
    }
    #endregion


    #region MovePointer
    private void UpdateUI()
    {
        // Update base on index
        Debug.Log("Player choose index: " + _currentIndex);
        _optionPointer.transform.DOMoveY(_optionObjects[_currentIndex].transform.position.y, 0.3f);
    }
    private void PlayerInputHandler(Vector2 playerInput)
    {
        if(playerInput.y > 1) // Move up
        {
            _currentIndex += 1;
        }
        else if(playerInput.y < 1) // Move down
        {
            _currentIndex -= 1;
        }
        
        _currentIndex = _currentIndex < 0 ? _optionObjects.Count-1 : _currentIndex;
        _currentIndex = _currentIndex > _optionObjects.Count-1 ? 0 : _currentIndex;
        UpdateUI();
    }
    #endregion

    #region PlayerAccept
    private void PlayerAcceptHandler()
    {
        if(_currentOptionLogic == null) return;
        _currentOptionLogic.PlayerAccept(_currentIndex);
        _currentOptionLogic = null;
    }
    #endregion
}
