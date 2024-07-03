using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class DialogueCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;

    private Canvas _dialogueCanvas;
    public TMP_Text DialogueBox;

    [HideInInspector] public bool IsTyping;
    public float TypeSpeed;
    private Coroutine _typewritterCoroutine;

    private WaitForSeconds _typeWait;
    private string _currentText;

    private void Start()
    {
        _dialogueCanvas = GetComponent<Canvas>();
        _typeWait = new WaitForSeconds(TypeSpeed);

        ToggleVisibility += OnToggleVisibility;
    }

    public void OnToggleVisibility(bool isVisible)
    {
        DialogueBox.text = string.Empty;
        FullConversation = string.Empty;
        _dialogueCanvas.enabled = isVisible;
    }

    public void StartTypeWritterEffect(string text)
    {
        _currentText = text; // helper variable
        _typewritterCoroutine = StartCoroutine(CharByCharTypewritter(_currentText));
    }

    public void EndTypeWritterEffect()
    {
        StopCoroutine(_typewritterCoroutine);
        SavingDialogue(_currentText);
    }

    private string FullConversation;
    public IEnumerator CharByCharTypewritter(string text)
    {
        if (FullConversation != string.Empty) FullConversation += "\n";
        IsTyping = true;

        int charIndex = 0;
        int tagLength;
        //charIndex = Mathf.Clamp(charIndex, 0, text.Length);

        while (charIndex < text.Length)
        {
            tagLength = CheckForTag(charIndex, text);
            charIndex += tagLength;

            DialogueBox.text = FullConversation + text.Substring(0, charIndex);
            charIndex++;
            yield return _typeWait;
        }

        SavingDialogue(text);
    }

    public void SavingDialogue(string text)
    {
        FullConversation += text;
        DialogueBox.text = FullConversation;
        IsTyping = false;
    }

    //private IEnumerator FullTextTypewritter(string text)
    //{
    //    IsTyping = true;

    //    string originalText = text;
    //    string displayedText;
    //    int tagLength;
    //    int alphaIndex = 0;

    //    while (alphaIndex < originalText.Length)
    //    {
    //        tagLength = CheckForTag(alphaIndex, text);
    //        alphaIndex += tagLength;

    //        DialogueBox.text = originalText;
    //        displayedText = text.Insert(alphaIndex, ALPHA_CHAR);
    //        DialogueBox.text = displayedText;

    //        alphaIndex++;
    //        yield return TypeWait;
    //    }

    //    IsTyping = false;
    //}

    private int CheckForTag(int endIndex, string text)
    {
        if (text[endIndex] != '<') return 0;

        var length = 1;
        do
        {
            endIndex++;
            length++;
        }
        while (text[endIndex] != '>');
        return length;
    }
}
