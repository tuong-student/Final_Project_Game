using System;
using System.Collections;
using System.Collections.Generic;
using NOOD.Sound;
using UnityEngine;

public class TalkInteract : Interactable
{
    public Action<bool> OnTalkInteracted;

    [SerializeField] DialogueContainer dialogue;
    public override void Interact(Character character)
    {
        GameManager.instance.dialogueSystem.Initialize(dialogue,this.gameObject.GetComponent<OptionHolder>());
        SoundManager.PlaySound(NOOD.Sound.SoundEnum.InteractClick);
        OnTalkInteracted?.Invoke(true);
        OptionLogic.OnPlayerChooseClose += OnPlayerChooseCloseHandler;
    }

    private void OnPlayerChooseCloseHandler()
    {
        OnTalkInteracted?.Invoke(false);
        OptionLogic.OnPlayerChooseClose -= OnPlayerChooseCloseHandler;
    }
}
