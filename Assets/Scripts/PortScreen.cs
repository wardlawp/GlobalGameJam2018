using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortScreen : MonoBehaviour
{
    public Port port;
    private TextMesh mesh;
    // [char/second]
    public float cursorRate;
    public int columnLenght = 20;
    private PresentationState state;
    

    void Start ()
	{
        state = null;
        mesh = GetComponentInChildren<TextMesh>();

        if(mesh == null)
        {
            throw new System.Exception("Shit son!");
        }
	}
	
	void Update ()
	{
        if(state != null)
        {
            if(!state.Run(mesh))
            {
                state = null;
            }
        }
        else if(port.currentTransmission != null)
        {
            state = new PresentationState(port.currentTransmission, columnLenght, cursorRate);
        }
		
	}

    class PresentationState
    {
        float curPosF = 0;
        int curPosI = 0;
        Transmission transmission;
        string content;
        float lastTime;
        float rate;

        public PresentationState(Transmission trans, int columnLenght, float rate)
        {
            content = "Incoming transmission... \n" +
                "Client:" + transmission.clientName +
                "\n Content:" + transmission.contentName;

            for(int i = 1; i < content.Length; i++)
            {
                if(i%columnLenght == 0)
                {
                    content.Insert(i, "\n");
                }
            }

            lastTime = Time.time;
            this.rate = rate;
        }

        public bool Run(TextMesh target)
        {
            // transmission over
            if (transmission == null) return false;

            // nothing to do
            if (curPosI == content.Length - 1) return true;

            curPosF += (Time.time - lastTime) * rate;

            if(curPosI < (int)curPosF)
            {
                curPosI = (int)curPosF;
                target.text = content.Substring(0, curPosI);
            }

            lastTime = Time.time;

            return true;

        }
    }
}
