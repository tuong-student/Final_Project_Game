using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolbarPanel : ItemPanel
{
    [SerializeField] ToolbarController toolbarController;

    private void Start()
    {
        Init();
        toolbarController.onChange += Hightlight;
        Hightlight(0);
    }
    public override void OnClick(int id)
    {
        toolbarController.Set(id);
        Hightlight(id);
    }

    int currentSelectedTool;


    public void Hightlight(int id)
    {
        buttons[currentSelectedTool].Highlight(false);
        currentSelectedTool = id;
        buttons[currentSelectedTool].Highlight(true);
    }
}
