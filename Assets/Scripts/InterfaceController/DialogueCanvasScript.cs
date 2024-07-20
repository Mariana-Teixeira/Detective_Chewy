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
        if (branch == null) return;

        if (index <= 0) BackNodeButton.interactable = false;
        else BackNodeButton.interactable = true;

        if (index >= branch.Length ||
            index == (branch.Length - 1) && stopDialogue) NextNodeButton.interactable = false;
        else NextNodeButton.interactable = true;
    }

    public void StartTypeWritterEffect(string text)
    {
        DialogueBox.text = string.Empty;
        _currentText = text;
        _typewritterCoroutine = StartCoroutine(AlphaTypewritter(text));
    }

    public void EndTypeWritterEffect()
    {
        StopCoroutine(_typewritterCoroutine);
        DialogueBox.text = _currentText;
        IsTyping = false;
    }

    const string ALPHATAG = "<mark=#000000>";
    public IEnumerator AlphaTypewritter(string text)
    {
        IsTyping = true;

        int index = 0;
        int tagLength = 0;

        while (index < text.Length)
        {
            if (text[index] == '<')
            {
                tagLength = GetTagLength(index, text);
                index += tagLength;
            }

            DialogueBox.text = text.Substring(0, index) + ALPHATAG + text.Substring(index) + "</mark>";
            index++;
            yield return _typeWait;
        }
        DialogueBox.text = text;
        IsTyping = false;
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
