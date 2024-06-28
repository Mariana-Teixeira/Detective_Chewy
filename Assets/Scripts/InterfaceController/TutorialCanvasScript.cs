using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;
    public static Action ClickToNext;
    private Canvas _tutorialCanvas;
    public Sprite[] TutorialImages;
    private int ImageIndex = 0;

    private void Start()
    {
        _tutorialCanvas = GetComponent<Canvas>();

        ToggleVisibility += OnToggleVisibility;
        ClickToNext += OnClickToNext;

        UpdateImage();
    }

    public void OnToggleVisibility(bool isVisible)
    {
        _tutorialCanvas.enabled = isVisible;
    }

    public void OnClickToNext()
    {
        ImageIndex++;

        if(ImageIndex < TutorialImages.Length)
        {
            UpdateImage();
        }
        else
        {
            PlayerStates.ChangeState?.Invoke(GameState.PLAYING);
        }
    }

    public void UpdateImage()
    {
        var child = this.transform.GetChild(1).GetComponent<Image>();
        child.sprite = TutorialImages[ImageIndex];
    }
}
