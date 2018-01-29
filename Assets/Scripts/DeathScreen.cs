using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{

    PrintingState state;
    public bool is_enabled = false;
    MeshRenderer mesh;

    // Use this for initialization
    void Start()
    {
        state = new PrintingState(GetComponentInChildren<TextMesh>(), 15f);
        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_enabled) return;
        mesh.enabled = true;
        GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        if (state != null)
        {
            if (!state.Run())
            {
                state = null;
                Application.Quit();
            }
        }

    }


    class PrintingState
    {
        float curPosF = 0;
        int curPosI = 0;
        string content = "";
        float lastTime = float.MaxValue;
        float rate;
        TextMesh target;

        public PrintingState(TextMesh target, float rate)
        {
            this.target = target;

            content = "Error          \nPacket overflow!";

            this.rate = rate;
        }


        public bool Run()
        {
            if (lastTime == float.MaxValue) lastTime = Time.time;

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
