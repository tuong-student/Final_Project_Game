using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] Text targetText;
    [SerializeField] Text nameText;
    [SerializeField] Image portrait;
    int currentTextLine;
    DialogueContainer currentDialogue;

    [Range(0f,1f)]
    [SerializeField] float visibleTextPercent;
    [SerializeField] float timePereLetter = 0.05f;
    float totalTimeTotype, currentTime;
    string lineToShow;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PushText();
        }
        TypeOutText();
    }

    private void TypeOutText()
    {
        if (visibleTextPercent >= 1f) return;
        currentTime += Time.deltaTime;
        visibleTextPercent = currentTime / totalTimeTotype;
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
        totalTimeTotype = lineToShow.Length * timePereLetter;
        currentTime = 0f;
        visibleTextPercent = 0f;
        targetText.text = "";
        currentTextLine += 1;
    }
    public void Initialize(DialogueContainer dialogueContainer)
    {
        Show(true);
        currentDialogue = dialogueContainer;
        currentTextLine = 0;
        CycleLine();
        UpdatePortrail();
    }

    private void UpdatePortrail()
    {
        portrait.sprite = currentDialogue.actor.portrait;
        nameText.text = currentDialogue.actor.Name;
    }

    private void Show(bool v)
    {
        gameObject.SetActive(v);
    }

    private void Conclude()
    {
        Debug.Log("ending");
        Show(false);
    }
}
