using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packet : MonoBehaviour {
    private Color oddChannel = Color.red;
    private Color evenChannel = Color.blue;
    private Color initial;

    public Transmission currentTansmission { get; private set; }

    public void Init(Transmission transmission)
    {
        initial = GetComponent<MeshRenderer>().material.color;
        currentTansmission = transmission;

        if(transmission != null)
            GetComponent<MeshRenderer>().material.color = (transmission.id % 2 == 0) ? evenChannel: oddChannel;

    }
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if(currentTansmission == null || currentTansmission.schedule.IsOver(Time.time))
        {
            GetComponent<MeshRenderer>().material.color = initial;
        }
	}
}
