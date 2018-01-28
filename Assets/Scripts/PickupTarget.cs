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

    public virtual void OnPickup()
    {
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    public virtual void OnThrow()
    {
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.freezeRotation = false;
    }

    public virtual void OnDrop()
    {
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.freezeRotation = false;
    }
}
