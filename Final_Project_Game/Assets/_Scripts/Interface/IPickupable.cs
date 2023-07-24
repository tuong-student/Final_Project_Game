using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interface
{
    public interface IPickupable 
    {
        void Pickup(Inventory collector);
    }
}
