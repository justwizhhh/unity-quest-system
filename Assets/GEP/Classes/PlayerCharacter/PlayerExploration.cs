using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExploration : MonoBehaviour
{
    // ------------------------------
    //
    // This class tracks the distances between important game-locations and the game
    //
    // ------------------------------


    // Update is called once per frame
    void Update()
    {
        BountyObject[] bounties = GetComponentsInChildren<BountyObject>();

        if (bounties.Length > 0)
        {
            foreach (BountyObject bounty in bounties)
            {
                for (int i = 0; i < bounty.BountyInfo.ExploreConditions.Count; i++)
                {
                    ConditionExplore condition = bounty.BountyInfo.ExploreConditions[i];
                    if (Vector3.Distance(transform.position, condition.LocationPosition) <= condition.Radius)
                    {
                        bounty.GetComponent<BountyCondition>().ExploreProgress[i] = true;
                    }
                }
            }
        }
    }
}
