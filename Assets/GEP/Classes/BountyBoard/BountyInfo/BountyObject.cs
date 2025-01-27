using UnityEngine;

[RequireComponent(typeof(BountyCondition))]
[RequireComponent(typeof(BountyReward))]
public class BountyObject : MonoBehaviour
{
    // ------------------------------
    //
    // The bounty "quest"
    // This object tracks the current state of the quest, as well as store the required conditions and rewards
    //
    // ------------------------------

    // Public variables
    public Bounty BountyInfo;

    [Space(10)]
    [Tooltip("Has the player accepted this bounty?")]
    public bool IsInProgress;
    [Tooltip("This tracks how far along the player is on this current bounty (how many of the conditions the player has completed in total")]
    public int CurrentProgress;

    [Space(10)]
    public bool IsCompleted;

    // Private variables
    [HideInInspector] public int _maxProgress;
    [HideInInspector] public BountyBoardUIButton _button;

    // Component references
    private BountyCondition _conditions;
    private BountyReward _rewards;

    private void Awake()
    {
        _conditions = GetComponent<BountyCondition>();
        _rewards = GetComponent<BountyReward>();
    }

    private void Start()
    {
        _maxProgress =
                BountyInfo.CollectConditions.Count +
                BountyInfo.ExploreConditions.Count;

        // Makes sure that this object actually has enough info to run the bounty
        if (BountyInfo == null)
        {
            Debug.LogError("No bounty info has been assigned to " + gameObject.name + "!");
        }
        else
        {
            if (BountyInfo.CollectConditions.Count == 0 
                && BountyInfo.ExploreConditions.Count == 0)
            {
                Debug.LogError("No conditions have been assigned to " + gameObject.name + "!");
            }

            if (BountyInfo.StatRewards.Count == 0 && BountyInfo.ItemRewards.Count == 0)
            {
                Debug.LogError("No rewards have been assigned to " + gameObject.name + "!");
            }
        }
    }

    public void ActivateBounty()
    {
        IsInProgress = true;
    }

    public void DeactivateBounty()
    {
        _conditions.ClearProgress();
        IsInProgress = false;
        IsCompleted = false;
    }

    public void DistributeRewards()
    {
        _rewards.GiveOutStatRewards();
        _rewards.GiveOutItemRewards();
        IsInProgress = false;
    }

    private void Update()
    {
        if (IsInProgress)
        {
            _conditions.TrackConditions();
        }
    }

    public void EditorCompleteBounty()
    {
        IsCompleted = true;
    }

    public void EditorEraseBountyProgress()
    {
        _conditions.ClearProgress();
        CurrentProgress = 0;
        IsCompleted = false;
    }
}
