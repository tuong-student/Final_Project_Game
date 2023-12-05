using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IPointerInteractable
{
    public abstract void Interact(Character character);
}
