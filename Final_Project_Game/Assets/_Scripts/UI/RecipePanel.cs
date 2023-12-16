using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class RecipePanel : ItemPanel
{
    [SerializeField] RecipeList recipeList;
    [SerializeField] GameObject infoCrafting;
    [SerializeField] SetInfoCrafting setInfoCrafting;

    public override void InitButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < buttons.Count && i < recipeList.recipes.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].SetItem(recipeList.recipes[i].output);
        }
    }

    public override void OnClick(int id)
    {
        if (id >= recipeList.recipes.Count)
            return;
        infoCrafting.SetActive(true);
        setInfoCrafting.SetInfo(recipeList.recipes[id]);
    }
}
