using System;
using System.Collections.Generic;
using UnityEngine;

public class BountyCondition : MonoBehaviour
{
    // ------------------------------
    //
    // This object tracks all of the required condition tasks the player has to complete
    //
    // ------------------------------

    // Private variables
    // This stores the names of the object type, and how many the player has collected of each type
    [HideInInspector] public Dictionary<string, int> PickUpProgress = new Dictionary<string, int>();
    [HideInInspector] public List<bool> ExploreProgress = new List<bool>();

    // Object references
    private BountyObject _bounty;

    private void Start()
    {
        _bounty = GetComponentInParent<BountyObject>();

        GenerateConditions();
    }

    private void GenerateConditions()
    {
        foreach (ConditionCollect targetPickUp in _bounty.BountyInfo.CollectConditions)
        {
            PickUpProgress.Add(targetPickUp.ObjectName.ToString(), 0);
        }

        foreach (ConditionExplore explorePos in _bounty.BountyInfo.ExploreConditions)
        {
            ExploreProgress.Add(false);
        }
    }

    private int TrackCollectConditions()
    {
        int progress = 0;

        if (PickUpProgress != null)
        {
            // Track progress of pick-up bounty conditions
            foreach (ConditionCollect targetPickUp in _bounty.BountyInfo.CollectConditions)
            {
                foreach (string pickUp in PickUpProgress.Keys)
                {
                    int pickUpCount;

                    if (PickUpProgress.TryGetValue(pickUp, out pickUpCount))
                    {
                        if (pickUpCount >= targetPickUp.Count)
                        {
                            progress++;
                        }
                    }
                }
            }
        }

        return progress;
    }

    private int TrackExploreConditions()
    {
        int progress = 0;

        if (ExploreProgress != null)
        {
            // Track how far away the player is from each location position
            foreach (bool explored in ExploreProgress)
            {
                if (explored)
                {
                    progress++;
                }
            }
        }

        return progress;
    }

    public void ClearProgress()
    {
        PickUpProgress.Clear();
        ExploreProgress.Clear();

        GenerateConditions();
    }

    public void TrackConditions()
    {
        if (!_bounty.IsCompleted)
        {
            TrackCollectConditions();

            // If all conditions have been met, mark bounty as complete
            int conditionsCount = 
                _bounty.BountyInfo.CollectConditions.Count +
                _bounty.BountyInfo.ExploreConditions.Count;

            _bounty.CurrentProgress = TrackCollectConditions() + TrackExploreConditions();
            if (_bounty.CurrentProgress == conditionsCount)
            {
                _bounty.IsCompleted = true;
            }
        }
    }
}
