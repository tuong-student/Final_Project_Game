using System.Collections.Generic;
using UnityEngine;
using NOOD;
using Unity.VisualScripting;

namespace Game
{

    public class UIManager : MonoBehaviorInstance<UIManager>
    {
        [SerializeField] private SettingPanel _settingPanel;
        [SerializeField] private RestartMenu _restartMenu;
        private static List<object> s_uiList = new List<object>();
        private bool _isSettingShow;

        #region Unity functions
        void OnDisable()
        {
            s_uiList.Clear();
            GlobalConfig.s_IsUiOpen = false;
            NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
        }
        #endregion

        #region UI Control
        public void ActiveDeadMenu()
        {
            _restartMenu.Show();
        }
        #endregion

        #region UIList
        public void AddToUIList(object obj)
        {
            if(s_uiList.Contains(obj) == false)
                s_uiList.Add(obj);
            GlobalConfig.s_IsUiOpen = s_uiList.Count > 0;
        }
        public void RemoveToUIList(object obj)
        {
            if(s_uiList.Contains(obj) == true)
                s_uiList.Remove(obj);
            GlobalConfig.s_IsUiOpen = s_uiList.Count > 0;
        }
        #endregion
    }
}
