using UnityEngine;

public class FullAuto : Gun
{
    public override void Shoot()
    {
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
        _gunViewIdle.sprite = _data._gunIdleSprite;
    }
}
