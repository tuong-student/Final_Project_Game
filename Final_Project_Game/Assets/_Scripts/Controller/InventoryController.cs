using System.Collections;
using System.Collections.Generic;
using Game;
using NOOD;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] PanelGroup craftingPanel;
    [SerializeField] GameObject toolbarPanel;
    [SerializeField] GameObject additionalPanel;
    [SerializeField] GameObject craftingInfoPanel;

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
        panel.transform.localPosition = new Vector3(0, -19.4152f, 0);
        panel.SetActive(true);
        //statusPanel.SetActive(true);
        toolbarPanel.SetActive(false);
        UIManager.Instance.AddToUIList(this);
    }
    public void Close()
    {
        panel.SetActive(false);
        craftingPanel.Close();
        toolbarPanel.SetActive(true);
        additionalPanel.SetActive(false);
        craftingInfoPanel.SetActive(false);
        UIManager.Instance.RemoveToUIList(this);
    }
    #endregion
}

