using System.Collections.Generic;
using UnityEngine;
using NOOD;
using NOOD.SerializableDictionary;

namespace Game
{

    public class UIManager : MonoBehaviorInstance<UIManager>
    {
        private static List<object> _uiList = new List<object>();

        void OnDisable()
        {
            _uiList.Clear();
            GlobalConfig._isBlockInput = false;
        }

        #region UIList
        public void AddToUIList(object obj)
        {
            _uiList.Add(obj);
            GlobalConfig._isBlockInput = _uiList.Count > 0;
        }
        public void RemoveToUIList(object obj)
        {
            _uiList.Remove(obj);
            GlobalConfig._isBlockInput = _uiList.Count > 0;
        }
        #endregion
    }
}
