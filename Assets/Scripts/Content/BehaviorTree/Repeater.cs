using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : Decorator
{
	private int			m_size = 0;

	public Repeater(BehaviorNode p_child, int p_size) : base(p_child) 
	{
		m_size = p_size;
	}

	public override BehaviorStatus Update()
	{
		m_status = BehaviorStatus.Success;

		for (int i = 0; i < m_size; ++i) {
			// 가진 횟수만큼 순회한다고 했는데 실패했을 경우는 일단 Failure로 빼내게 함.
			if(base.Update() == BehaviorStatus.Failure) {
				m_status = BehaviorStatus.Failure;
				break;
			}
		}

		return m_status;
	}
}
