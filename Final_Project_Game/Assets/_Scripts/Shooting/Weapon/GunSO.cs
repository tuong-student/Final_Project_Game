using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/GunSO")]
public class GunSO : ScriptableObject
{
    public float _shootingRate = 2;
    public RuntimeAnimatorController _gunViewController, _casingController, _flashController;
    public Sprite _gunIdleSprite;
    public BulletSO _bulletData;

    public BulletSO GetBulletData()
    {
        return _bulletData;
    }
}
