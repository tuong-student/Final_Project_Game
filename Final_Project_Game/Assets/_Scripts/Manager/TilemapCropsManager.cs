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
    [SerializeField] CropSprite cropsSpritePrefab;
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
            container.crops[i].DestroyView();
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
            if (cropTile.Damage > 1f)
            {
                HarvestCropTile(cropTile);
                continue;
            }
            cropTile.growTimer += 1;
            cropTile.UpdateCropSlider();

            if (cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                cropTile.Renderer.sprite = cropTile.crop.sprites[cropTile.growStage];
                cropTile.growStage += 1;
                cropTile.growStage = Mathf.Clamp(cropTile.growStage, 0, cropTile.crop.growthStageTime.Count - 1);
            }

            if (cropTile.Complete)
            {
                cropTile.Damage += 0.05f;
                cropTile.ActiveHarvestIcon(true);
                continue;
            }
        }
    }

    public void HarvestCropTile(CropTile cropTile)
    {
        cropTile.Harvested();
        targetTilemap.SetTile(cropTile.position, plowed);
        cropTile.ActiveCropSlider(false);
        cropTile.ActiveHarvestIcon(false);
    }

    public bool Check(Vector3Int position)
    {
        return container.GetCropTile(position) != null;
    }

    public void Plow(Vector3Int position)
    {
        if (Check(position) == true)
            return;

        Debug.Log("Plow");
        CreatePlowedTile(position);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        CropTile tile = container.GetCropTile(position);
        if (tile == null)
            return;

        Debug.Log("Seed");

        targetTilemap.SetTile(position, seeded);
        tile.crop = toSeed;
        tile.ActiveHarvestIcon(false);
        tile.ActiveCropSlider(true);
        tile.UpdateCropSlider();
    }

    public void VisualizeTile(CropTile cropTile)
    {
        targetTilemap.SetTile(cropTile.position, cropTile.crop != null ? seeded : plowed);
        if (cropTile.IsHasView == false)
        {
            CropSprite cropSprite = Instantiate(cropsSpritePrefab, transform);
            cropSprite.transform.position = targetTilemap.CellToWorld(cropTile.position);
            cropSprite.transform.position = new Vector3(cropSprite.transform.position.x + 0.5f, cropSprite.transform.position.y + 0.5f, cropSprite.transform.position.z);
            cropSprite.transform.position -= Vector3.forward * 0.01f;
            
            cropTile.SetView(cropSprite);
            cropTile.worldPosition = targetTilemap.CellToWorld(cropTile.position);
        }
        
        bool growing = cropTile.crop != null && cropTile.growTimer >= cropTile.crop.growthStageTime[0];

        if(growing)
        {
            cropTile.Renderer.sprite = cropTile.crop.sprites[cropTile.growStage-1];
            if (cropTile.Complete)
                cropTile.ActiveHarvestIcon(true);
            else
                cropTile.ActiveHarvestIcon(false);
            cropTile.ActiveCropSlider(true);
            cropTile.UpdateCropSlider();
            Debug.Log("growing");
        }
        else
        {
            if (cropTile.crop != null) cropTile.ActiveCropSlider(true);
            else cropTile.ActiveCropSlider(false);

            cropTile.Renderer.sprite = null;
            cropTile.ActiveHarvestIcon(false);
            Debug.Log("NotGrowing");
        }

    }

    public void VisualizeMap()
    {
        for (int i = 0; i < container.crops.Count; i++)
        {
            CropTile cropTile = container.crops[i];
            VisualizeTile(cropTile);
            if(cropTile.crop != null)
                cropTile.UpdateCropSlider();
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
            HarvestCropTile(tile);
            VisualizeTile(tile);
        }
    }
}
