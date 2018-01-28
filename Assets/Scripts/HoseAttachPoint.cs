using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseAttachPoint : MonoBehaviour {

    public float m_attachTolerance = 0.5f;
    public Transform m_attachedHose = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (m_attachedHose != null)
        {
            return;
        }

        Collider[] nearby = Physics.OverlapSphere(transform.position, m_attachTolerance);
        foreach(Collider c in nearby)
        {
            HoseEnd hose = c.GetComponent<HoseEnd>();
            if (hose != null && !hose.m_held)
            {
                hose.m_connectionJoint.connectedAnchor = transform.position;
                if (hose != null)
                {
                    hose.m_connectionJoint.xMotion = ConfigurableJointMotion.Locked;
                    hose.m_connectionJoint.yMotion = ConfigurableJointMotion.Locked;
                    hose.m_connectionJoint.zMotion = ConfigurableJointMotion.Locked;
                }
                //                 Rigidbody hoseRb = hose.GetComponent<Rigidbody>();
                //                 if (hoseRb != null)
                //                 {
                //                     hoseRb.useGravity = false;
                //                     hoseRb.freezeRotation = true;
                //                 }
                //                 hose.m_attachTo = transform;
                //                 m_attachedHose = hose.transform;
                break;
            }
        }
    }
}
