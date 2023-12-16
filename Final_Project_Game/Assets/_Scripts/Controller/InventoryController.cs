using System.Collections;
using System.Collections.Generic;
using Game;
using NOOD;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject statusPanel;
    [SerializeField] GameObject toolbarPanel;
    [SerializeField] GameObject additionalPanel;
    [SerializeField] GameObject craftingPanel;

    #region Unity functions
    void Start()
    {
        GameInput.onPlayerOpenInventory += ShowHideInventory;
    }
    void OnDisable()
    {
        NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
    }
    #endregion

    #region Open Close
    private void ShowHideInventory()
    {
        if (!panel.activeInHierarchy)
            Open();
        else Close();
    }
    public void Open()
    {
        panel.SetActive(true);
        //statusPanel.SetActive(true);
        toolbarPanel.SetActive(false);
        UIManager.Instance.AddToUIList(this);
    }
    public void Close()
    {
        panel.SetActive(false);
        statusPanel.SetActive(false);
        toolbarPanel.SetActive(true);
        additionalPanel.SetActive(false);
        craftingPanel.SetActive(false);
        UIManager.Instance.RemoveToUIList(this);
    }
    #endregion
}

