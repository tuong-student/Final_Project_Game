using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using MoreMountains.Feedbacks;
using NOOD;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    #region Events
    public UnityEvent OnFinishDialogue;
    public UnityEvent OnInit;
    #endregion
    
    #region Editor parameters Zone
    [SerializeField] MMF_Player _showFeedback, _hideFeedback;
    [SerializeField] TextMeshProUGUI targetText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image portrait;
    [SerializeField] private DialogueOptionUI dialogueOptionUI;
    [SerializeField] private OptionHolder optionHolder;
    [SerializeField] float visibleTextPercent;
    [SerializeField] float timePereLetter = 0.05f;
    #endregion

    #region private
    int currentTextLine;
    DialogueContainer currentDialogue;
    [Range(0f,1f)]
    float totalTimeToType, currentTime;
    string lineToShow;
    private bool _isShow => targetText.gameObject.activeInHierarchy; 
    #endregion

    #region Unity functions
    void OnEnable()
    {
        GameInput.onPlayerChooseOption += PushText;
    }
    void Awake()
    {
        dialogueOptionUI.OnPlayerChoose += () => Show(false);
    }
    private void Update()
    {
        if(_isShow == false) return;
        TypeOutText();
    }
    void OnDisable()
    {
        GameInput.onPlayerChooseOption -= PushText;
    }
    void OnDestroy()
    {
        NoodyCustomCode.UnSubscribeFromStatic(typeof(GameInput), this);
    }
    #endregion

    #region Init
    public void Initialize(DialogueContainer dialogueContainer, OptionHolder holder)
    {
        optionHolder = holder;
        Show(true);
        currentDialogue = dialogueContainer;
        currentTextLine = 0;
        CycleLine();
        UpdatePortrait();
        OnInit?.Invoke();
    }
    #endregion

    #region Update Text
    private void TypeOutText()
    {
        if (visibleTextPercent >= 1f) return;
        currentTime += Time.deltaTime;
        visibleTextPercent = currentTime / totalTimeToType;
        visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0, 1f);
        UpdateText();
    }

    private void UpdateText()
    {
        int letterCount = (int)(lineToShow.Length * visibleTextPercent);
        targetText.text = lineToShow.Substring(0, letterCount);
    }

    private void PushText()
    {
        if (_isShow == false) return;
        if (visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        if (currentTextLine >= currentDialogue.line.Count)
        {
            Conclude();
        }
        else
            CycleLine();
    }

    private void CycleLine()
    {
        lineToShow = currentDialogue.line[currentTextLine];
        totalTimeToType = lineToShow.Length * timePereLetter;
        currentTime = 0f;
        visibleTextPercent = 0f;
        targetText.text = "";
        currentTextLine += 1;
    }
    #endregion

    #region Active Functions
    private void UpdatePortrait()
    {
        portrait.sprite = currentDialogue.actor.portrait;
        nameText.text = currentDialogue.actor.Name;
    }
    private void Conclude()
    {
        Debug.Log("End conversation");
        OnFinishDialogue?.Invoke();
        dialogueOptionUI.gameObject.SetActive(true);
        dialogueOptionUI.Open();
        dialogueOptionUI.DisplayOptions(optionHolder._optionDataSOs, optionHolder);
    }
    #endregion

    #region Open Close
    private void Show(bool v)
    {
        if(v)
        {
            _showFeedback.PlayFeedbacks();
            UIManager.Instance.AddToUIList(this);
        }
        else
        {
            _hideFeedback.PlayFeedbacks();
            UIManager.Instance.RemoveToUIList(this);
        }
    }
    #endregion
}












