using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : PickupTarget
{
    public BucketContents contents;

    public override void OnPickup()
    {
        contents.Freeze();
        base.OnPickup();
    }

    public override void OnThrow()
    {
        contents.UnFreeze();
        contents.ClearContents();
        base.OnThrow();
    }

    public override void OnDrop()
    {
        contents.UnFreeze();
        base.OnDrop();
    }
}
