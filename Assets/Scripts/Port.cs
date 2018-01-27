using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour {
    public float reservedUntil { get; private set; }
    public int currentTransmissionId { get; private set; }
    // Use this for initialization

    public void reserve(int transmissionId, float untilTime)
    {
        reservedUntil = untilTime;
        currentTransmissionId = transmissionId;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "packet")
        {
            Debug.Log("Packet has entered port trigger");
            Packet collidingPacket = other.gameObject.GetComponent<Packet>();

            if (CanAcceptPacket(collidingPacket.transmissionId))
            {
                Debug.Log("Port destroying packet");
                Destroy(other.gameObject);
            }
        }

    }


    public void emmit()
    {
        //todo spawn packet with trasnmissionId
    }

    private bool CanAcceptPacket(int transmissionId)
    {
        return (transmissionId == currentTransmissionId);
    }

	void Start () {
        currentTransmissionId = -2;
        reservedUntil = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
