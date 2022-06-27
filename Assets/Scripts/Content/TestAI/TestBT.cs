using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBT : BehaviorTree
{
    [SerializeField]
    private Vector3[] m_wayPoints;

    [SerializeField]
    private float m_moveSpeed = 5.0f;

    [SerializeField]
    private float m_checkRange = 5.0f;

    // Start, Update는 따로 호출 안해줘도 됨
    protected override BehaviorNode SetupTree()
	{
        // 이때 데이터 셋팅을 해준다.
        SetData("MoveSpeed", m_moveSpeed);
        SetData("CheckRange", m_checkRange);

        // Sequence : 전부 순회해서 실행
        // Selector : 한녀석만 실행
        BehaviorNode root = new Selector(new List<BehaviorNode>
        {
            new Sequence(new List<BehaviorNode>
			{
                new TestCheckRange(this),
                new TestMoveToTarget(this),
                new Inverter(new TestAttack(this)),
			}),
            new TestPatrol(this, m_wayPoints),
        });

        return root;
    }


	private void OnDrawGizmos()
	{
        DrawWayPoint();
        DrawCheckRange();
    }

	private void DrawWayPoint()
    {
        Gizmos.color = Color.red;

        if (m_wayPoints.Length == 0) {
            return;
        }
        int size = m_wayPoints.Length;
        for (int i = 0; i < size - 1; ++i) {
            Gizmos.DrawLine(m_wayPoints[i], m_wayPoints[i + 1]);
            Gizmos.DrawSphere(m_wayPoints[i], 0.5f);
        }
        Gizmos.DrawSphere(m_wayPoints[size - 1], 0.5f);
    }

    private void DrawCheckRange()
	{
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, m_checkRange);
    }

}
