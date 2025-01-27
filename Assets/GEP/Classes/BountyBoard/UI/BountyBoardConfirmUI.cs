using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BountyBoardConfirmUI : MonoBehaviour
{
    // Public variables
    public Sprite CancelImage;
    public Sprite CancelHighlight;

    // Private variables
    private SpriteState _confirmState;
    private SpriteState _cancelState;

    // Component references
    private Image _image;
    private Button _button;

    private Sprite _confirmImage;
    private Sprite _confirmHighlight;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _confirmImage = _image.sprite;

        _button = GetComponent<Button>();
        _confirmHighlight = _button.spriteState.highlightedSprite;

        _confirmState.highlightedSprite = _confirmHighlight;
        _confirmState.pressedSprite = _confirmHighlight;
        _confirmState.selectedSprite = _confirmHighlight;
        _confirmState.disabledSprite = _confirmImage;

        _cancelState.highlightedSprite = CancelHighlight;
        _cancelState.pressedSprite = CancelHighlight;
        _cancelState.selectedSprite = CancelHighlight;
        _cancelState.disabledSprite = CancelImage;
    }

    public void SwitchToConfirm()
    {
        _image.sprite = _confirmImage;
        _button.spriteState = _confirmState;
    }

    public void SwitchToCancel()
    {
        _image.sprite = CancelImage;
        _button.spriteState = _cancelState;
    }
}
