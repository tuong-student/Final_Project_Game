using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Crops Container")]
public class CropsContainer : ScriptableObject
{
    public List<CropTile> crops;
    public CropTile GetCropTile(Vector3 position)
    {
        return crops.Find(x => x.position == position);
    }

    public void Add(CropTile crop)
    {
        crops.Add(crop);
    }
}
