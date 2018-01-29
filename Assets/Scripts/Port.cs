using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour {

    public GameObject packetBlueprint;
    public Transform spawnPoint;
    public float reservedUntil { get; private set; }
    public Transmission currentTransmission { get; private set; }
    public float spawnForce = 5f;

    public MeshRenderer l1, l2, l3;
    public Light ll1, ll2, ll3;
    private Color color1 = Color.white, color2 = Color.white, color3 = Color.white;
    public Color lightColor1, lightColor2, glassColor1, glassColor2, glassColorOff;
    float blink = 0.0f;
    bool blinkOn = false;

    public bool isSource;
    private bool init = false;
    private float initTime = -1.0f;

    public void Reset(bool force = false)
    {
        if (!force && initTime == Time.time) return; //something else has reserved this tick, don't reset

        init = false;
        reservedUntil = 0.0f;
        currentTransmission = null;

        unsetColors();
    }

    public void reserve(Transmission transmission, float untilTime, bool isSource = false)
    {
        init = true;
        this.isSource = isSource;
        reservedUntil = untilTime;
        currentTransmission = transmission;

        initTime = Time.time;
        //setColors();

    }

    void unsetColors()
    {
        color1 = glassColorOff;
        color2 = glassColorOff;
        color3 = glassColorOff;
        ll1.color = Color.white;
        ll2.color = Color.white;
        ll3.color = Color.white;
        ll1.gameObject.SetActive(false);
        ll2.gameObject.SetActive(false);
        ll3.gameObject.SetActive(false);
    }

    void setColors()
    {
        Color color = (currentTransmission.id % 2 == 1) ? glassColor1 : glassColor2;
        Color lightColor = (currentTransmission.id % 2 == 1) ? lightColor1 : lightColor2;

        color1 = color;
        ll1.color = lightColor;
        ll1.gameObject.SetActive(true);

        if (currentTransmission.schedule.type > FlowName.Light)
        {
            color2 = color;
            ll2.color = lightColor;
            ll2.gameObject.SetActive(true);
        }

        if (currentTransmission.schedule.type >= FlowName.Medium)
        {
            color3 = color;
            ll3.color = lightColor;
            ll3.gameObject.SetActive(true);
        }


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
		if(currentTransmission != null && Time.time > (blink + .8f))
        {
            if (isSource) setColors(); 
            else
            {
                if (blinkOn)
                {
                    setColors();
                    GetComponent<AudioSource>().Play();
                }
                else unsetColors();

                blinkOn = !blinkOn;
                blink = Time.time;
            }

            

        } else if (currentTransmission == null) { unsetColors();  }

        //keegan change here
        l1.material.SetColor("_EmissionColor", color1);
	    l2.material.SetColor("_EmissionColor", color2);
	    l3.material.SetColor("_EmissionColor", color3);
    }
}
