using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCheckRange : Exacution
{
	private float m_range = 0.0f;

    public TestCheckRange(BehaviorTree p_tree) : base(p_tree)
	{
		m_range = m_tree.GetData<float>("CheckRange");
	}

	public override BehaviorStatus Update()
	{
		PlayerController target = m_tree.GetData<PlayerController>("Target");

		// 타겟이 없으면 범위에 대상이 있는지 체크한다.
		if (target == null) {
			// 리스트를 순회해서 거리를 비교한다.
			foreach(PlayerController player in Managers.Game.Player.List) {

			}

			// 타겟이 아직도 없으면 실패
			m_status = BehaviorStatus.Failure;
		}
		// 타겟이 있을 경우
		else {
			m_status = BehaviorStatus.Success;
		}

		return m_status;
	}
}
