using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : Decorator
{
	private int			m_size = 0;

	public Repeater(BehaviorNode p_child, int p_size = 1) : base(p_child) 
	{
		m_size = p_size;
	}

	public override BehaviorStatus Update()
	{
		m_status = BehaviorStatus.Success;

		for (int i = 0; i < m_size; ++i) {
			if(m_child.Update() == BehaviorStatus.Failure) {
				m_status = BehaviorStatus.Failure;
				break;
			}
		}

		return m_status;
	}
}
