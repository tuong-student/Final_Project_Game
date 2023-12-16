using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetInfoCrafting : MonoBehaviour
{
    [SerializeField] private Image icon1;
    [SerializeField] private Image icon2;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI amount2;
    [SerializeField] private Button onCrafting;
    [SerializeField] Crafting crafting;
    [SerializeField] private GameObject craftingInfo;
    [SerializeField] private GameObject craftingFail;
    private bool isFail;
    CraftingRecipe recipeCraft;
    

    private void Start()
    {
        onCrafting.onClick.AddListener(OnClickCrafting);
    }

    public void SetInfo(CraftingRecipe recipe)
    {
        isFail = false;
        craftingInfo.SetActive(true);
        craftingFail.SetActive(false);
        icon1.sprite = recipe.elements[0].storable.IconImage;
        icon2.sprite = recipe.elements[1].storable.IconImage;
        amount.text = recipe.elements[0].count.ToString();
        amount2.text = recipe.elements[1].count.ToString();
        recipeCraft = recipe;
    }

    private void OnClickCrafting()
    {
        for (int i = 0; i < recipeCraft.elements.Count; i++)
        {
            if (!PlayerManager.Instance.InventoryContainer.CheckItem(recipeCraft.elements[i]))
            {
                isFail = true;
                break;
            }
        }
        if (isFail)
            StartCoroutine(WaitForTwoSeconds());
        else
        {
            crafting.Craft(recipeCraft);
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator WaitForTwoSeconds()
    {
        Debug.Log("Coroutine started");
        craftingInfo.SetActive(false);
        craftingFail.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }

}
