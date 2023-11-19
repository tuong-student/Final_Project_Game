using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using NOOD.Extension;
using Game;

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
        if(_targetCropTile == null)
        {
            // if don't have crop tile attack player instead
            _targetPos = FindObjectOfType<PlayerManager>().transform.position;
        }

        if(_targetCropTile == null)
        {
            CropsContainer cropsContainer = ShootingManager.Instance._tilemapCropsManager.GetCropContainer();
            _targetCropTile = cropsContainer.crops.GetRandom();
            if(_targetCropTile.growStage == 0)
            {
                _targetCropTile = null;
            }
            else
            {
                _targetCropTile.OnHarvest += () => 
                {
                    _targetCropTile = null;
                };
                _targetPos = _targetCropTile.worldPosition + new Vector3(0.5f, 0.5f, 0f);
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
        if(_targetCropTile != null)
        {
            _targetCropTile.damage += 0.2f;
            if(_targetCropTile.damage >= 1)
            {
                ShootingManager.Instance._tilemapCropsManager.HarvestCropTile(_targetCropTile);
            }
            Debug.Log("Enemy Attack Crop");
        }
        else
        {
            PlayerManager.Instance.MinusHealth(_damage);
        }
    }
}
