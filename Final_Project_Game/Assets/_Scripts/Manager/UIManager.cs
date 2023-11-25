using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD;
using NOOD.SerializableDictionary;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Game
{
    public enum UI
    {
        Shop,
    }

    public class UIManager : MonoBehaviorInstance<UIManager>
    {
        [SerializeField] private SerializableDictionary<UI, GameObject> _uiDic = new SerializableDictionary<UI, GameObject>();

        public void LoadUI(UI ui)
        {
            switch (ui)
            {
                case UI.Shop:
                    if(_uiDic.ContainsKey(ui))
                    {
                        _uiDic.Dictionary[ui].SetActive(true);
                    }
                    break;
            }
        }
    }
}
