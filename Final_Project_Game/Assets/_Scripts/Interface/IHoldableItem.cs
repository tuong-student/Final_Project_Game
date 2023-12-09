using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldableItem 
{
    public float InteractRate { get; }
    public Sprite IconImage { get; }
    public Storable StorableData { get; }
}
