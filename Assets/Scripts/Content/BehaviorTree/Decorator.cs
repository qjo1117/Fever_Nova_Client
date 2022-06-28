using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : BehaviorNode
{
	// 행동패턴의 데코레이터는 자식 노드를 하나만 가질 수 있다.
	protected BehaviorNode m_child = null;

	public Decorator(BehaviorNode p_child) : base()
	{
		m_child = p_child;
	}

	public override BehaviorStatus Update()
	{
		m_status = m_child.Update();
		return m_status;
	}
}
