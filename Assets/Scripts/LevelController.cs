using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelController : MonoBehaviour {

    public enum FlowName { Light, Medium, High }
    public List<Pipe> pipes;

    [System.Serializable]
    public class ScheduleEntry
    {
        public float time;
        public FlowName type;

        private Queue<float> emmitTicks;
        private float startTime;

        public bool EmmitNow(float timeNow)
        {
            if(emmitTicks.Count == 0) return false; 

            float nextTime = emmitTicks.Peek();

            if((timeNow - startTime) > nextTime)
            {
                emmitTicks.Dequeue();
                return true;
            }

            return false;
        }

        public ScheduleEntry Init(float timeNow)
        {
            startTime = timeNow;
            emmitTicks = new Queue<float>();

            int total = LevelController.FlowLevels[type].NumEmmits();
            float interval = 1 / LevelController.FlowLevels[type].rate;

            for (int i = 0; i < total; i++)
                emmitTicks.Enqueue(interval * i);

            return this;
        }

        public bool IsOver(float runningTime)
        {
            return runningTime >= (time + LevelController.FlowLevels[type].duration);
        }

    }

    public List<ScheduleEntry> schedule;

    struct Flow
    {
        // [Emmits/second]     
        public float rate;
        // [seconds]
        public float duration;

        public int NumEmmits()
        {
            return Mathf.FloorToInt(rate * duration);
        }
    };


    

    
	void Start () {
        scheduleQueue = new Queue<ScheduleEntry>(schedule);
        runningEntries = new List<ScheduleEntry>();
        runningTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        RunSchedule();
        runningTime += Time.deltaTime;
    }

    void RunSchedule()
    {
        DequeueScheduledEntries();

        for (int i = 0; i < runningEntries.Count;)
        {
            ScheduleEntry entry = runningEntries[i];

            if (entry.IsOver(runningTime))
            {
                //Debug.Log("End");
                runningEntries.RemoveAt(i);
            }
            else
            {
                if (entry.EmmitNow(runningTime))
                {
                    //Debug.Log("emmit");
                }
                i++;
            }
        }
    }

    private void DequeueScheduledEntries()
    {
        bool done = false;

        // get the entries we should run
        while (!done)
        {
            if (scheduleQueue.Count == 0)
            {
                break;
            }

            var entry = scheduleQueue.Peek();

            if (entry.time < runningTime)
            {
                //Debug.Log("Start");
                runningEntries.Add(scheduleQueue.Dequeue().Init(runningTime));
            }
            else
            {
                done = true;
            }
        }
    }

    static Dictionary<FlowName, Flow> FlowLevels = new Dictionary<FlowName, Flow>()
    {
        { FlowName.Light , new Flow{ rate = 1.0f,  duration = 1.0f } },
        { FlowName.Medium , new Flow{ rate = 1.5f,  duration = 5.0f } },
        { FlowName.High , new Flow{ rate = 2.0f,  duration = 10.0f } }
    };
    

    float runningTime;
    private List<ScheduleEntry> runningEntries;
    private Queue<ScheduleEntry> scheduleQueue;
}
