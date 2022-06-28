using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Decorator
{
	public Inverter(BehaviorNode p_child) : base(p_child) { }

	public override BehaviorStatus Update()
	{
		// 실패일 경우는 성공 / 성공일 경우는 실패
		m_status = base.Update() == BehaviorStatus.Failure ? BehaviorStatus.Success : BehaviorStatus.Failure;
		return m_status;
	}
}
