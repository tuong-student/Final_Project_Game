using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[Serializable]
public class CropTile
{
    public Action OnHarvest;
    public Action<CropTile> OnDamage;
    public int growTimer;
    public int growStage;
    public Crop crop;
    public SpriteRenderer Renderer => _cropSprite.Renderer;
    public bool IsHasView => _cropSprite != null;
    private CropSprite _cropSprite;
    private float damage;
    public float Damage
    {
        get
        {
            return damage;
        }  
        set
        {
            damage = value;
            _cropSprite.CropDamageHandler(!Complete, damage, 1, growTimer, crop.timeToGrow);
            OnDamage?.Invoke(this);
        }
    }
    public Vector3Int position;
    public Vector3 worldPosition;
    public bool Complete
    {
        get
        {
            if (crop == null)
                return false;
          return  growTimer >= crop.timeToGrow;
        }
    }

    internal void Harvested()
    {
        OnHarvest?.Invoke();
        growTimer = 0;
        growStage = 0;
        Damage = 0;
        crop = null;
        OnHarvest = null;
        _cropSprite.Renderer.sprite = null;
    }

    public void SetView(CropSprite cropSprite)
    {
        _cropSprite = cropSprite;
    }

    public void ActiveCropSlider(bool value)
    {
        _cropSprite.ActiveCropSlider(value);
    }
    public void ActiveHarvestIcon(bool value)
    {
        _cropSprite.ActiveHarvestIcon(value);
    }
    public void UpdateCropSlider() 
    {
        if(Complete == false)
        {
            _cropSprite.ReturnToOldColor();
            _cropSprite.UpdateCropSlider(growTimer, crop.timeToGrow);
        }
    }
    public void DestroyView()
    {
        _cropSprite.DestroySelf();
    }
}

public class CropsManager : MonoBehaviour
{
    public TilemapCropsManager cropsManager;
    public void PickUp(Vector3Int position)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("No crops manager");
            return;
        }
        cropsManager.PickUp(position);
    }

    public bool Check(Vector3Int position)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("No crops manager");
            return false;
        }
        return cropsManager.Check(position);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("No crops manager");
            return;
        }
        cropsManager.Seed(position, toSeed);
    }
    public void Plow(Vector3Int position)
    {
        if (cropsManager == null)
        {
            Debug.LogWarning("No crops manager");
            return;
        }
        cropsManager.Plow(position);
    }
}
