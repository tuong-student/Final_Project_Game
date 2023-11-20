using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Crops Container")]
public class CropsContainer : ScriptableObject
{
    public List<CropTile> crops;
    Dictionary<CropTile, bool> _displayHarvestIconDic = new Dictionary<CropTile, bool>();
    Dictionary<CropTile, GameObject> _harvestIconDic = new Dictionary<CropTile, GameObject>();
    Dictionary<CropTile, CircleSlider> _cropCircleSlider = new Dictionary<CropTile, CircleSlider>();
    [SerializeField] private GameObject _fieldCanvas;
    public CropTile GetCropTile(Vector3 position)
    {
        return crops.Find(x => x.position == position);
    }

    public void ClearDatas()
    {
        _displayHarvestIconDic.Clear();
        _harvestIconDic.Clear();
        _cropCircleSlider.Clear();
    }

    #region CropCircleSlider
    public void AddCropCircleSlider(CropTile cropTile)
    {   
        if(_cropCircleSlider.TryGetValue(cropTile, out CircleSlider circleSlider))
        {
            circleSlider = _cropCircleSlider[cropTile];
            circleSlider.Init(0, cropTile.crop.timeToGrow);
            ShowCropCircleSlider(cropTile, true);
        }
        else
        {
            GameObject canvas = Instantiate(_fieldCanvas, null);
            canvas.transform.position = cropTile.worldPosition;
            circleSlider = canvas.GetComponentInChildren<CircleSlider>();
            circleSlider.Init(0, cropTile.crop.timeToGrow);
            cropTile.OnDamage += PlayCircleSliderDamage;

            _cropCircleSlider.Add(cropTile, circleSlider);
        }
    }
    public void UpdateCropCircleSlider(CropTile cropTile)
    {
        if(_cropCircleSlider.TryGetValue(cropTile, out CircleSlider circleSlider))
        {
            circleSlider.SetValue(cropTile.growTimer);
        }
        else
        {
            AddCropCircleSlider(cropTile);
        }
    }
    public void PlayCircleSliderDamage(CropTile cropTile)
    {
        if(_cropCircleSlider.TryGetValue(cropTile, out CircleSlider circleSlider))
        {
            circleSlider.ChangeColor(Color.red);
            circleSlider.Init(cropTile.Damage, 1);
            circleSlider.onAnimationComplete += () => 
            {
                circleSlider.ReturnOldColor();
                circleSlider.Init(cropTile.growTimer, cropTile.crop.timeToGrow);
            };
        }
    }
    public void ShowCropCircleSlider(CropTile cropTile, bool isShow)
    {
        if(_cropCircleSlider.TryGetValue(cropTile, out CircleSlider circleSlider))
        {
            circleSlider.gameObject.SetActive(isShow);
        }
    }
    #endregion

    #region HarvestIcon
    public void AddHarvestIconDic(CropTile cropTile, GameObject icon)
    {
        if(_harvestIconDic.ContainsKey(cropTile))
        {
            _harvestIconDic[cropTile] = icon;
        }
        else
            _harvestIconDic.Add(cropTile, icon);
    }

    public void DestroyHarvestIcon(CropTile cropTile)
    {
        if(_harvestIconDic.TryGetValue(cropTile, out GameObject icon))
        {
            Destroy(icon);
            _harvestIconDic.Remove(cropTile);
        }
    }
    public bool IsDisplayHarvestIconAt(CropTile cropTile)
    {
        if(GetCropTile(cropTile.position) != null)
        {
            if(_displayHarvestIconDic.TryGetValue(cropTile, out bool value))
            {
                return value;
            }
        }
        return false;
    }
    public void SetDisplayHarvestIconValue(CropTile cropTile, bool value)
    {
        if(_displayHarvestIconDic.ContainsKey(cropTile))
        {
            _displayHarvestIconDic[cropTile] = value;
        }
        else
            _displayHarvestIconDic.Add(cropTile, value);
    }
    #endregion
    
    public void Add(CropTile crop)
    {
        crops.Add(crop);
    }
}
