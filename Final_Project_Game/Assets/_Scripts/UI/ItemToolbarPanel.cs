using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolbarPanel : ItemPanel
{
    [SerializeField] ToolbarController toolbarController;
    int currentSelectedTool;

    private void Start()
    {
        Init();
        toolbarController.onChange += Highlight;
        Highlight(0);
    }
    public override void OnClick(int id)
    {
        toolbarController.Set(id);
        toolbarController.UpdateHighlightIcon();
        Highlight(id);
    }

    public int GetToolbarCount()
    {
        return buttons.Count;
    }

    public void Highlight(int id)
    {
        buttons[currentSelectedTool].Highlight(false);
        currentSelectedTool = id;
        buttons[currentSelectedTool].Highlight(true);
    }

    public override void InitButtons()
    {
        base.InitButtons();
        toolbarController.UpdateHighlightIcon();
    }
}
