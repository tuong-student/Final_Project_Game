using System.Collections;
using System.Collections.Generic;
using NOOD;
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
        }
        else
        {
            GameObject canvas = Instantiate(_fieldCanvas, null);
            canvas.transform.position = cropTile.worldPosition;
            canvas.transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y + 0.3f, canvas.transform.position.z);
            circleSlider = canvas.GetComponentInChildren<CircleSlider>();
            circleSlider.Init(0, cropTile.crop.timeToGrow);

            _cropCircleSlider.Add(cropTile, circleSlider);
        }
        ShowCropCircleSlider(cropTile, true);
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
            if(cropTile.Complete)
            {
                circleSlider.ChangeColor(NoodyCustomCode.HexToColor("#ff140099"));
                circleSlider.Init(cropTile.Damage, 1);
            }
            else
            {
                circleSlider.ChangeColor(Color.red);
                circleSlider.Init(cropTile.Damage, 1);
                NoodyCustomCode.StartDelayFunction(() => 
                {
                    if(cropTile == null || cropTile.crop == null) return;
                    circleSlider.ReturnOldColor();
                    circleSlider.Init(cropTile.growTimer, cropTile.crop.timeToGrow);
                }, circleSlider.AnimationTime);
            }
        }
    }
    public void ShowCropCircleSlider(CropTile cropTile, bool isShow)
    {
        if(_cropCircleSlider.TryGetValue(cropTile, out CircleSlider circleSlider))
        {
            circleSlider.gameObject.SetActive(isShow);
            if(isShow)
            {
                cropTile.OnDamage += PlayCircleSliderDamage;
                circleSlider.ChangeColor(NoodyCustomCode.HexToColor("#9bfd9b99"));
            }
            else
                cropTile.OnDamage -= PlayCircleSliderDamage;
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
