using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game;
using NOOD;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected GunSO _data;
    public BulletMono _bulletPref;
    public BulletSO _bulletData;
    public Transform _bulletSpawnTrans;
    private GunHolder _gunHolder;
    protected float _attackTime, _nextAttackTime;
    protected bool _isAuto;
    protected bool _isShot;
    public Light2D _flashLight;
    public Animator _gunView, _casing, _flash;
    public SpriteRenderer _gunViewIdle;
    public bool IsHasData => _data != null;

    protected virtual void Awake()
    {
        _gunHolder = GetComponentInParent<GunHolder>();
    }

    public void ChangeGunData(GunSO data)
    {
        Debug.Log("ChangeGunData");
        if(data == null)
        {
            _data = null;
            _gunViewIdle.sprite = null;
            _gunView.runtimeAnimatorController = null;
            _casing.runtimeAnimatorController = null;
            _flash.runtimeAnimatorController = null;
            Hide();
        }
        else
        {
            _data = data;
            ChangeGun();
            Show();
        }
    }

    private void ChangeGun()
    {
        if(_data == null) return;
        _gunViewIdle.sprite = _data._gunIdleSprite;
        _gunView.runtimeAnimatorController = _data._gunViewController;
        _casing.runtimeAnimatorController = _data._casingController;
        _flash.runtimeAnimatorController = _data._flashController;
    }

    protected virtual void Start()
    {
        GameInput.onPlayerShoot += () => 
        {
            _isAuto = true;
        };
        GameInput.onPlayerStopShooting += () => {_isAuto = false;};
        _nextAttackTime = Time.time;
        _attackTime = Time.time;
    }

    private void Update()
    {
        _attackTime += Time.deltaTime;
        //Debug.Log(_data == null);
        if(_isAuto == true && _data != null)
        {
            if(_attackTime >= _nextAttackTime)
            {
                Shoot();
                _flashLight.intensity = 3;
                NoodyCustomCode.StartDelayFunction(() =>
                {
                    _flashLight.intensity = 0;
                }, 0.1f);
                _attackTime = Time.time;
                _nextAttackTime = Time.time + 1/_data._shootingRate;
                FeedbackManager.Instance.PlayPlayerShootFB();
            }
        }
        else
        {
            if(_data != null)
            {
                StopShooting();
            }
        }
    }

    public abstract void Shoot();
    public abstract void StopShooting();

    public void Show()
    {
        this.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
    }
    public void Hide()
    {
        this.transform.DOScale(Vector3.zero, 0.2f);
    }
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
}
