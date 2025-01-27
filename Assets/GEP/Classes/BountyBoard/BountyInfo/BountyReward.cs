using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyReward : MonoBehaviour
{
    // ------------------------------
    //
    // This object distributes all of the player's rewards once a bounty has been completed
    //
    // ------------------------------

    // Object references
    private BountyObject _bounty;
    private PlayerInventory _player;

    private void Start()
    {
        _bounty = GetComponentInParent<BountyObject>();
        _player = FindAnyObjectByType<PlayerInventory>();
    }

    public void GiveOutStatRewards()
    {
        foreach (RewardStat stat in _bounty.BountyInfo.StatRewards)
        {
            switch (stat.StatName)
            {
                case InventoryVar.Health:
                    _player.Health += stat.Count;
                    break;

                case InventoryVar.Mana:
                    _player.Mana += stat.Count;
                    break;

                case InventoryVar.XP:
                    _player.XP += stat.Count;
                    break;
            }
        }
    }

    public void GiveOutItemRewards()
    {
        foreach (RewardItem item in _bounty.BountyInfo.ItemRewards)
        {
            if (item.IsUnique)
            {
                if (_player.InventoryItems.ContainsKey(item.ItemName))
                {
                    break;
                }
            }
            _player.InventoryItems.Add(item.ItemName, item.Count);
        }
    }
}
