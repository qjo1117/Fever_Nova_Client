using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite 
{
	public Selector() : base() { }
	public Selector(List<BehaviorNode> p_listChild) : base(p_listChild) { }

	public override BehaviorStatus Update()
	{
		// 가진게 없다면 성공으로 해주고 다음 노드에게 순서를 넘긴다.
		if (EmptyChildren() == true) {
			return BehaviorStatus.Success;
		}

		int size = m_listChildren.Count;

		for (int i = 0; i < size; ++i) {
			BehaviorStatus state = m_listChildren[i].Update();

			// Selector이라 순서대로 순회중 성공or실패가 존재할 경우 성공을 리턴하고 종료
			switch(state) {
				case BehaviorStatus.Success:
					m_status = BehaviorStatus.Success;
					return m_status;
				case BehaviorStatus.Running:
					m_status = BehaviorStatus.Running;
					return m_status;
				case BehaviorStatus.Failure:
					continue;
			}
		}

		// 전부 넘어갔으면 실패
		m_status = BehaviorStatus.Failure;
		return m_status;
	}
}


