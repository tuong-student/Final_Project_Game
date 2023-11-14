using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    private GunHolder _gunHolder;
    public float _gunForce = 3;
    public GameObject _bulletPref;
    public Transform _bulletSpawnTrans;

    protected virtual void Awake()
    {
        _gunHolder = GetComponentInParent<GunHolder>();
    }

    public abstract void Shoot();
    protected virtual GameObject SpawnBullet()
    {
        GameObject bulletGO;
        foreach(Transform child in _gunHolder.BulletTransformHolder)
        {
            if(child.gameObject.activeInHierarchy == false)
            {
                bulletGO = child.gameObject;
                return bulletGO;
            }
        }
        bulletGO = Instantiate(_bulletPref, _gunHolder.BulletTransformHolder);

        return bulletGO;
    }
}
