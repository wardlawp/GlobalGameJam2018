using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour {

    public GameObject packetBlueprint;
    public Transform spawnPoint;
    public float reservedUntil { get; private set; }
    public Transmission currentTransmission { get; private set; }
    public float spawnForce = 5f;

    public bool isSource;
    private bool init = false;
    private float initTime = -1.0f;

    public void Reset(bool force = false)
    {
        
        if (!force && initTime == Time.time) return; //something else has reserved this tick, don't reset

        init = false;
        reservedUntil = 0.0f;
        currentTransmission = null;
    }

    public void reserve(Transmission transmission, float untilTime, bool isSource = false)
    {
        init = true;
        this.isSource = isSource;
        reservedUntil = untilTime;
        currentTransmission = transmission;

        initTime = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isSource || !init) return;

        if (other.gameObject.tag == "packet")
        {
            
            Packet collidingPacket = other.gameObject.GetComponent<Packet>();
            int packetTransmissionId = collidingPacket.currentTansmission.id;

            Debug.Log("Packet [trans:" + packetTransmissionId.ToString() +
                "] has collided with port [trans:" + currentTransmission.id.ToString() +"]");

            if (currentTransmission == collidingPacket.currentTansmission)
            {
                Debug.Log("Port destroying packet");
                Destroy(other.gameObject);
            }
        }
    }


    public void emit(bool overide = false)
    {
        if(!overide && !init)
        {
            throw new System.Exception("Port::emit() being called before initialization");
        }

        GameObject newPacketGObj = Instantiate(packetBlueprint);
        newPacketGObj.GetComponent<Packet>().Init(currentTransmission);
        //todo remove this hack?
        Vector3 randomNudge = new Vector3(Random.Range(-.1f, +.1f), Random.Range(-.1f, +.1f), Random.Range(-.1f, +.1f));
        newPacketGObj.transform.position = spawnPoint.position + randomNudge;
        Rigidbody packetRb = newPacketGObj.GetComponent<Rigidbody>();
        if (packetRb != null)
        {
            packetRb.AddForce(transform.forward * spawnForce);
        }
    }

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
