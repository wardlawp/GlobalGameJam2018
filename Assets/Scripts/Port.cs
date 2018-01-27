using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour {

    public GameObject packetBlueprint;
    public Vector3 spawnOffset;
    public float reservedUntil { get; private set; }
    public int currentTransmissionId { get; private set; }
    // Use this for initialization

    private bool isSource;

    public void reserve(int transmissionId, float untilTime, bool isSource = false)
    {
        this.isSource = isSource;
        reservedUntil = untilTime;
        currentTransmissionId = transmissionId;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isSource) return;

        if (other.gameObject.tag == "packet")
        {
            
            Packet collidingPacket = other.gameObject.GetComponent<Packet>();
            int packetTransmissionId = collidingPacket.transmissionId;

            Debug.Log("Packet [trans:" + packetTransmissionId.ToString() +
                "] has collided with port [trans:" + currentTransmissionId.ToString() +"]");

            if (currentTransmissionId == currentTransmissionId)
            {
                Debug.Log("Port destroying packet");
                Destroy(other.gameObject);
            }
        }
    }


    public void emmit()
    {
        GameObject newPacketGObj = Instantiate(packetBlueprint);
        newPacketGObj.GetComponent<Packet>().Init(currentTransmissionId);
        newPacketGObj.transform.position = transform.position + spawnOffset;
    }

	void Start () {
        currentTransmissionId = -2;
        reservedUntil = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
