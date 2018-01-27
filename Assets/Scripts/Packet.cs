using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packet : MonoBehaviour {

    public int transmissionId { get; private set; }

    public void Init(int id)
    {
        transmissionId = id;
    }
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
