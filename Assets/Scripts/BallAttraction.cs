using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttraction : MonoBehaviour {

    public float rangeOnceAttached = 2.0f;
    public float forceConstant = 20.0f;
    List<GameObject> attractingObjects = new List<GameObject>();
    private Transmission currentTransmission = null;

    void OnTriggerEnter(Collider other)
    {
        if (GetComponentInParent<Port>().isSource) return;
        if (currentTransmission == null) return;


        GameObject obj = other.gameObject;

        if (obj.tag == "packet")
        {
            if (obj.GetComponent<Packet>().currentTansmission == currentTransmission)
            {
                if (!attractingObjects.Contains(obj))
                {
                    attractingObjects.Add(obj);
                }
            }
        }
    }

        // Update is called once per frame
    void Update () {
        if (GetComponentInParent<Port>().isSource) return;
        currentTransmission = GetComponentInParent<Port>().currentTransmission;

        if (currentTransmission == null) return;

        for (int i = 0; i < attractingObjects.Count;)
        {
            GameObject obj = attractingObjects[i];

            if (obj != null && obj.GetComponent<Packet>().currentTansmission == currentTransmission)
            {
                i++;
            }
            else
            {
                attractingObjects.RemoveAt(i);
            }
        }

        for (int i =0; i< attractingObjects.Count;)
        {
            GameObject obj = attractingObjects[i];

            Vector3 forceDirection = (transform.position - obj.transform.position);
            float distance = forceDirection.magnitude;

            if(distance < rangeOnceAttached)
            {
                distance /= 3;
                Vector3 force = forceDirection.normalized * (forceConstant / distance*distance);
                obj.GetComponent<Rigidbody>().AddForce(force);
                i++;
            }
            else
            {
                attractingObjects.RemoveAt(i);
            }
        }
	}
}
