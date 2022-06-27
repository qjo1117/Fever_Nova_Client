using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TaskPatrol : SkillNode {
    private Transform   m_transform = null;
    private Vector3[]   m_wayPoints;
    private SkillTree   m_skillTree = null;

    private int m_currentWayIndex = 0;

    private float m_speed = 2.0f;

    private float m_waitTime = 1.0f; // in seconds
    private float m_waitCount = 0f;
    private bool m_isWait = false;

    public TaskPatrol(Transform transform, Vector3[] waypoints)
    {
        m_transform = transform;
        m_wayPoints = waypoints;
        m_skillTree = transform.GetComponent<SkillTree>();

        m_speed = (float)m_skillTree.GetData("MoveSpeed");
        if((object)m_speed == null) {
            m_skillTree.SetData("MoveSpeed", (float)10.0f);
        }
    }

    public override SkillNodeState Evaluate()
    {
        if (m_isWait == true) {
            m_waitCount += Time.deltaTime;
            if (m_waitCount >= m_waitTime) {
                m_isWait = false;
            }
        }
        else {
            // 아무것도 없으면 실패
            if(m_wayPoints.Length == 0) {
                return SkillNodeState.FAILURE;
            }

            Vector3 wayPoint = m_wayPoints[m_currentWayIndex];
            if (Vector3.Distance(m_transform.position, wayPoint) < 0.01f) {
                m_transform.position = wayPoint;
                m_waitCount = 0f;
                m_isWait = true;

                m_currentWayIndex = (m_currentWayIndex + 1) % m_wayPoints.Length;
            }
            else {
                m_transform.position = Vector3.MoveTowards(m_transform.position, wayPoint, m_speed * Time.deltaTime);
                m_transform.LookAt(wayPoint);
            }
        }


        m_state = SkillNodeState.RUNNING;
        return m_state;
    }

}