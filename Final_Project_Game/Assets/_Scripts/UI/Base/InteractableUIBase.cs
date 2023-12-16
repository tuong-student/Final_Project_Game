using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Interface;
using NOOD.Sound;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InteractableUIBase : MonoBehaviour, IPointerInteractable, IPointerEnterHandler, IPointerExitHandler
{
    #region abstract functions
    public abstract void Interact(object sender);
    #endregion

    #region Virtual functions
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.Hover);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }
    #endregion
}
