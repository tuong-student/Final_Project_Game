using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using NOOD.Extension;
using Game;
using System.Linq;

public class EnemyAttackCrop : BaseEnemy
{
    private CropTile _targetCropTile = null;

    protected override void ChildUpdate()
    {
        if(_isTest) return;

        float distance = Vector3.Distance(this.transform.position, _targetPos); 
        if(distance > 0.5 && _isAttacking == false)
        {
            Move();
        }
        else
        {
            Attack();
        }
    }

    protected override void FindCrop()
    {
        if(IsCropNullOrEmpty())
        {
            // if don't have crop tile attack player instead
            _targetPos = FindObjectOfType<PlayerManager>().transform.position;
        }

        if(IsCropNullOrEmpty())
        {
            CropsContainer cropsContainer = ShootingManager.Instance._tilemapCropsManager.GetCropContainer();
            List<CropTile> targetList = cropsContainer.crops.Where(x => x.crop != null).ToList();
            _targetCropTile = targetList.GetRandom();
            if(_targetCropTile == null) return;
            if(_targetCropTile.crop == null)
            {
                _targetCropTile = null;
            }
            else
            {
                _targetPos = _targetCropTile.worldPosition + new Vector3(0.5f, 0.5f, 0f);
                _targetCropTile.OnHarvest += () => 
                {
                    _targetCropTile = null;
                };
            }
        }
        else
        {
            _targetPos = _targetCropTile.worldPosition + new Vector3(0.5f, 0.5f, 0f);
        }
    }
    protected override void Move()
    {
        Vector3 direction = (_targetPos - this.transform.position).normalized;
        this.transform.position += _moveSpeed * direction * Time.deltaTime;
    }

    protected override void ChildAttack()
    {
        if(!IsCropNullOrEmpty())
        {
            _targetCropTile.Damage += 0.2f;
            if(_targetCropTile.Damage >= 1)
            {
                ShootingManager.Instance._tilemapCropsManager.HarvestCropTile(_targetCropTile);
                _targetCropTile = null;
            }
        }
        else
        {
            if(PlayerManager.Instance.GetHealth() > 0)
            {
                PlayerManager.Instance.MinusHealth(_damage);
            }
        }
    }

    private bool IsCropNullOrEmpty() 
    {
        return (_targetCropTile == null || _targetCropTile.crop == null);
    }
}
