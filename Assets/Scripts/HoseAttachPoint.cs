using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseAttachPoint : MonoBehaviour {

    public float m_attachTolerance = 0.5f;
    public HoseEnd m_attachedHose = null;
    public Port m_port;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, m_attachTolerance);

        foreach (Collider c in nearby)
        {
            HoseEnd hose = c.GetComponent<HoseEnd>();
            if (hose != null && !hose.m_held && m_attachedHose == null)
            {
                hose.m_connectionJoint.connectedAnchor = transform.position;
                if (hose != null)
                {
                    hose.m_connectionJoint.xMotion = ConfigurableJointMotion.Locked;
                    hose.m_connectionJoint.yMotion = ConfigurableJointMotion.Locked;
                    hose.m_connectionJoint.zMotion = ConfigurableJointMotion.Locked;

                    m_attachedHose = hose;

                    Rigidbody hoseRb = hose.GetComponent<Rigidbody>();
                    if (hoseRb != null)
                    {
                        hoseRb.freezeRotation = true;
                        hoseRb.angularVelocity = Vector3.zero;
                    }
                }

                break;
            }

            if (m_attachedHose)
            {
                Packet packet = c.GetComponent<Packet>();
                if (packet && packet.currentTansmission.source == m_port)
                {
                    m_attachedHose.Send(packet);
                }
            }
        }
    }
}
