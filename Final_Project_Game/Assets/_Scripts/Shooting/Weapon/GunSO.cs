using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/GunSO")]
[System.Serializable]
public class GunSO : Storable, IHoldableItem
{
    #region original parameter
    public float _shootingRate = 2;
    public RuntimeAnimatorController _gunViewController, _casingController, _flashController;
    public Sprite _gunIdleSprite;
    public Sprite _icon;
    public BulletSO _bulletData;
    public float _bulletForce;
    #endregion

    #region Interface parameter
    public override bool Stackable => false;
    public override Sprite Icon => _icon;
    public override StorageType StorageType => StorageType.Weapon;
    public override int Id => 0;
    public override int Price => 9999;
    public override bool Tradable => false;
    public float InteractRate => _shootingRate;
    public Sprite HoldImage => _icon;
    public Storable StorableData => this;
    #endregion

    public BulletSO GetBulletData()
    {
        return _bulletData;
    }
}
