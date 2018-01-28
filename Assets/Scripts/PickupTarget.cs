using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTarget : MonoBehaviour
{
    public Vector3 rotationOffset;
    public Vector3 InitialScale { get; private set; }

    void Awake()
    {
        InitialScale = transform.localScale;
    }

    public virtual void OnPickup() { }
    public virtual void OnDrop() { }
}
