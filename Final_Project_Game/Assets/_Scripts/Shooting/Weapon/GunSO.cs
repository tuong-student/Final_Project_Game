using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/GunSO")]
[System.Serializable]
public class GunSO : Storable
{
    public float _shootingRate = 2;
    public RuntimeAnimatorController _gunViewController, _casingController, _flashController;
    public Sprite _gunIdleSprite;
    public Sprite _icon;
    public BulletSO _bulletData;
    public float _bulletForce;

    public override bool Stackable => false;

    public override Sprite Icon => _icon;

    public override StorageType StorageType => StorageType.Weapon;

    public override int Id => 0;

    public BulletSO GetBulletData()
    {
        return _bulletData;
    }
}
