using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packet : MonoBehaviour {

    public Transmission currentTansmission { get; private set; }

    public void Init(Transmission transmission)
    {
        currentTansmission = transmission;
    }
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
