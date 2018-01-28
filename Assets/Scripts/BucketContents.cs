using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BucketContents : MonoBehaviour
{
    public List<GameObject> contents = new List<GameObject>();
    public List<GameObject> frozenContents = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider c)
    {
        var root = c.transform.GetComponent<PickupTarget>();
        if (root && !contents.Contains(c.gameObject))
        {
            if (c.gameObject.layer == LayerMask.NameToLayer("Packets"))
                contents.Add(c.gameObject);
        }
    }

    void OnTriggerExit(Collider c)
    {
        contents.Remove(c.gameObject);
    }

    public void ClearContents()
    {
        UnFreeze();
        contents.Clear();
        frozenContents.Clear();
    }

    public void Freeze()
    {
        foreach (var c in contents)
        {
            frozenContents.Add(c.gameObject);
            Rigidbody rb = c.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            c.layer = LayerMask.NameToLayer("Details");
            c.transform.parent = transform;
        }

        contents.Clear();
    }

    public void UnFreeze()
    {
        foreach (var c in frozenContents)
        {
            contents.Add(c.gameObject);
            Rigidbody rb = c.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            c.layer = LayerMask.NameToLayer("Packets");
            c.transform.parent = null;
        }

        frozenContents.Clear();
    }
}
