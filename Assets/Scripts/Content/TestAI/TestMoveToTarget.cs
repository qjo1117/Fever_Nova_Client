using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveToTarget : Exacution
{
	private float m_moveSpeed = 0.0f;
	private float m_range = 0.0f;
	private PlayerController m_target = null;

    public TestMoveToTarget(BehaviorTree p_tree) : base(p_tree)
	{

		m_moveSpeed = p_tree.GetData<float>("MoveSpeed");
		m_range = p_tree.GetData<float>("CheckRange");
	}

	public override BehaviorStatus Update()
	{
		m_target = m_tree.GetData<PlayerController>("Target");

		// 타겟이 없을 경우 실패
		if (m_target == null) {
			m_status = BehaviorStatus.Failure;
			return m_status;
		}

		// 타겟의 위치를 가져오는 구문이 길어서 단축 시킴
		Vector3 pos = m_target.transform.position;
		Vector3 dist = pos - m_transform.position;			// 거리를 알아보고

		// 타겟과의 거리를 비교한다.
		if (dist.sqrMagnitude <= m_range * m_range) {
			m_status = BehaviorStatus.Success;
			return m_status;
		}

		// 타겟이 범위 밖이라면 그쪽으로 간다.
		m_transform.position = Vector3.MoveTowards(m_transform.position, pos, m_moveSpeed * Time.deltaTime);
		m_transform.LookAt(pos);

		m_status = BehaviorStatus.Running;
		return m_status;

	}
}
