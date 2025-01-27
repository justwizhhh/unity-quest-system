using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BountyBoardUIButton : MonoBehaviour
{
    // ------------------------------
    //
    // A UI button used by the player to select a bounty
    //
    // ------------------------------

    // Public variables
    public BountyObject AssignedBounty;
    [HideInInspector] public BountyBoardUI _ui;

    [Space(10)]
    public Color CompletedColour;

    // Private variables
    private Color _originalColour;

    // Object references
    private TMP_Text _text;
    private Image _iconImage;
    private Image _bgImage;

    public void SetUp(BountyObject bounty, BountyBoardUI ui)
    {
        AssignedBounty = bounty;
        _ui = ui;

        // Find components for button graphics
        _text = GetComponentInChildren<TMP_Text>();
        _text.text = bounty.BountyInfo.Name;

        foreach (Image img in GetComponentsInChildren<Image>())
        {
            if (img.gameObject != gameObject)
            {
                _iconImage = img;
                break;
            }
        }
        _bgImage = GetComponent<Image>();

        // Tweak button graphics
        _iconImage.sprite = bounty.BountyInfo.Icon;
        _originalColour = _bgImage.color;
    }

    public void SetColour(bool isCompleted)
    {
        _bgImage.color = isCompleted ? CompletedColour : _originalColour;
    }

    public void Select()
    {
        _ui.SelectNewBounty(AssignedBounty);
    }
}
