using NOOD.Sound;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItem : AbstractItem
{
    private bool _itemPreformed;
    #region abstract functions
    public override void PerformAction()
    {
        if (_data == null) return;
        if(_data.StorageType == StorageType.Weapon)
            Shoot();
        if (_data.StorageType == StorageType.FarmItem && _itemPreformed == false)
            ToolPerform();

    }

    public override void StopPerform()
    {
        if (_data == null) return;
        if(_data.StorageType == StorageType.Weapon )
            StopShooting();
        if (_data.StorageType == StorageType.FarmItem)
            StopToolPerform();
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

        // Play sound
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.Shoot);
    }
    private void StopShooting()
    {
        Debug.Log("StopShooting");
        _itemView.SetBool("Play", false);
        _casing.SetBool("Play", false);
        _flash.SetBool("Play", false);
        _itemViewIdle.sprite = _data.IconImage;
    }
    #endregion

    #region Tool functions
    private void ToolPerform()
    {
        _itemPreformed = true;
        _itemView.SetTrigger("Perform");
    }
    private void StopToolPerform()
    {
        _itemPreformed = false;
    }
    #endregion
}
