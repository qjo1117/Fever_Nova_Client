using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPatrol : Exacution {
    private Vector3[] m_wayPoints = null;

    private bool m_isWait = false;
    private float m_waitCount = 0.0f;
    private float m_waitTime = 1.0f;
    private int m_currentWayIndex = 0;

    private float m_speed = 0.0f;

    // 자료가 필요한 경우 초기화를 받는다.
    // 만약 에디터를 만들경우 private로 닫는것보다는 public으로 열어서 에디터에서 접근이 용이하게 만들어줘야할 것
    public TestPatrol(BehaviorTree p_tree, Vector3[] p_wayPoints) : base(p_tree)
    {
        m_wayPoints = p_wayPoints;

        m_speed = m_tree.GetData<float>("MoveSpeed");
    }

    public override BehaviorStatus Update()
    {
        if (m_isWait == true) {
            // 잠깐 기다려준다.
            m_waitCount += Time.deltaTime;
            if (m_waitCount >= m_waitTime) {
                m_isWait = false;
            }
        }
        else {
            // 아무것도 없으면 실패
            if (m_wayPoints.Length == 0) {
                return BehaviorStatus.Failure;
            }

            // 웨이포인트를 가져와서 검사를 한다.
            Vector3 wayPoint = m_wayPoints[m_currentWayIndex];
            // 웨이포인트에 도착할 경우 포인트 인덱스를 교체해준다.
            if (Vector3.Distance(m_transform.position, wayPoint) < 0.01f) {
                m_transform.position = wayPoint;
                m_waitCount = 0f;
                m_isWait = true;

                m_currentWayIndex = (m_currentWayIndex + 1) % m_wayPoints.Length;
            }
            // 아닐경우 웨이포인트쪽으로 간다.
            else {
                m_transform.position = Vector3.MoveTowards(m_transform.position, wayPoint, m_speed * Time.deltaTime);
                m_transform.LookAt(wayPoint);
            }
        }

        // 현재 상태는 실패또는 성공이게 된다.
        m_status = BehaviorStatus.Running;
        return m_status;

    }
}
