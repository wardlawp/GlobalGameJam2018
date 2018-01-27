using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmission {

    public Transmission(Port source, Port destination, ScheduleEntry schedule)
    {
        this.source = source;
        this.destination = destination;
        this.schedule = schedule;

        source.reservedUntil = schedule.EndTime();
    }

    public Port source;
    public Port destination;
    public ScheduleEntry schedule;
}
