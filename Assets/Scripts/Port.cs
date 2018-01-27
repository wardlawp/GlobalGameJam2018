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

	void Start () {
        reservedUntil = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
