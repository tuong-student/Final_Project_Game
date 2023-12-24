using System.Security.Cryptography;
using NOOD;
using UnityEngine;

public class CropSprite : MonoBehaviour
{
    [SerializeField] private GameObject _cropSlider;
    [SerializeField] private GameObject _harvestIcon;
    private SpriteRenderer _spriteRenderer;
    private CircleSlider _circleSlider;

    public SpriteRenderer Renderer => _spriteRenderer;

    #region Unity functions
    void Awake()
    {
        _circleSlider = _cropSlider.GetComponentInChildren<CircleSlider>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    #endregion

    #region pubic functions
    public void ActiveHarvestIcon(bool value)
    {
        _harvestIcon.gameObject.SetActive(value);
    }
    public void ActiveCropSlider(bool value)
    {
        _cropSlider.gameObject.SetActive(value);
    }
    public void UpdateCropSlider(float value, float maxValue)
    {
        _circleSlider.SetMaxValue(maxValue);
        _circleSlider.SetValue(value);
    }
    public void CropDamageHandler(bool isReturnToOldColor, float damage, float maxDamage, float growTimer, float timeToGrow)
    {
        if(!isReturnToOldColor)
        {
            _circleSlider.ChangeColor(Color.red);
            UpdateCropSlider(damage, maxDamage);
        }
        else
        {
            _circleSlider.ChangeColor(Color.red);
            UpdateCropSlider(damage, maxDamage);
            NoodyCustomCode.StartDelayFunction(() => 
            {
                if(_cropSlider.activeInHierarchy == false) return;
                _circleSlider.ReturnOldColor();
                _circleSlider.Init(growTimer, timeToGrow);
            },_circleSlider.AnimationTime);
        }
    }
    public void ReturnToOldColor()
    {
        _circleSlider.ReturnOldColor();
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
    #endregion
}