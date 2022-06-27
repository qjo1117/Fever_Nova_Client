using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exacution : BehaviorNode
{
	protected BehaviorTree	m_tree = null;
	protected Transform m_transform = null;

	// 생성자, 아래에 Update는 행동을 만들때 꼭 만들어야함. 
    public Exacution(BehaviorTree p_tree)
	{
		m_tree = p_tree;
		m_transform = p_tree.transform;
	}

	public override BehaviorStatus Update() => BehaviorStatus.Invaild;
}
