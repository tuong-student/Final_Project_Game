using UnityEngine;

public class PlayerGun : Gun
{
    public override void Shoot()
    {
        BulletMono bullet = SpawnBullet();
        bullet.transform.position = _bulletSpawnTrans.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(this.transform.right * _data._bulletForce);
        _gunView.SetBool("Play", true);
        _casing.SetBool("Play", true);
        _flash.SetBool("Play", true);
        bullet.ShowTrail();
    }

    public override void StopShooting()
    {
        _gunView.SetBool("Play", false);
        _casing.SetBool("Play", false);
        _flash.SetBool("Play", false);
        _gunViewIdle.sprite = _data._gunIdleSprite;
    }
}
