using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleItem : MonoBehaviour, IPickupable
{
    public void Pickup()
    {
        gameObject.SetActive(false);
    }
}
