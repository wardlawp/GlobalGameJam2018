using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packet : MonoBehaviour {
    private Color oddChannel = Color.yellow;
    private Color evenChannel = Color.blue;
    private Color initial;
    private bool scheduleOver = false;

    public Transmission currentTansmission { get; private set; }

    public List<Collider> hoseSegments;
    public float hoseSpeed = 10f;
    public float hoseTolerance = 0.1f;

    public void Init(Transmission transmission)
    {
        initial = GetComponent<MeshRenderer>().material.color;
        currentTansmission = transmission;

        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", ((transmission.id % 2 == 0) ? evenChannel : oddChannel)*0.5f);

    }
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if(!scheduleOver && (currentTansmission == null || currentTansmission.schedule.IsOver(Time.time)))
        {
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
            scheduleOver = true;
        }
	}

    void FixedUpdate()
    {
        if (hoseSegments.Count  > 0)
        {
            Vector3 transVec = (hoseSegments[0].transform.position - transform.position).normalized * (hoseSpeed * Time.fixedDeltaTime);
            transform.Translate(transVec);
            if ((transform.position - hoseSegments[0].transform.position).sqrMagnitude > (hoseTolerance * hoseTolerance))
            {
                hoseSegments.RemoveAt(0);
                if (hoseSegments.Count == 0)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
    }
}
