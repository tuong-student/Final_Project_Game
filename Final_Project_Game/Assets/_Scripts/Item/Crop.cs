using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum CropState
    {
        Seed,
        Prepare,
        Done,
        Dead
    }

    public class Crop : MonoBehaviour
    {
        [SerializeField] private CropState _cropState;
        [SerializeField] private Sprite _seedIcon, _doneIcon, _deadIcon;
        [SerializeField] private ItemSO _itemData;

        public CropState GetCropState()
        {
            return _cropState;
        }

        //public void NextCropState()
        //{
        //    switch(_cropState)
        //    {
        //        case CropState.Seed:
        //            _icon = _seedIcon;
        //            _cropState = CropState.Prepare;
        //            break;
        //        case CropState.Prepare:
        //            _icon = null;
        //            _cropState = CropState.Done;
        //            break;
        //        case CropState.Done:
        //            _icon = _doneIcon;
        //            _cropState = CropState.Dead;
        //            break;;
        //        case CropState.Dead:
        //            _icon = _deadIcon;
        //            break;
        //    }
        //}
    }
}
