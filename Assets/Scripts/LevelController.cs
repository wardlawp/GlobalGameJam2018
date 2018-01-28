﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowName { Light, Medium, Medium_Slow, High }

public class ScheduleException : System.Exception {
    public ScheduleException(string reason) : base(reason) { }
} 
public class LevelController : MonoBehaviour {

    public List<Port> ports;
    public List<ScheduleEntry> schedule;

    public static Dictionary<FlowName, Flow> FlowLevels = new Dictionary<FlowName, Flow>()
    {
        { FlowName.Light , new Flow{ rate = 1.0f,  duration = 1.0f, receiveDurationAfterSend = 7.0f } },
        { FlowName.Medium , new Flow{ rate = 1.5f,  duration = 5.0f, receiveDurationAfterSend = 7.0f } },
        { FlowName.Medium_Slow , new Flow{ rate = 0.5f,  duration = 10.0f, receiveDurationAfterSend = 7.0f } },
        { FlowName.High , new Flow{ rate = 2.0f,  duration = 10.0f, receiveDurationAfterSend = 2.0f } }
    };

    float startTime;
    private List<Transmission> runningEntries;
    private Queue<ScheduleEntry> scheduleQueue;

    // [packets/second]
    public float gameOverFloodRate = 2.0f;
    public int packetLimit = 100;
    private float lastFlood = -1.0f; //init value, don't change
    private bool failed = false;

    void Start () {
        scheduleQueue = new Queue<ScheduleEntry>(schedule);
        runningEntries = new List<Transmission>();
        startTime = Time.time;

        testSchedule();
    }
	
	// Update is called once per frame
	void Update () {

        if (PlayerFailed())
        {
            FloodRoom();
        }
        else
        {
            RunSchedule();
        }
    }

    bool PlayerFailed()
    {
        if(!failed) failed = (GameObject.FindGameObjectsWithTag("packet").Length > packetLimit);

        return failed;
    }

    void FloodRoom()
    {
        if(lastFlood == -1.0f)
        {
            Debug.Log("Flood starting");
            //first flood, reset all ports
            foreach (var p in ports)
            {
                p.Reset();
            }
        }

        float floodInterval = 1 / gameOverFloodRate;
        float timeNow = Time.time;
        if ((lastFlood + floodInterval) < timeNow)
        {
            lastFlood = timeNow;

            foreach(var p in ports)
            {
                p.emmit(true);
            }
        }
    }

    void RunSchedule()
    {
        DequeueScheduledEntries(Time.time, runningEntries, scheduleQueue);

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

    //delegate double QueuedEntryProcessor(ScheduleEntry);

    private void DequeueScheduledEntries(float time, List<Transmission> target, Queue<ScheduleEntry> queue)
    {
        bool done = false;
        float elapsedTime = time - startTime;

        while (!done)
        {
            if (queue.Count == 0)
            {
                break;
            }

            var entry = queue.Peek();

            if (entry.scheduledStart <=  elapsedTime)
            {
                Port[] freePort = Get2FreePorts(elapsedTime);
                
                Transmission startingTransmission = new Transmission(
                    freePort[0],
                    freePort[1],
                    queue.Dequeue().Init(time)
                );

                target.Add(startingTransmission);
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
            throw new ScheduleException("Schedule is trying to get free port pair when there are less than 2");
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

    void testSchedule()
    {
        // deep copy
        int maxConcurrent = ports.Count;
        Queue<ScheduleEntry> queue = new Queue<ScheduleEntry>(scheduleQueue);
        List<Transmission> running = new List<Transmission>();
        float time = 0.0f;

        bool allTransmissionRun = false;
        try
        {
            while (queue.Count != 0 || !allTransmissionRun)
            {
                DequeueScheduledEntries(time, running, queue);

                for (int i = 0; i < running.Count;)
                {
                    Transmission trans = running[i];

                    if (trans.schedule.IsOver(time))
                    {
                        running.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                allTransmissionRun = (running.Count == 0);

                time += 0.33f;
            }
        } catch (ScheduleException s)
        {
            Debug.Log("Schdule is bad. Entry at " + queue.Peek().scheduledStart + "s starts too early.");
            throw s;
        }
        


        Debug.Log("Schedule is good");

        foreach (var p in ports) p.Reset(true);
        Transmission.ID_INCREMENT = 0;
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

    private Queue<float> emmitionTimes;
    private float actualStart;

    public bool EmmitNow()
    {
        float elapsedTime = Time.time - actualStart;
        if(emmitionTimes.Count == 0) return false; 

        float nextTime = emmitionTimes.Peek();

        if(elapsedTime > nextTime)
        {
            emmitionTimes.Dequeue();
            return true;
        }

        return false;
    }

    public ScheduleEntry Init(float time)
    {
        LevelController.Flow flow = LevelController.FlowLevels[type];
        actualStart = time;
        endTime = actualStart + flow.duration + flow.receiveDurationAfterSend;
        emmitionTimes = new Queue<float>();

        int total = flow.NumEmmits();
        float interval = 1 / flow.rate;

        for (int i = 0; i < total; i++)
            emmitionTimes.Enqueue(interval * i);

        return this;
    }

    public bool IsOver(float time)
    {
        return time >= endTime;
    }
}