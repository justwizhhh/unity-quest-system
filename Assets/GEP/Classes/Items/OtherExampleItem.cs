using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherExampleItem : MonoBehaviour, IPickupable
{
    public void Pickup()
    {
        gameObject.SetActive(false);
    }
}
