using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCropsManager : TimeAgent
{
    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;
    private Tilemap targetTilemap;
    [SerializeField] GameObject cropsSpritePrefab;
    [SerializeField] CropsContainer container;
    [SerializeField] GameObject harvestIconPref;

    private void Start()
    {
        GameManager.instance.GetComponent<CropsManager>().cropsManager = this;
        targetTilemap = GetComponent<Tilemap>();
        onTimeTick += Tick;
        Init();
        VisualizeMap();
    }

    private void OnDestroy()
    {
        for(int i = 0;i< container.crops.Count; i++)
        {
            container.crops[i].renderer = null;
        }
    }

    public CropsContainer GetCropContainer()
    {
        return container;
    }

    public void Tick()
    {
        if (targetTilemap == null)
            return;
        foreach (CropTile cropTile in container.crops)
        {
            if (cropTile.crop == null)
                continue;
            cropTile.damage += 0.02f;
            if (cropTile.damage > 1f)
            {
                cropTile.Harvested();
                targetTilemap.SetTile(cropTile.position, plowed);
                container.SetDisplayHarvestIconValue(cropTile, false);
                continue;
            }
            if (cropTile.Complete)
            {
                if(container.IsDisplayHarvestIconAt(cropTile) == false)
                {
                    Debug.Log("im done");
                    CreateHarvestIcon(cropTile);
                    cropTile.OnHarvest = () => container.DestroyHarvestIcon(cropTile);
                }
                continue;
            }

            cropTile.growTimer += 1;

            Debug.Log("GrowStage: " + cropTile.growStage);
            if (cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                cropTile.renderer.gameObject.SetActive(true);
                cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];
                cropTile.growStage += 1;
            }
        }
    }

    public void CreateHarvestIcon(CropTile cropTile)
    {
        GameObject icon = Instantiate(harvestIconPref, cropTile.worldPosition, Quaternion.identity);
        icon.transform.localScale = Vector3.one * 0.25f;

        container.SetDisplayHarvestIconValue(cropTile, true);
        container.AddHarvestIconDic(cropTile, icon);
    }

    public bool Check(Vector3Int position)
    {
        return container.GetCropTile(position) != null;
    }

    public void Plow(Vector3Int position)
    {
        if (Check(position) == true)
            return;

        CreatePlowedTile(position);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        CropTile tile = container.GetCropTile(position);
        if (tile == null)
            return;

        targetTilemap.SetTile(position, seeded);
        tile.crop = toSeed;
    }

    public void VisualizeTile(CropTile cropTile)
    {
        targetTilemap.SetTile(cropTile.position, cropTile.crop != null ? seeded : plowed);
        if (cropTile.renderer == null)
        {
            GameObject cropSpriteGO = Instantiate(cropsSpritePrefab, transform);
            cropSpriteGO.transform.position = targetTilemap.CellToWorld(cropTile.position);
            cropSpriteGO.transform.position -= Vector3.forward * 0.01f;
            
            cropTile.renderer = cropSpriteGO.GetComponent<SpriteRenderer>();
            cropTile.worldPosition = targetTilemap.CellToWorld(cropTile.position);
        }
        
        bool growing = cropTile.crop != null && cropTile.growTimer >= cropTile.crop.growthStageTime[0];
        cropTile.renderer.gameObject.SetActive(growing);
        if(growing)
            cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage-1];

    }

    public void VisualizeMap()
    {
        for (int i = 0; i < container.crops.Count; i++)
        {
            VisualizeTile(container.crops[i]);
        }
    }

    private void CreatePlowedTile(Vector3Int position)
    {
        CropTile crop = new CropTile();
        container.Add(crop);

        crop.position = position;
        VisualizeTile(crop);

        targetTilemap.SetTile(position, plowed);
    }
    public void PickUp(Vector3Int gridPosition)
    {
        Vector2Int position = (Vector2Int)gridPosition;
        CropTile tile = container.GetCropTile(gridPosition);
        if (tile == null)
            return;
        if (tile.Complete)
        {
            ItemSpawnManager.instance.SpawnItem(
                targetTilemap.CellToWorld(gridPosition),
                null,
                tile.crop.yield,
                tile.crop.count
                );
            tile.Harvested();
            VisualizeTile(tile);
        }
    }
}
