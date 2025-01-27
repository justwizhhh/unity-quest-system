using UnityEngine;

[System.Serializable]

public class ConditionExplore
{
    // ------------------------------
    //
    // Bounty condition struct for going to a specific part of the level
    //
    // ------------------------------

    public Vector3 LocationPosition;
    public float Radius;
    public string LocationDescription;
}
