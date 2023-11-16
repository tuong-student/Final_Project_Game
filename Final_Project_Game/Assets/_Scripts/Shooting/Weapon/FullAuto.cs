using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FullAuto : Gun
{
    public MMF_Player  _shootFeedback; 
    public override void Shoot()
    {
        GameObject bullet = SpawnBullet();
        _shootFeedback.PlayFeedbacks();
        bullet.transform.position = _bulletSpawnTrans.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(this.transform.right * _gunForce);
    }
}
