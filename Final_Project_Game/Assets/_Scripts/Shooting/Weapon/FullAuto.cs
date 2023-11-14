using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAuto : Gun
{
    public override void Shoot()
    {
        GameObject bullet = SpawnBullet();
        bullet.transform.position = _bulletSpawnTrans.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(this.transform.right * _gunForce);
    }
}
