using UnityEngine;

public class PlayerItem : AbstractItem
{
    #region abstract functions
    public override void PerformAction()
    {
        if(_data.StorageType == StorageType.Weapon)
            Shoot();
        if (_data.StorageType == StorageType.FarmItem)
            ToolPerform();

    }

    public override void StopPerform()
    {
        if(_data.StorageType == StorageType.Weapon)
            StopShooting();
    }
    #endregion

    #region Shoot functions
    private void Shoot()
    {
        GunSO gunSO = (GunSO)_data;
        Debug.Log("Shoot");
        BulletMono bullet = SpawnBullet();
        _itemView.SetBool("Play", true);
        _casing.SetBool("Play", true);
        _flash.SetBool("Play", true);
        bullet.transform.position = _bulletSpawnTrans.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(this.transform.right * gunSO._bulletForce);
    }
    private void StopShooting()
    {
        _itemView.SetBool("Play", false);
        _casing.SetBool("Play", false);
        _flash.SetBool("Play", false);
        _itemViewIdle.sprite = HoldableItem.HoldImage;
    }
    #endregion

    #region Tool functions
    private void ToolPerform()
    {
        _itemView.SetTrigger("Perform");
    }
    #endregion
}
