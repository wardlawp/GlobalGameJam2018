using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour {

    public GameObject packetBlueprint;
    public Vector3 spawnOffset;
    public float reservedUntil { get; private set; }
    public int currentTransmissionId { get; private set; }

    private bool isSource;
    private bool init = false;

    public void Reset()
    {
        init = false;
        reservedUntil = 0.0f;
        currentTransmissionId = -2;
    }

    public void reserve(int transmissionId, float untilTime, bool isSource = false)
    {
        init = true;
        this.isSource = isSource;
        reservedUntil = untilTime;
        currentTransmissionId = transmissionId;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isSource || !init) return;

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


    public void emmit(bool overide = false)
    {
        if(!overide && !init)
        {
            throw new System.Exception("Port::emmit() being called before initialization");
        }

        GameObject newPacketGObj = Instantiate(packetBlueprint);
        newPacketGObj.GetComponent<Packet>().Init(currentTransmissionId);
        //todo remove this hack?
        Vector3 randomNudge = new Vector3(Random.Range(-.2f, +.2f), Random.Range(-.2f, +.2f), Random.Range(-.2f, +.2f));
        newPacketGObj.transform.position = transform.position + spawnOffset + randomNudge;
    }

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
