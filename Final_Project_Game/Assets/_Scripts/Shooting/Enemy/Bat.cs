using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using NOOD.Extension;
using Game;

public class Bat : BaseEnemy
{
    private CropTile _targetCropTile = null;

    void Update()
    {
        Move();
    }

    protected override void FindTarget()
    {
        if(_targetCropTile == null)
        {
            CropsContainer cropsContainer = GameObject.Find("CropsTilemap").transform.GetComponent<TilemapCropsManager>().GetCropContainer();
            _targetCropTile = cropsContainer.crops.GetRandom();
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
        FindTarget();
        Vector3 direction = (_targetPos - this.transform.position).normalized;
        this.transform.position += _moveSpeed * direction * Time.deltaTime;
    }

    protected override void PlayDeadEffect()
    {
    }

}
