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
    public SpriteRenderer renderer;
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
        crop = null;
        renderer.gameObject.SetActive(false);
        Damage = 0;
        OnHarvest = null;
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
