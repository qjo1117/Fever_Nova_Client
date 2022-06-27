using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Composite
{
	private bool m_isAllSuccessFail = false;
	private bool m_isSuccessOnAll = false;
	private bool m_isFailOnAll = false;

	private int m_minSuccess = 0;
	private int m_minFail = 0;

	public Parallel(bool p_successOnAll = true, bool p_failOnAll = true)
	{
		m_isAllSuccessFail = true;
		m_isSuccessOnAll = p_successOnAll;
		m_isFailOnAll = p_failOnAll;
	}

	public Parallel(int p_minSuccess, int p_minFail)
	{
		m_minSuccess = p_minSuccess;
		m_minFail = p_minFail;
	}


	public override BehaviorStatus Update() 
	{
		// 체크해야할 게 있으니 확인하기전 캐싱한다.
		int successSize = m_minSuccess;
		int failSize = m_minFail;

		// 전부 성공 실패 Flag가 켜져있는지 확인한다.
		if(m_isAllSuccessFail == true) {
			// 성공이 켜져있는 경우
			if(m_isSuccessOnAll == true) {
				successSize = m_listChildren.Count;
			}
			else {
				successSize = 1;
			}

			// 실패가 켜져있는 경우
			if(m_isFailOnAll == true) {
				failSize = m_listChildren.Count;
			}
			else {
				failSize = 1;
			}
		}

		// 성공, 실패 갯수를 카운팅할 변수
		int successCount = 0;
		int failCount = 0;

		// 순회를 돌면서 성공횟수, 실패횟수를 카운팅한다.
		int size = m_listChildren.Count;
		for (int i = 0; i < size; ++i) {
			BehaviorStatus status = m_listChildren[i].Update();

			if(status == BehaviorStatus.Success) {
				successCount += 1;
			}
			else if (status == BehaviorStatus.Failure){
				failCount += 1;
			}
		}

		// 현재 갯수의 따라서 Parallel 상태를 갱신한다.
		if(successCount >= successSize) {
			m_status = BehaviorStatus.Success;
		}
		else if (failCount >= failSize) {
			m_status = BehaviorStatus.Failure;
		}
		else {
			m_status = BehaviorStatus.Running;
		}

		return m_status;
	}
}
