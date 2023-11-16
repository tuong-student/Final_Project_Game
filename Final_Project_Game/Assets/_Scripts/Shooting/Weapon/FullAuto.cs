using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FullAuto : Gun
{
    public Animator _gunView, _casing, _flash;

    public override void Shoot()
    {
        Debug.Log("Child Shoot");
        BulletMono bullet = SpawnBullet();
        _gunView.SetBool("Play", true);
        _casing.SetBool("Play", true);
        _flash.SetBool("Play", true);
        bullet.transform.position = _bulletSpawnTrans.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(this.transform.right * _gunForce);
    }

    public override void StopShooting()
    {
        _gunView.SetBool("Play", false);
        _casing.SetBool("Play", false);
        _flash.SetBool("Play", false);
    }
}
