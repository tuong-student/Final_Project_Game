using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using NOOD.Extension;
using Game;

public class Bat : BaseEnemy
{
    private CropTile _targetCropTile = null;

    protected override void Update()
    {
        base.Update();
        if(_isTest) return;
        Move();
    }

    protected override void FindCrop()
    {
        if(_targetCropTile == null)
        {
            CropsContainer cropsContainer = GameObject.Find("CropsTilemap").transform.GetComponent<TilemapCropsManager>().GetCropContainer();
            _targetCropTile = cropsContainer.crops.GetRandom();
            _targetCropTile.OnHarvest += () => 
            {
                Debug.Log("Enemy Harvest");
                _targetCropTile = null;
            };
        }
        else
        {
            _targetPos = _targetCropTile.worldPosition + new Vector3(0.5f, 0.5f, 0f);
        }

        if(_targetCropTile == null)
        {
            // if don't have crop tile attack player instead
            _targetPos = FindObjectOfType<PlayerManager>().transform.position;
        }
    }
    protected override void Move()
    {
        FindCrop();
        Vector3 direction = (_targetPos - this.transform.position).normalized;
        this.transform.position += _moveSpeed * direction * Time.deltaTime;

        if(Vector3.Distance(this.transform.position, _targetPos) > 0.5)
        {
            Attack();
        }
    }
    protected override void Attack()
    {
        // _attackTime will be compute in base class
        if(_attackTime >= _nextAttackTime)
        {
            _targetCropTile.damage += 1;
            base.Attack();
            Debug.Log("Attack");
        }
    }
}
