using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PacketGroup
{
    public Packet packet;
    public Collider segment;
    public bool direction;
}

public class Hose : MonoBehaviour
{
    List<PacketGroup> m_sending = new List<PacketGroup>();
    List<PacketGroup> m_toRemove = new List<PacketGroup>();
    private List<Collider> _hoseSegments;
    private List<Collider> _hoseSegmentsReverse;

    void Start()
    {
        _hoseSegments = GetComponentsInChildren<Collider>().ToList();
        _hoseSegmentsReverse = _hoseSegments.ToList();
        _hoseSegmentsReverse.Reverse();
    }

    public void Send(Packet packet, HoseEnd end)
    {
        foreach (var packetGroup in m_sending)
        {
            if (packetGroup.packet == packet)
            {
                return;
            }
        }

        packet.GetComponent<Rigidbody>().isKinematic = true;
        packet.gameObject.layer = LayerMask.NameToLayer("Details");
        m_sending.Add(new PacketGroup
        {
            packet = packet,
            segment = end.transform.parent.GetComponent<Collider>(),
            direction = end.direction
        });
    }

    void Update()
    {
        float closestDist = float.MaxValue;
        Collider selectedSegment = _hoseSegments.First();

        foreach (var sendingPacket in m_sending)
        {
            if (!sendingPacket.packet)
            {
                m_toRemove.Add(sendingPacket);
            }
            else
            {
                bool hasHitCurrentSegment = false;
                foreach (var hoseSegment in sendingPacket.direction ? _hoseSegments : _hoseSegmentsReverse)
                {
                    if (hasHitCurrentSegment)
                    {
                        var distToCenter = (hoseSegment.transform.position - sendingPacket.packet.transform.position).sqrMagnitude;
                        if (distToCenter < 1f)
                        {
                            selectedSegment = hoseSegment;
                            break;
                        }
                    }

                    if (hoseSegment == sendingPacket.segment)
                    {
                        hasHitCurrentSegment = true;
                    }
                }

                sendingPacket.segment = selectedSegment;
                sendingPacket.packet.transform.position = Vector3.Lerp(sendingPacket.packet.transform.position, selectedSegment.transform.position, Time.deltaTime);
            }
        }

        foreach (var packet in m_toRemove)
        {
            m_sending.Remove(packet);
        }
    }
}
