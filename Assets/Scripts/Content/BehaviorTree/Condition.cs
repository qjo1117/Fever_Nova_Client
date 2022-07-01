using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : Decorator 
{
	protected Func<bool> m_func = null;

	public Condition(BehaviorNode p_child, Func<bool> p_func) : base(p_child) { m_func = p_func; }

	public override BehaviorStatus Update()
	{
		if(m_func.Invoke() == true) {
			m_status = BehaviorStatus.Success;
			m_child.Update();
		}
		else {
			m_status = BehaviorStatus.Failure;
		}
		return m_status;
	}
}
