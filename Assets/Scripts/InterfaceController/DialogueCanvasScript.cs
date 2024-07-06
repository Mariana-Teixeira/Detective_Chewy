using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class DialogueCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;

    private Canvas _dialogueCanvas;
    public TMP_Text DialogueBox;
    public AudioSource AudioSource;

    public Button NextNodeButton;
    public Button BackNodeButton;

    [HideInInspector] public bool IsTyping;
    private Coroutine _typewritterCoroutine;
    private WaitForSeconds _typeWait;
    public float TypeSpeed;
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
        _dialogueCanvas.enabled = isVisible;
    }

    public void StartSound(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource.clip = clip;
        AudioSource.Play();
    }

    public void EndSound()
    {
        AudioSource.Stop();
        AudioSource.clip = null;
    }

    public void UpdateArrows(DialogueNode[] branch, int index, bool stopDialogue)
    {
        if (index <= 0) BackNodeButton.interactable = false;
        else BackNodeButton.interactable = true;

        if (index >= branch.Length) NextNodeButton.interactable = false;
        else if (stopDialogue && index == branch.Length - 1) NextNodeButton.interactable = false;
        else NextNodeButton.interactable = true;
    }

    public void StartTypeWritterEffect(string text)
    {
        DialogueBox.text = string.Empty;
        _currentText = text;
        _typewritterCoroutine = StartCoroutine(TypewritterEffect(text));
    }

    public void EndTypeWritterEffect()
    {
        StopCoroutine(_typewritterCoroutine);
        _currentText = AddParagraphsToString(_currentText);
        DialogueBox.text = _currentText;
        IsTyping = false;
    }


    public IEnumerator TypewritterEffect(string text)
    {
        text = AddParagraphsToString(text);

        IsTyping = true;

        int index = 0;
        int tagLength;
        while (index < text.Length)
        {
            if (text[index] == '<')
            {
                tagLength = GetTagLength(index, text);
                index += tagLength;
            }

            DialogueBox.text = text.Substring(0, index);
            index++;

            yield return _typeWait;
        }
        DialogueBox.text = text.Substring(0, text.Length);
        IsTyping = false;
        yield return null;
    }

    int maximumPerLine = 58;
    private string AddParagraphsToString(string text)
    {
        int index = 0;
        int tagLen;
        int lineTagsLen = 0;
        int wordLen;
        int phraseLen;
        int paragraphs = 1;
        char[] textArray = text.ToCharArray();

        while (index < text.Length)
        {
            var current = textArray[index];

            if (current == '<')
            {
                tagLen = GetTagLength(index, text);
                lineTagsLen += tagLen;
            }
            else if (current == ' ')
            {
                wordLen = GetWordLength(index, text);
                phraseLen = index - lineTagsLen;

                if (phraseLen + wordLen > maximumPerLine * paragraphs)
                {
                    paragraphs++;
                    textArray[index] = '\n';
                }
            }

            index++;
        }

        return new string(textArray);
    }

    // Doesn't account for tags inside the word.
    private int GetWordLength(int index, string text)
    {
        index++;

        var length = 0;
        while (index < text.Length)
        {
            if (text[index] == '<') index += GetTagLength(index, text);
            if (index >= text.Length) return length;
            if (text[index] == ' ') return length;

            index++;
            length++;
        }

        return length;
    }

    private int GetTagLength(int index, string text)
    {
        var length = 1;
        do
        {
            index++;
            length++;
        }
        while (text[index] != '>');

        return length;
    }
}
