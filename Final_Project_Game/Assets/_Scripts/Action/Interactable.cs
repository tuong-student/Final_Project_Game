using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IPointerInteractable
{
    public virtual void Interact(Character character)
    {

    }

    #region Events
    // void OnMouseOver()
    // {
    //     Debug.Log("OnMouseOver");
    // }
    // protected void OnMouseEnter()
    // {
    //     Debug.Log("OnMouseEnter");
    //     CustomPointer.Instance._currentInteractableGameObject = this.gameObject;
    // }
    // protected void OnMouseExit()
    // {
    //     CustomPointer.Instance._currentInteractableGameObject = null;
    // }
    #endregion
}
