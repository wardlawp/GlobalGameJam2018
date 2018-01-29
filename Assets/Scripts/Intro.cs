using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

    PrintingState state;

    MeshRenderer mesh;

    // Use this for initialization
    void Start () {
        state = new PrintingState(GetComponentInChildren<TextMesh>(), 15f);
        mesh = GetComponent<MeshRenderer>();
        GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (state != null)
        {
            if (!state.Run())
            {
                state = null;
                GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
                GetComponentInChildren<TextMesh>().text = "";
                mesh.enabled = false;
            }
        }

    }


    class PrintingState
    {
        float curPosF = 0;
        int curPosI = 0;
        string content = "";
        float lastTime;
        float rate;
        TextMesh target;

        public PrintingState(TextMesh target, float rate)
        {
            this.target = target;

            content = "Lonksis home router V0.0.1.Beta       \n" +
                "Starting...          \n" +
                "Incoming packets... begin routing.";

            lastTime = Time.time;
            this.rate = rate;
        }


        public bool Run()
        {

            if (curPosI == content.Length - 1) return (Time.time < (lastTime + 3f));

            curPosF += (Time.time - lastTime) * rate;

            if (curPosI < (int)curPosF)
            {
                curPosI = (int)curPosF;
                target.text = content.Substring(0, curPosI);
            }

            lastTime = Time.time;

            return true;
        }
    }
}
