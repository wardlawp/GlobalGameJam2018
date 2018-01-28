using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseEnd : MonoBehaviour {

    public Transform m_attachTo;
    public Transform m_attachmentPoint;
    public ConfigurableJoint m_connectionJoint;
    public float m_pullForce = 75f;
    public float m_orientationSpeed = 360f;

    public bool m_held = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
//         if (m_attachTo != null && m_attachmentPoint != null)
//         {
//             Rigidbody rb = GetComponent<Rigidbody>();
//             // Update velocity
//             Vector3 dir = (m_attachTo.position - m_attachmentPoint.position);
//             float distSq = dir.sqrMagnitude;
//             if (distSq > 0f)
//             {
//                 dir.Normalize();
//                 dir *= distSq * m_pullForce;
//                 rb.velocity = dir;
//             }
//             else
//             {
//                 rb.velocity = new Vector3(0f, 0f, 0f);
//             }
// 
//             float rotationPerTick = Mathf.Deg2Rad * m_orientationSpeed * Time.fixedDeltaTime;
//             Quaternion targetRotation = Quaternion.Euler(m_attachTo.forward);
//             targetRotation = Quaternion.LookRotation(transform.forward, Vector3.up) * targetRotation;
//             rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationPerTick);
//        }
    }
}
