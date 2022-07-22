using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBT : BehaviorTree
{
    // 굳이 Dictional에 있는 데이터를 가져와서 쓰지않아도 된다.
    // 모든 Exacution노드는 Tree를 가져오기 때문에 Tree에 있는 값(프로퍼티)를 참조해도 상관없어진다.

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

        // Sequence : 노드중 실패가 생기면 순회 X
        // Selector : 노드중 성공이 생이면 순회 X

        BehaviorNode root = new Selector(new List<BehaviorNode>
        {
            new Sequence(new List<BehaviorNode>
			{
                new TestCheckRange(this),                   // 범위체크
                new TestMoveToTarget(this),                 // 타겟으로 움직임
                new Inverter(new TestAttack(this)),         // 공격
			}),
            new TestPatrol(this, m_wayPoints),              // 순찰
        });


        return root;
    }


	private void OnDrawGizmos()
	{
        DrawWayPoint();         // 현재 웨이포인트를 보여준다.
        DrawCheckRange();       // 인식 범위를 보여준다.
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
