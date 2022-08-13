using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_WayPointPatrol : BT_Action
{
    private GameObject m_object;
    private float m_moveSpeed;
    private List<Vector3> waypointList = null;

    private int currentWayPoint = 0;
    private float wayPointRadius = 1.0f;

    private Rigidbody m_rigid = null;

    public Action_WayPointPatrol(GameObject _object, float _moveSpeed, List<Vector3> _waypointList)
    {
        m_object = _object;
        m_moveSpeed = _moveSpeed;
        waypointList = _waypointList;
        wayPointRadius = _object.transform.localScale.x + 0.25f;

        m_rigid = _object.GetComponent<Rigidbody>();
    }

    public override void Initialize()
    {

    }

    public override void Terminate() { }

    public override AI.State Update()
    {
        if (waypointList.Count == 1) {
            return AI.State.FAILURE;
        }
        OnMove();
        return AI.State.RUNNING;
    }

    private void OnMove()
    {
        if (waypointList.Count <= 0)
        {
            return;
        }

        // �ؼ� �뵵 : ���� �������ϴ� WayPoint�� ������
        Vector3 wayPoint = waypointList[currentWayPoint];

        // WayPoint���� ���µ� �Ÿ��� �̿��� ������ ���Ѵ�.
        Vector3 direction = wayPoint - m_object.transform.position;
        direction.y = 0.0f;

        float distance = direction.sqrMagnitude;
        if (distance < wayPointRadius * wayPointRadius) {
            currentWayPoint = ++currentWayPoint % waypointList.Count;
            wayPoint = waypointList[currentWayPoint];
        }

        m_object.transform.rotation = Quaternion.Slerp
            (m_object.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * 8);

        // �̰� �׳� Transform�� �޷��մ� �ɷ� �����̸� ����� �ȿ�����
        m_object.transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);
    }
}
