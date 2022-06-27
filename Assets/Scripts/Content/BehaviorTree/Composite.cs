using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composite : BehaviorNode
{
	protected List<BehaviorNode> m_listChildren = new List<BehaviorNode>();
	protected int m_index = 0;

	public Composite() : base() { }
	public Composite(List<BehaviorNode> p_listChild) 
	{ 
		foreach(BehaviorNode child in p_listChild) {
			Attach(child);
		}
	}

	// 가진게 없으면 빈상태
	public bool EmptyChildren()
	{
		return m_listChildren.Count == 0;
	}

	// 연결
	public void Attach(BehaviorNode p_node)
	{
		m_listChildren.Add(p_node);
	}
	
	// 연결 끊음
	public void UnAttach(BehaviorNode p_node)
	{
		m_listChildren.Remove(p_node);
	}

	// 아직 추상단계라서
	public override BehaviorStatus Update() => BehaviorStatus.Invaild;

}
