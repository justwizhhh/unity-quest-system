using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum InventoryVar
{
    Health,
    Mana,
    XP
};

public class PlayerInventory : MonoBehaviour
{
    // ------------------------------
    //
    // This class tracks all of the items the player has, as well as the player's main stats
    //
    // ------------------------------

    public int Health;
    public int Mana;
    public int XP;

    public Dictionary<string, int> InventoryItems = new Dictionary<string, int>();
}
