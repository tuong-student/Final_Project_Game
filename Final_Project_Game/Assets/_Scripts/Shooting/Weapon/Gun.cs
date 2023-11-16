using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using TMPro;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] private GunSO _data;
    [SerializeField] protected float _attackRate = 2;
    public BulletMono _bulletPref;
    public BulletSO _bulletData;
    public Transform _bulletSpawnTrans;
    public float _gunForce = 3;
    private GunHolder _gunHolder;
    protected float _attackTime, _nextAttackTime;
    protected bool _isAuto;
    protected bool _isShot;

    protected virtual void Awake()
    {
        _gunHolder = GetComponentInParent<GunHolder>();
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
        if(_isAuto == true)
        {
            if(_attackTime >= _nextAttackTime)
            {
                Shoot();
                _attackTime = Time.time;
                _nextAttackTime = Time.time + 1/_attackRate;
            }
        }
        else
        {
            StopShooting();
        }
    }

    public abstract void Shoot();
    public abstract void StopShooting();
    protected virtual BulletMono SpawnBullet()
    {
        BulletMono bulletMono = null;
        foreach(Transform child in _gunHolder.BulletTransformHolder)
        {
            if(child.gameObject.activeInHierarchy == false)
            {
                child.gameObject.SetActive(true);
                bulletMono = child.gameObject.GetComponent<BulletMono>();
            }
        }
        if(bulletMono == null)
            bulletMono = Instantiate<BulletMono>(_bulletPref, _gunHolder.BulletTransformHolder);

        bulletMono.SetData(_bulletData);

        return bulletMono;
    }
}
