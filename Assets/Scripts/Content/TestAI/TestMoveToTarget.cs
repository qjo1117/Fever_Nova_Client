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
		if (m_target == null || m_target.IsDead == true) {
			m_status = BehaviorStatus.Failure;
			return m_status;
		}

		Vector3 pos = m_target.transform.position;
		Vector3 dist = pos - m_transform.position;

		if (dist.sqrMagnitude <= m_range * m_range) {
			m_status = BehaviorStatus.Success;
			return m_status;
		}

		m_transform.position = Vector3.MoveTowards(m_transform.position, pos, m_moveSpeed * Time.deltaTime);
		m_transform.LookAt(pos);

		m_status = BehaviorStatus.Running;
		return m_status;

	}
}
