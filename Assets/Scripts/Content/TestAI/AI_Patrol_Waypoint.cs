using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patrol_Waypoint : BT_Action
{
    private GameObject m_object;
    private float m_moveSpeed;
    private List<Vector3> waypointList = null;

    private int currentWayPoint = 0;
    private float wayPointRadius = 0.25f;

    public AI_Patrol_Waypoint(GameObject _object, float _moveSpeed, List<Vector3> _waypointList)
    {
        m_object = _object;
        m_moveSpeed = _moveSpeed;
        waypointList = _waypointList;
    }

    public override void Initialize()
    {
      
    }

    public override void Terminate() { }

    public override AI.State Update()
    {
        OnMove();
        return AI.State.RUNNING;
    }

    private void OnMove()
    {
        Vector3 WayPoint = waypointList[currentWayPoint];

        float Distance = Vector3.Distance(WayPoint, m_object.transform.position);
        if (Distance < wayPointRadius)
        {
            if (++currentWayPoint >= waypointList.Count)
            {
                currentWayPoint = 0;
            }
            WayPoint = waypointList[currentWayPoint];
        }
        Vector3 Dir = WayPoint - m_object.transform.position;
        m_object.transform.rotation = Quaternion.Slerp
            (m_object.transform.rotation,
            Quaternion.LookRotation(Dir),
            Time.deltaTime * 8);

        m_object.transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);

    }
}
