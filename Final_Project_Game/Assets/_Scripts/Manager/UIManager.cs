using System.Collections.Generic;
using UnityEngine;
using NOOD;

namespace Game
{

    public class UIManager : MonoBehaviorInstance<UIManager>
    {
        private static List<object> _uiList = new List<object>();
        [SerializeField] private SettingPanel settingPanel;
        private bool _isSettingShow;

        #region Unity functions
        void Start()
        {
            GameInput.onPlayerPressEscape += ShowHideSetting;
        }
        void OnDisable()
        {
            _uiList.Clear();
            GlobalConfig._isBlockInput = false;
            NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
        }
        #endregion

        #region UI Control
        private void ShowHideSetting()
        {
            _isSettingShow = !_isSettingShow;
            if (_isSettingShow)
                settingPanel.Show();
            else
                settingPanel.Hide();
        }
        #endregion

        #region UIList
        public void AddToUIList(object obj)
        {
            if(_uiList.Contains(obj) == false)
                _uiList.Add(obj);
            GlobalConfig._isBlockInput = _uiList.Count > 0;
        }
        public void RemoveToUIList(object obj)
        {
            if(_uiList.Contains(obj) == true)
                _uiList.Remove(obj);
            GlobalConfig._isBlockInput = _uiList.Count > 0;
        }
        #endregion
    }
}
