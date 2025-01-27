using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // ------------------------------
    //
    // This class tracks all player interactions with other objects
    // This will include: pick-ups, interactions, enemy kills, quests/bounties etc.
    //
    // ------------------------------

    [Header("Bounty Settings")]
    public int MaxBountyCount;

    public IInteractible CurrentInteractObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractible interact))
        {
            CurrentInteractObj = interact;
        }
        else
        {
            CurrentInteractObj = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IPickupable pickup))
        {
            GameObject pickUpObj = collision.gameObject;
            string pickUpType = pickUpObj.GetComponent<MonoBehaviour>().GetType().ToString();

            pickup.Pickup();

            // Check if the object the player just picked up is being tracked by any active bounty
            BountyCondition[] bountyConditions = GetComponentsInChildren<BountyCondition>();

            if (bountyConditions.Length > 0)
            {
                foreach (BountyCondition condition in bountyConditions)
                {
                    int value;
                    if (condition.PickUpProgress.TryGetValue(pickUpType, out value))
                    {
                        condition.PickUpProgress[pickUpType.ToString()]++;
                    }
                }
            }
        }
    }
}
