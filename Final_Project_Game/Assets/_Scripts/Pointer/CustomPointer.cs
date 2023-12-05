using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using Game.Interface;
using NOOD;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum PointerType
{
    Idle,
    Choose,
    Press,
    MenuChoose,
}

public class CustomPointer : MonoBehaviorInstance<CustomPointer>
{
    [SerializeField] private SerializedDictionary<PointerType, Sprite> _pointerSpriteDic = new SerializedDictionary<PointerType, Sprite>();
    [SerializeField] private Vector3 _normalScale, _menuChooseScale;
    private Image _mouseImage;
    private PointerType _pointerStage;
    private GameObject _currentInteractableGameObject;

    #region Unity Functions
    void Awake()
    {
        _mouseImage = GetComponent<Image>();
        Cursor.visible = false;
        _pointerStage = PointerType.Idle;
        _mouseImage.transform.localScale = _normalScale;
        GameInput.onPlayerAccept += PlayerChoose;
        GameInput.onPlayerReleaseAccept += PlayerReleaseChoose;
    }
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        _mouseImage.transform.position = mousePos;

        _currentInteractableGameObject = NoodyCustomCode.GetCurrentPointObject();
        if(_currentInteractableGameObject != null)
        {
            if(_currentInteractableGameObject.TryGetComponent<IPointerInteractable>(out IPointerInteractable interactable))
            {
                if(_pointerStage != PointerType.Choose)
                {
                    _pointerStage = PointerType.Choose;
                    Debug.Log("Interactable");
                }
            }
            else if(_currentInteractableGameObject.TryGetComponent<IPointerMenu>(out IPointerMenu component) )
            {
                if(_pointerStage != PointerType.MenuChoose)
                {
                    _pointerStage = PointerType.MenuChoose;
                    Debug.Log("Menu");
                }
            }
            else
            {
                if(_pointerStage != PointerType.Idle)
                {
                    Debug.Log("Idle");
                    _pointerStage = PointerType.Idle;
                }
            }
        }
        else
        if(_pointerStage != PointerType.Idle)
        {
            _pointerStage = PointerType.Idle;
        }

        ChangePointerScale();
        ChangePointerSprite();
    }
    void OnDestroy()
    {
        GameInput.onPlayerAccept -= PlayerChoose;
        GameInput.onPlayerReleaseAccept -= PlayerReleaseChoose;
    }
    #endregion

    #region Events Functions
    private void PlayerChoose()
    {
        if(_pointerStage == PointerType.Choose)
        {
            _pointerStage = PointerType.Press;
        }
    }
    private void PlayerReleaseChoose()
    {
        if(_pointerStage == PointerType.Press)
        {
            _pointerStage = PointerType.Choose;
        }
    }
    #endregion

    #region Sprite
    private void ChangePointerSprite()
    {
        if(_pointerStage != PointerType.MenuChoose)
            _mouseImage.sprite = _pointerSpriteDic[_pointerStage];
    }
    #endregion

    #region Scale
    private void ChangePointerScale()
    {
        switch (_pointerStage)
        {
            case PointerType.Idle:
            case PointerType.Choose:
            case PointerType.Press:
                _mouseImage.transform.localScale = _normalScale;
                break;
        }
    }
    #endregion
}
