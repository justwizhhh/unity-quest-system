using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // ------------------------------
    //
    // This class constantly updates the UI based on the player's current information
    //
    // ------------------------------

    // Public variables
    [Header("UI Object References")]
    public GameObject BountyPrefab;

    [Space(10)]
    public GameObject BountyListContainer;

    // Private variables
    private PlayerInteraction _player;

    private void Awake()
    {
        _player = FindFirstObjectByType<PlayerInteraction>();

        for (int i = 0; i < _player.MaxBountyCount; i++)
        {
            GameObject newBounty = Instantiate(BountyPrefab);
            newBounty.transform.SetParent(BountyListContainer.transform, false);
            newBounty.SetActive(false);
        }
    }

    public void UpdateBountyList(bool removeComplete)
    {
        List<BountyObject> currentBounties = _player.GetComponentsInChildren<BountyObject>().ToList();

        if (currentBounties.Count == 0)
        {
            foreach (PlayerUIBounty bountyUI in BountyListContainer.GetComponentsInChildren<PlayerUIBounty>())
            {
                bountyUI.gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _player.MaxBountyCount; i++)
            {
                PlayerUIBounty bountyUI = BountyListContainer.transform.GetChild(i).GetComponent<PlayerUIBounty>();

                if (i < currentBounties.Count)
                {
                    bountyUI.SetTrackingBounty(currentBounties[i]);

                    if (removeComplete)
                    {
                        if (bountyUI.TrackedBounty.IsCompleted
                            && !bountyUI.TrackedBounty.IsInProgress)
                        {
                            bountyUI.gameObject.SetActive(false);
                        }
                        else
                        {
                            bountyUI.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        bountyUI.gameObject.SetActive(true);
                    }
                }
                else
                {
                    bountyUI.gameObject.SetActive(false);
                }
            }
        }
    }
}
