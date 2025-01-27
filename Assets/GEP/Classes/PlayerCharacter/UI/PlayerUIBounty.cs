using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIBounty : MonoBehaviour
{
    // ------------------------------
    //
    // Provides the player with subtle visuals on the progress on their current bounty
    //
    // ------------------------------

    // Public variables
    public BountyObject TrackedBounty;

    [Space(10)]
    public Color CompletionColour;

    // Private variables
    private Color defaultColor;

    // Object references
    private TMP_Text _text;
    private Image _panel;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _panel = GetComponentInChildren<Image>();
        defaultColor = _panel.color;
    }

    public void SetTrackingBounty(BountyObject newBounty)
    {
        TrackedBounty = newBounty;
        _text.text = newBounty.BountyInfo.Name;

        _panel.color = defaultColor;
    }

    private void Update()
    {
        if (TrackedBounty != null)
        {
            if (TrackedBounty.IsCompleted)
            {
                _panel.color = CompletionColour;
            }
            else
            {
                _panel.color = defaultColor;
            }
        }
    }
}
