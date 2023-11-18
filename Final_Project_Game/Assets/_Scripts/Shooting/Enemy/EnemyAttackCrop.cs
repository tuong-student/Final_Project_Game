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
        if(distance < 0.5)
        {
            Attack();
        }
        else
        {
            Move();
        }
    }

    protected override void FindCrop()
    {
        if(_targetCropTile == null)
        {
            CropsContainer cropsContainer = GameObject.Find("CropsTilemap").transform.GetComponent<TilemapCropsManager>().GetCropContainer();
            _targetCropTile = cropsContainer.crops.GetRandom();
        }
        else
        {
            if (_targetCropTile.damage < 1)
            {
                _targetCropTile.OnHarvest += () => 
                {
                    Debug.Log("Enemy Harvest");
                    _targetCropTile = null;
                };
                _targetPos = _targetCropTile.worldPosition + new Vector3(0.5f, 0.5f, 0f);
            }
            else
            {
                _targetCropTile = null;
            }
        }

        if(_targetCropTile == null)
        {
            // if don't have crop tile attack player instead
            _targetPos = FindObjectOfType<PlayerManager>().transform.position;
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
            _targetCropTile.damage += 1;
        }
        else
        {
            PlayerManager.Instance.MinusHealth(_damage);
        }
    }
}
