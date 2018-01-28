using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmission {
    public static int ID_INCREMENT = 0;
    public int id;
    private Port source;
    private Port destination;
    public ScheduleEntry schedule;

    public Transmission(Port source, Port destination, ScheduleEntry schedule)
    {
        id = ID_INCREMENT++;

        this.source = source;
        this.destination = destination;
        this.schedule = schedule;

        float timeEnd = schedule.endTime; ;

        source.reserve(id, timeEnd, true);
        destination.reserve(id, timeEnd);

        Debug.Log("Transmission [" + id.ToString() + "] starting at " + Time.time.ToString());
    }

    public bool Run()
    {
        if (schedule.EmmitNow())
        {
            Debug.Log("Transmission [" + id.ToString() + "] emmiting at " + Time.time.ToString());
            source.emmit();
        }

        return !schedule.IsOver(Time.time);
    }

    public void End()
    {
        source.Reset();
        destination.Reset();
        Debug.Log("Transmission [" + id.ToString() + "] ending at " + Time.time.ToString());
    }
}
