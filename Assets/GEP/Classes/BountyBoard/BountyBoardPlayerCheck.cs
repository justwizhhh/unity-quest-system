using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyBoardPlayerCheck : MonoBehaviour, IInteractible
{
    // ------------------------------
    //
    // The collision checker that will allow for the player to interact with the bounty board if they are up close enough to it
    //
    // ------------------------------

    private BountyBoard _bountyBoard;
    
    private void Start()
    {
        _bountyBoard = GetComponentInParent<BountyBoard>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ThirdPersonController player))
        {
            if (!_bountyBoard._isPlayerNearby)
            {
                _bountyBoard._isPlayerNearby = true;

                // Don't show the selection UI at the start of the game, we only enable it once a player is nearby
                if (!_bountyBoard._ui._selectionUI.activeSelf)
                {
                    _bountyBoard._ui._selectionUI.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ThirdPersonController player))
        {
            if (_bountyBoard._isPlayerNearby)
            {
                _bountyBoard._isPlayerNearby = false;
            }
        }
    }

    void IInteractible.OnPlayerInteract()
    {
        if (_bountyBoard._isPlayerNearby)
        {
            _bountyBoard.PlayerTrigger();
        }
    }
}
