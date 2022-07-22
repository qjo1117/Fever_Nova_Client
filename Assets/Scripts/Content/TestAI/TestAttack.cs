using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack : Exacution
{
	private float m_attackCount = 0.0f;
	private float m_attackTime = 1.0f;

	private float m_range = 0.0f;

	public TestAttack(BehaviorTree p_tree) : base(p_tree)
	{
		m_range = m_tree.GetData<float>("CheckRange");
	}

	public override BehaviorStatus Update()
	{
		PlayerController target = m_tree.GetData<PlayerController>("Target");

		if(target == null) {
			return BehaviorStatus.Failure;
		}

		// 시간 체크
		m_attackCount += Time.deltaTime;
		if (m_attackTime >= m_attackCount) {
			return BehaviorStatus.Failure;
		}
		m_attackCount = 0.0f;
		
		// 거리 체크

		m_status = BehaviorStatus.Success;
		return m_status;
	}
}
