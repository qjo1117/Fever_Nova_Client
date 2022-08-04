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

        // �ؼ� �뵵 : ���� �������ϴ� WayPoint�� ������
        Vector3 WayPoint = waypointList[currentWayPoint];

        // WayPoint���� ���µ� �Ÿ��� �̿��� ������ ���Ѵ�.
        float Distance = Vector3.Distance(WayPoint, m_object.transform.position);
        if (Distance < wayPointRadius)
        {
            // ���������� ���� ���� ���� �����ص� �����δ�.
            if (++currentWayPoint >= waypointList.Count)
            {
                // ���� �ִ밪�� �Ѿ�� 0����
                currentWayPoint = 0;
            }
            WayPoint = waypointList[currentWayPoint];

            // �ٵ� �׳� �̰� �����̸� �Ǳ���
            //WayPoint = waypointList[++currentWayPoint % waypointList.Count];
        }
        // �ٶ󺸴� ������ �������ش�.
        Vector3 Dir = WayPoint - m_object.transform.position;
        Dir.y = 0.0f;
        m_object.transform.rotation = Quaternion.Slerp
            (m_object.transform.rotation,
            Quaternion.LookRotation(Dir),
            Time.deltaTime * 8);

        // �̰� �׳� Transform�� �޷��մ� �ɷ� �����̸� ����� �ȿ�����
        m_object.transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);

        // ���� ���� �ڵ�� ���� �ٶ󺸴� ������ WayPoint�� �״� ������ ���� �ָ� Waypoint�� ���� �� ó�� ���δ�.
        //m_rigid.AddForce(Vector3.forward * m_moveSpeed);
    }
}
