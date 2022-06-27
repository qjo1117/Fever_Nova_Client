using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Composite
{
	public Sequence() : base() { }
	public Sequence(List<BehaviorNode> p_listChild) : base(p_listChild) { }


	public override BehaviorStatus Update()
	{
		// 가진게 없다면 성공으로 해주고 다음 노드에게 순서를 넘긴다.
		if (EmptyChildren() == true) {
			return BehaviorStatus.Success;
		}

		int size = m_listChildren.Count;

		for (int i = 0; i < size; ++i) {
			BehaviorStatus state = m_listChildren[i].Update();

			// Selector이라 순서대로 순회중 실패가 존재할 경우 바로 리턴한다.
			switch (state) {
				case BehaviorStatus.Success:
					continue;
				case BehaviorStatus.Running:
					continue;
				case BehaviorStatus.Failure:
					m_status = BehaviorStatus.Failure;
					return m_status;
			}
		}

		// 전부 성공했으면 Selector의 상태는 성공이다.
		m_status = BehaviorStatus.Success;
		return m_status;
	}


}
