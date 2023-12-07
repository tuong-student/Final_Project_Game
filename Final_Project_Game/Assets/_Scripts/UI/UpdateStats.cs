using Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStats : MonoBehaviour
{
    [SerializeField] private Image hp;
    [SerializeField] private Text coin;
    [SerializeField] private Storable _money;
    [SerializeField] private ItemContainer _itemContainer;
    [SerializeField] private List<Sprite> hpImageList;

    private void Update()
    {
        UpdateCoin(GetMoney());
        UpdateHP(PlayerManager.Instance.GetHealth(), PlayerConfig._maxHealth);
    }
    public void UpdateHP(float hpNow, float maxHp)
    {
        if(hpNow > 0.75 * maxHp)
        {
            hp.sprite = hpImageList[0];
        }
        else if(hpNow > 0.5 * maxHp)
        {
            hp.sprite = hpImageList[1];
        }
        else if(hpNow > 0.25 * maxHp )
        {
            hp.sprite = hpImageList[2];
        }
        else if(hpNow < 0.25 * maxHp && hpNow > 0)
        {
            hp.sprite = hpImageList[3];
        }
        else if (hpNow == 0)
        {
            hp.sprite = hpImageList[4];
        }
    }
    public void UpdateCoin(int amount)
    {
        coin.text = amount.ToString();
    }
    public int GetMoney()
    {
        ItemSlot montySlot = _itemContainer.slots.First(x => x.storable == _money);
        return montySlot.count;
    }
}
