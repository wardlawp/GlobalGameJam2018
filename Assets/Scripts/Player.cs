﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Camera m_viewCamera;

    //bool m_carrying;
    public Transform m_attachedObject;
    int m_attachedObjectOriginalLayer;
    public Transform m_attachmentPoint;
    public float m_pullForce = 20f;
    public float m_throwForce = 75f;
    public float m_orientationSpeed = 360f;
    public float m_pickupRange = 5f;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        RaycastHit raycastHit;
        if (m_attachedObject == null && (Input.GetButtonDown("Interact") || Input.GetButtonDown("Throw")))
        {
            if (Physics.Raycast(m_viewCamera.transform.position, m_viewCamera.transform.forward, out raycastHit, m_pickupRange))
            {
                if (raycastHit.transform != transform)
                {
                    //m_carrying = true;
                    var pickupTarget = raycastHit.transform.GetComponentInParent(typeof(PickupTarget)) as PickupTarget;
                    if (pickupTarget)
                    {
                        m_attachedObject = pickupTarget.transform;
                        //m_attachedObject.transform.position = m_viewCamera.transform.position + m_viewCamera.transform.forward;
                        //m_attachedObject.transform.parent = m_viewCamera.transform;
                        Rigidbody rb = m_attachedObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            pickupTarget.OnPickup();
                            m_attachedObjectOriginalLayer = m_attachedObject.gameObject.layer;
                            m_attachedObject.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
                            rb.useGravity = false;
                            rb.freezeRotation = true;
                        }
                        HoseEnd hoseEnd = m_attachedObject.GetComponent<HoseEnd>();
                        if (hoseEnd != null)
                        {
                            hoseEnd.m_held = true;
                            hoseEnd.m_connectionJoint.xMotion = ConfigurableJointMotion.Free;
                            hoseEnd.m_connectionJoint.yMotion = ConfigurableJointMotion.Free;
                            hoseEnd.m_connectionJoint.zMotion = ConfigurableJointMotion.Free;
                        }
                        //                         Rigidbody hoseRb = hoseEnd.GetComponent<Rigidbody>();
                        //                         if (hoseRb != null)
                        //                         {
                        //                             hoseRb.useGravity = true;
                        //                             hoseRb.freezeRotation = false;
                        //                         }
                        //                         if  (hoseEnd  != null)
                        //                         {
                        //                             hoseEnd.m_held = true;
                        //                             if (hoseEnd.m_attachTo != null)
                        //                             {
                        //                                 HoseAttachPoint hoseAttach = hoseEnd.m_attachTo.GetComponent<HoseAttachPoint>();
                        //                                 if (hoseAttach != null)
                        //                                 {
                        //                                     hoseAttach.m_attachedHose = null;
                        //                                 }
                        // 
                        //                                 hoseEnd.m_attachTo = null;
                        //                             }
                        //                         }
                    }
                   
                }
            }
        }
        else if (m_attachedObject != null && Input.GetButtonDown("Interact"))
        {
            //m_attachedObject.transform.parent = null;
            m_attachedObject.gameObject.layer = m_attachedObjectOriginalLayer;
            m_attachedObjectOriginalLayer = -1;
            Rigidbody rb = m_attachedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.freezeRotation = false;
                var pickupTarget = m_attachedObject.GetComponent<PickupTarget>();
                pickupTarget.OnDrop();
            }
            HoseEnd hoseEnd = m_attachedObject.GetComponent<HoseEnd>();
            if (hoseEnd != null)
            {
                hoseEnd.m_held = false;
            }
            m_attachedObject = null;
            //m_carrying = false;
        }

        if (m_attachedObject != null && Input.GetButtonDown("Throw"))
        {
            //m_attachedObject.transform.parent = null;
            m_attachedObject.gameObject.layer = m_attachedObjectOriginalLayer;
            m_attachedObjectOriginalLayer = -1;
            Rigidbody rb = m_attachedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.freezeRotation = false;
                var pickupTarget = m_attachedObject.GetComponent<PickupTarget>();
                pickupTarget.OnDrop();
            }
            HoseEnd hoseEnd = m_attachedObject.GetComponent<HoseEnd>();
            if (hoseEnd != null)
            {
                hoseEnd.m_held = false;
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
                Bounds bounds = new Bounds(m_attachedObject.position, Vector3.zero);
                foreach (var col in m_attachedObject.GetComponentsInChildren<Collider>())
                {
                    bounds.Encapsulate(col.bounds);
                }

                Vector3 dir = (m_attachmentPoint.position - bounds.center);
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
