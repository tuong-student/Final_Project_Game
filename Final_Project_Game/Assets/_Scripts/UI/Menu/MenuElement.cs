using DG.Tweening;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(OptionHolder))]
public class MenuElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public OptionHolder _optionHolder;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _price, _quantity;
    private MenuController _parentMenuController;
    private Storable _storableData;
    private ItemSlot _itemSlot;

    void Awake()
    {
        _optionHolder = GetComponent<OptionHolder>();
        _parentMenuController = GetComponentInParent<MenuController>();
    }

    public void SetItemSlot(ItemSlot itemSlot)
    {
        if(itemSlot != null && itemSlot.storable != null)
        {
            // Set new data
            _itemSlot = itemSlot;
            _storableData = itemSlot.storable;
            _icon.color = Color.white;
            _icon.sprite = _storableData.Icon;
            _price.gameObject.SetActive(true);
            _price.text = itemSlot.storable.Price.ToString("0");
            if(_quantity != null)
            {
                _quantity.gameObject.SetActive(true);
                _quantity.text = itemSlot.count.ToString("0"); 
            }
            if(_storableData.Tradable == false)
            {
                this.GetComponent<Button>().interactable = false;
            }
            else
            {
                this.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            // Clear data
            _itemSlot = null;
            _storableData = null;
            _icon.color = new Color(1, 1, 1, 0);
            _price.gameObject.SetActive(false);
            if(_quantity != null)
                _quantity.gameObject.SetActive(false);

            this.GetComponent<Button>().interactable = false;
        }
    }
    public ItemSlot GetItemSlot()
    {
        return _itemSlot;
    }

    public void Select()
    {
        this.transform.DOScale(1.15f, 0.3f).SetEase(Ease.OutCubic);
    }
    public void Deselect()
    {
        this.transform.DOScale(1, 0.3f).SetEase(Ease.InCubic);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = this.gameObject;
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentMenuController.SetLastSelectedObject(this);
        eventData.selectedObject = null;
        Deselect();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselect();
    }
}
