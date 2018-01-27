using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Camera m_viewCamera;

    bool m_carrying;
    public Transform m_attachedObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit raycastHit;
        if (!m_carrying && Input.GetButtonDown("Interact"))
        {
            if (Physics.Raycast(m_viewCamera.transform.position, m_viewCamera.transform.forward, out raycastHit, 3f))
            {
                if (!raycastHit.Equals(gameObject))
                {
                    m_carrying = true;
                    m_attachedObject = raycastHit.transform;
                    m_attachedObject.transform.position = m_viewCamera.transform.position + m_viewCamera.transform.forward;
                    m_attachedObject.transform.parent = m_viewCamera.transform;
                    Rigidbody rb = m_attachedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }
        }
        else if (m_carrying && Input.GetButtonDown("Interact"))
        {
            m_attachedObject.transform.parent = null;
            Rigidbody rb = m_attachedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            m_attachedObject = null;
            m_carrying = false;
        }
    }   

    void FixedUpdate()
    {
        
    }
}
