using DG.Tweening;
using Game;
using ImpossibleOdds;
using NOOD;
using NOOD.SerializableDictionary;
using NOOD.Sound;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
    #region SerializeField
    [SerializeField] private SerializableDictionary<PointerType, Sprite> _pointerSpriteDic = new SerializableDictionary<PointerType, Sprite>();
    [Space(20)]
    [SerializeField] private Vector3 _normalScale;
    [SerializeField] private CanvasGroup _infoPanelCanvasGroup;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _holdIcon;
    #endregion
    private Image _mouseImage;
    private PointerType _pointerStage;
    private GameObject _currentInteractableGameObject;

    #region Unity Functions
    void Awake()
    {
        // GameInput events
        // Left mouse Button
        GameInput.onPlayerChooseOption += PlayerChoose;
        GameInput.onPlayerReleaseAccept += PlayerReleaseChoose;

        _mouseImage = GetComponent<Image>();
        Cursor.visible = false;
        _pointerStage = PointerType.Idle;
        _mouseImage.transform.localScale = _normalScale;
        _infoPanelCanvasGroup.alpha = 0;
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
                }
            }
            else
            {
                if(_pointerStage != PointerType.Idle)
                {
                    _pointerStage = PointerType.Idle;
                }
            }
        }
        else
        {
            if(_pointerStage != PointerType.Idle)
            {
                _pointerStage = PointerType.Idle;
            }

            HideInfoPanel();
        }

        if(NoodyCustomCode.IsPointerOverUIElement())
        {
            if(_currentInteractableGameObject.TryGetComponent<IDisplayInfo>(out IDisplayInfo displayInfo))
            {
                SetInfo(displayInfo.GetName().Item1, displayInfo.GetName().Item2);
            }
            else
            {
                HideInfoPanel();
            }
        }
        else
        {
            HideInfoPanel();
        }

        ChangePointerScale();
        ChangePointerSprite();
    }
    void OnDestroy()
    {
        NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
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
        _mouseImage.sprite = _pointerSpriteDic.Dictionary[_pointerStage];
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

    #region Info
    public void SetInfo(string name, Color color)
    {
        if (name.IsNullOrEmpty()) return;
        _nameText.text = name;
        _nameText.color = color;
        ShowInfoPanel();
    }
    public void ShowInfoPanel()
    {
        _infoPanelCanvasGroup.DOFade(1, 0.5f);
    }
    public void HideInfoPanel()
    {
        _infoPanelCanvasGroup.DOFade(0, 0.5f);
    }
    public void SetHoldItem(Storable storable)
    {
        if (storable != null)
        {
            _holdIcon.gameObject.SetActive(true);
            _holdIcon.sprite = storable.IconImage;
        }
        else
            _holdIcon.gameObject.SetActive(false);
    }
    #endregion
}
