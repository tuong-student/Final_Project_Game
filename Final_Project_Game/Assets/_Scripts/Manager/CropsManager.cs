using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[Serializable]
public class CropTile
{
    public Action OnHarvest;
    public int growTimer;
    public int growStage;
    public Crop crop;
    public SpriteRenderer renderer;
    public float damage;
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
        damage = 0;
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
