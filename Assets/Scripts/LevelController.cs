using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowName { Light, Medium, High }


public class LevelController : MonoBehaviour {

    public List<Port> ports;
    public List<ScheduleEntry> schedule;

    public static Dictionary<FlowName, Flow> FlowLevels = new Dictionary<FlowName, Flow>()
    {
        { FlowName.Light , new Flow{ rate = 1.0f,  duration = 1.0f, receiveDurationAfterSend = 5.0f } },
        { FlowName.Medium , new Flow{ rate = 1.5f,  duration = 5.0f, receiveDurationAfterSend = 5.0f } },
        { FlowName.High , new Flow{ rate = 2.0f,  duration = 10.0f, receiveDurationAfterSend = 2.0f } }
    };

    float startTime;
    private List<Transmission> runningEntries;
    private Queue<ScheduleEntry> scheduleQueue;

    void Start () {
        scheduleQueue = new Queue<ScheduleEntry>(schedule);
        runningEntries = new List<Transmission>();
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        RunSchedule();
    }

    void RunSchedule()
    {
        DequeueScheduledEntries();

        for (int i = 0; i < runningEntries.Count;)
        {
            Transmission trans = runningEntries[i];

            if (!trans.Run())
            {
                trans.End();
                runningEntries.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

    private void DequeueScheduledEntries()
    {
        bool done = false;
        float elapsedTime = Time.time - startTime;

        while (!done)
        {
            if (scheduleQueue.Count == 0)
            {
                break;
            }

            var entry = scheduleQueue.Peek();

            if (entry.scheduledStart <=  elapsedTime)
            {
                Port[] freePort = Get2FreePorts(elapsedTime);
                
                Transmission startingTransmission = new Transmission(
                    freePort[0],
                    freePort[1],
                    scheduleQueue.Dequeue().Init()
                );

                runningEntries.Add(startingTransmission);
            }
            else
            {
                done = true;
            }
        }
    }


    Port[] Get2FreePorts(float timeNow)
    {
        List<Port> freePorts = new List<Port>();

        foreach(var p in ports)
        {
            if(p.reservedUntil <= timeNow)
            {
                freePorts.Add(p);
            }
        }

        int numFreePorts = freePorts.Count;

        if (numFreePorts < 2)
        {
            throw new System.Exception("Schedule is trying to get free port pair when there are less than 2");
        }

        int[] rands = new int[] { -1, -1 };
        rands[0] = Random.Range(0, numFreePorts);
            
        int next;

        do
        {
            next = Random.Range(0, numFreePorts);
        } while (rands[0] == next);

        rands[1] = next;

        Random.Range(0, numFreePorts);


        return new Port[] { freePorts[rands[0]], freePorts[rands[1]] };
    }

    public struct Flow
    {
        // [Emmits/second]     
        public float rate;
        // [seconds]
        public float duration;
        // [seconds]
        public float receiveDurationAfterSend;

        public int NumEmmits()
        {
            return Mathf.FloorToInt(rate * duration);
        }
    };
}

[System.Serializable]
public class ScheduleEntry
{
    public float scheduledStart;
    public FlowName type;
    public float endTime { get; private set; }

    private Queue<float> emmitTicks;
    private float actualStart;

    public bool EmmitNow()
    {
        float elapsedTime = Time.time - actualStart;
        if(emmitTicks.Count == 0) return false; 

        float nextTime = emmitTicks.Peek();

        if(elapsedTime > nextTime)
        {
            emmitTicks.Dequeue();
            return true;
        }

        return false;
    }

    public ScheduleEntry Init()
    {
        LevelController.Flow flow = LevelController.FlowLevels[type];
        actualStart = Time.time;
        endTime = actualStart + flow.duration + flow.receiveDurationAfterSend;
        emmitTicks = new Queue<float>();

        int total = flow.NumEmmits();
        float interval = 1 / flow.rate;

        for (int i = 0; i < total; i++)
            emmitTicks.Enqueue(interval * i);

        return this;
    }

    public bool IsOver()
    {
        return Time.time >= endTime;
    }
}