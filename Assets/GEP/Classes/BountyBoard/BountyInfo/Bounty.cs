using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBounty", menuName = "Bounty")]

public class Bounty : ScriptableObject
{
    // ------------------------------
    //
    // The scriptable object template for all bounties/"quests" in the game
    //
    // ------------------------------

    // Public variables
    [Header("General Info")]
    [Tooltip("The name/title of the bounty.")]
    public string Name;
    [Tooltip("The graphic that will be displayed on the bounty board itself.")]
    public Sprite Icon;
    [TextArea]
    [Tooltip("The description of the bounty. This can describe the required objectives and/or provide flavour text.")]
    public string Description;

    [Space(10)]
    [Header("Bounty Conditions")]
    [Tooltip("These conditions are for objects/items the player has to pick up and collect in the level.")]
    public List<ConditionCollect> CollectConditions = new List<ConditionCollect>();
    [Tooltip("These conditions are for parts of the level that the player need to get to and explore.")]
    public List<ConditionExplore> ExploreConditions = new List<ConditionExplore>();

    [Space(10)]
    [Header("Bounty Rewards")]
    [Tooltip("These rewards increase a specific stat on the player.")]
    public List<RewardStat> StatRewards = new List<RewardStat>();
    [Tooltip("These rewards give the player a new item and puts it in their inventory.")]
    public List<RewardItem> ItemRewards = new List<RewardItem>();
}
