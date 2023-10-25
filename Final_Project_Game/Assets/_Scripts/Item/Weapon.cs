using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PickUpItem))]
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Item _itemData;
    }

}
