using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/GunSO")]
public class GunSO : ScriptableObject
{
    public Animator _gunView, _casing, _flash;
    public Sprite _gunIdleSprite;
    public BulletSO _bulletData;

    public BulletSO GetBulletData()
    {
        return _bulletData;
    }
}
