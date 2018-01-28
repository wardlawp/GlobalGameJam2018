using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortScreen : MonoBehaviour
{
    public Port port;
    private TextMesh mesh;
    // [char/second]
    private float cursorRate = 17;
    private int columnLenght = 25;
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
            state = new PresentationState(port.currentTransmission, columnLenght, cursorRate, port.isSource);
        }
		
	}

    class PresentationState
    {
        float curPosF = 0;
        int curPosI = 0;
        Transmission trans;
        string content = "";
        float lastTime;
        float rate;

        public PresentationState(Transmission trans, int columnLenght, float rate, bool incoming)
        {
            this.trans = trans;
            string start = incoming ? "Incoming Transmission... " : "Outgoing Transmission...";
            List<string> items = new List<string> {
               start,
                "Client: " + trans.clientName,
                "Content: " + trans.contentName
            };

            for (int i = 1; i < items.Count; i++)
            {
                if(items[i].Length > columnLenght)
                {
                    items[i] = items[i].Insert(columnLenght, "-\n\t");
                }
            }

            foreach(var s in items) content += s + "\n\n";

            lastTime = Time.time;
            this.rate = rate;
        }


        public bool Run(TextMesh target)
        {
            // transmission over
            if (trans == null) return false;

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
