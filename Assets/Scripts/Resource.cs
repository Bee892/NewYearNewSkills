using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class Resource : MonoBehaviour
{
    public ResourceType Type;
    public float Quantity;
    public bool Persistent;

    public Resource(ResourceType type, float quantity, bool persistent)
    {
        Type = type;
        Quantity = quantity;
        Persistent = persistent;
    }
}
