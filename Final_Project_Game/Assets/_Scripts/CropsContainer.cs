using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Crops Container")]
public class CropsContainer : ScriptableObject
{
    public List<CropTile> crops;
    Dictionary<CropTile, bool> DisplayHarvestIconDic = new Dictionary<CropTile, bool>();
    Dictionary<CropTile, GameObject> harvestIconDic = new Dictionary<CropTile, GameObject>();
    public CropTile GetCropTile(Vector3 position)
    {
        return crops.Find(x => x.position == position);
    }
    public void AddHarvestIconDic(CropTile cropTile, GameObject icon)
    {
        if(harvestIconDic.ContainsKey(cropTile))
        {
            harvestIconDic[cropTile] = icon;
        }
        else
            harvestIconDic.Add(cropTile, icon);
    }
    public void DestroyHarvestIcon(CropTile cropTile)
    {
        if(harvestIconDic.TryGetValue(cropTile, out GameObject icon))
        {
            Destroy(icon);
            harvestIconDic.Remove(cropTile);
        }
    }
    public bool IsDisplayHarvestIconAt(CropTile cropTile)
    {
        if(GetCropTile(cropTile.position) != null)
        {
            if(DisplayHarvestIconDic.TryGetValue(cropTile, out bool value))
            {
                return value;
            }
        }
        return false;
    }
    public void SetDisplayHarvestIconValue(CropTile cropTile, bool value)
    {
        if(DisplayHarvestIconDic.ContainsKey(cropTile))
        {
            DisplayHarvestIconDic[cropTile] = value;
        }
        else
            DisplayHarvestIconDic.Add(cropTile, value);
    }
    public void Add(CropTile crop)
    {
        crops.Add(crop);
    }
}
