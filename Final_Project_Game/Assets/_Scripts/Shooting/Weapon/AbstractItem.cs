using System.Collections;
using System.Data.Common;
using DG.Tweening;
using Game;
using NOOD;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class AbstractItem : MonoBehaviour
{
    #region SerializedField
    [SerializeField] protected Storable _data;
    #endregion

    #region public
    public BulletMono _bulletPref;
    public BulletSO _bulletData;
    public Transform _bulletSpawnTrans;
    public Light2D _flashLight;
    public Animator _itemView, _casing, _flash;
    public SpriteRenderer _itemViewIdle;
    public bool IsHasData => _data != null;
    #endregion

    #region protected
    protected float _performTimer, _nextPerformTime;
    protected bool _isAuto;
    protected bool _isShot;
    protected IHoldableItem HoldableItem 
    {
        get => _data as IHoldableItem;
        set
        {
            _data = value.StorableData;
        }
    }
    #endregion

    #region private
    private AbstractItemHolder _gunHolder;
    #endregion

    #region Unity functions
    protected virtual void Awake()
    {
        _gunHolder = GetComponentInParent<AbstractItemHolder>();
    }
    protected virtual void Start()
    {
        GameInput.onPlayerShoot += () => 
        {
            _isAuto = true;
        };
        GameInput.onPlayerStopShooting += () => {_isAuto = false;};
        _nextPerformTime = Time.time;
        _performTimer = Time.time;
    }
    private void Update()
    {
        _performTimer += Time.deltaTime;
        //Debug.Log(_data == null);
        if(_isAuto == true && _data != null)
        {
            if(_performTimer >= _nextPerformTime)
            {
                PerformAction();
                if(_data.StorageType == StorageType.Weapon)
                {
                    _flashLight.intensity = 3;
                    NoodyCustomCode.StartDelayFunction(() =>
                    {
                        _flashLight.intensity = 0;
                    }, 0.1f);
                }
                _performTimer = Time.time;
                _nextPerformTime = Time.time + 1/HoldableItem.InteractRate;
                FeedbackManager.Instance.PlayPlayerShootFB();
            }
        }
        else
        {
            if(_data != null)
            {
                StopPerform();
            }
        }
    }
    #endregion

    #region Change item
    public void ChangeItemData(IHoldableItem data)
    {
        Debug.Log("ChangeGunData");
        if(data == null)
        {
            _data = null;
            _itemViewIdle.sprite = null;
            _itemView.runtimeAnimatorController = null;
            _casing.runtimeAnimatorController = null;
            _flash.runtimeAnimatorController = null;
            Hide();
        }
        else
        {
            _data = data.StorableData;
            if(data.StorableData.StorageType == StorageType.Weapon)
            {
                // This is gun
                ChangeGun();
            }
            if(data.StorableData.StorageType == StorageType.FarmItem)
            {
                // This is tool
                ChangeTool();
            }
            Show();
        }
    }
    private void ChangeGun()
    {
        if(_data == null) return;
        GunSO gunSO = _data as GunSO;
        _itemViewIdle.sprite = gunSO.Icon;
        _itemView.runtimeAnimatorController = gunSO._gunViewController;
        _casing.runtimeAnimatorController = gunSO._casingController;
        _flash.runtimeAnimatorController = gunSO._flashController;
    }
    private void ChangeTool()
    {
        if (_data == null) return;
        ItemSO itemSO = _data as ItemSO;
        _itemViewIdle.sprite = itemSO.icon;
        if(itemSO.animator != null)
            _itemView.runtimeAnimatorController = itemSO.animator;
        else
            _itemView.runtimeAnimatorController = null;

        _casing.runtimeAnimatorController = null;
        _flash.runtimeAnimatorController = null;
    }
    #endregion

    #region abstract function
    public abstract void PerformAction();
    public abstract void StopPerform();
    #endregion

    #region Show Hide
    public void Show()
    {
        this.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
    }
    public void Hide()
    {
        this.transform.DOScale(Vector3.zero, 0.2f);
    }
    #endregion

    #region Bullet
    protected virtual BulletMono SpawnBullet()
    {
        BulletMono bulletMono = null;
        foreach(Transform child in _gunHolder.BulletTransformHolder)
        {
            if(child.gameObject.activeInHierarchy == false)
            {
                child.gameObject.SetActive(true);
                bulletMono = child.gameObject.GetComponent<BulletMono>();
                break;
            }
        }
        if(bulletMono == null)
            bulletMono = Instantiate<BulletMono>(_bulletPref, _gunHolder.BulletTransformHolder);
        bulletMono.transform.rotation = this.transform.rotation;

        bulletMono.SetData(_bulletData);

        return bulletMono;
    }
    #endregion
}
