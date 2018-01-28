using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Camera m_viewCamera;

    //bool m_carrying;
    public Transform m_attachedObject;
    public Transform m_attachmentPoint;
    public float m_pullForce = 20f;
    public float m_throwForce = 75f;
    public float m_orientationSpeed = 360f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit raycastHit;
        if (m_attachedObject == null && Input.GetButtonDown("Interact"))
        {
            if (Physics.Raycast(m_viewCamera.transform.position, m_viewCamera.transform.forward, out raycastHit, 3f))
            {
                if (raycastHit.transform != transform)
                {
                    //m_carrying = true;
                    var hitTransform = raycastHit.transform.GetComponentInParent(typeof(PickupTarget));
                    if (hitTransform)
                    {
                        m_attachedObject = hitTransform.transform;
                        //m_attachedObject.transform.position = m_viewCamera.transform.position + m_viewCamera.transform.forward;
                        //m_attachedObject.transform.parent = m_viewCamera.transform;
                        Rigidbody rb = m_attachedObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.useGravity = false;
                            rb.freezeRotation = true;
                        }
                    }
                   
                }
            }
        }
        else if (m_attachedObject != null && Input.GetButtonDown("Interact"))
        {
            m_attachedObject.transform.parent = null;
            Rigidbody rb = m_attachedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.freezeRotation = false;
            }
            m_attachedObject = null;
            //m_carrying = false;
        }

        if (m_attachedObject != null && Input.GetButtonDown("Throw"))
        {
            m_attachedObject.transform.parent = null;
            Rigidbody rb = m_attachedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.freezeRotation = false;
            }
            m_attachedObject = null;
            rb.AddForceAtPosition(m_viewCamera.transform.forward * m_throwForce, m_attachmentPoint.position);
        }
    }   

    void FixedUpdate()
    {
        if (m_attachedObject != null)
        {
            Rigidbody rb = m_attachedObject.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                // Update velocity
                Vector3 dir = (m_attachmentPoint.position - m_attachedObject.position);
                float distSq = dir.sqrMagnitude;
                if (distSq > 0f)
                {
                    dir.Normalize();
                    dir *= distSq * m_pullForce;
                    rb.velocity = dir;
                }
                else
                {
                    rb.velocity = new Vector3(0f, 0f, 0f);
                }

                var pickupTarget = m_attachedObject.GetComponent<PickupTarget>();
                float rotationPerTick = Mathf.Deg2Rad * m_orientationSpeed * Time.fixedDeltaTime;
                Quaternion targetRotation = Quaternion.Euler(pickupTarget.rotationOffset);
                targetRotation = Quaternion.LookRotation(transform.forward, Vector3.up) * targetRotation;
                rb.rotation = Quaternion.Slerp(m_attachedObject.rotation, targetRotation, rotationPerTick);
            }
        }
    }
}
