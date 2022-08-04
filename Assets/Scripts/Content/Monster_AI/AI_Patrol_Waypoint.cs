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

    private Rigidbody m_rigid = null;

    public AI_Patrol_Waypoint(GameObject _object, float _moveSpeed, List<Vector3> _waypointList)
    {
        m_object = _object;
        m_moveSpeed = _moveSpeed;
        waypointList = _waypointList;

        m_rigid = _object.GetComponent<Rigidbody>();
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
        if(waypointList.Count <= 0) {
            return;
		}

        // 해설 용도 : 현재 가고자하는 WayPoint를 선택함
        Vector3 WayPoint = waypointList[currentWayPoint];

        // WayPoint까지 가는데 거리를 이용해 도착을 정한다.
        float Distance = Vector3.Distance(WayPoint, m_object.transform.position);
        if (Distance < wayPointRadius)
        {
            // 도착했으면 다음 값을 증가 시켜준뒤 움직인다.
            if (++currentWayPoint >= waypointList.Count)
            {
                // 만약 최대값을 넘어가면 0으로
                currentWayPoint = 0;
            }
            WayPoint = waypointList[currentWayPoint];

            // 근데 그냥 이거 한줄이면 되긴함
            //WayPoint = waypointList[++currentWayPoint % waypointList.Count];
        }
        // 바라보는 방향을 설정해준다.
        Vector3 Dir = WayPoint - m_object.transform.position;
        Dir.y = 0.0f;
        m_object.transform.rotation = Quaternion.Slerp
            (m_object.transform.rotation,
            Quaternion.LookRotation(Dir),
            Time.deltaTime * 8);

        // 이거 그냥 Transform에 달려잇는 걸로 움직이면 제대로 안움직임
        m_object.transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);

        // 현재 위에 코드로 인해 바라보는 방향이 WayPoint일 테니 앞으로 힘을 주면 Waypoint로 가는 것 처럼 보인다.
        //m_rigid.AddForce(Vector3.forward * m_moveSpeed);
    }
}
