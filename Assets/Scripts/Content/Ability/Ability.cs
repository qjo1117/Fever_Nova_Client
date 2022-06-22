using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------------------------------------------------------------------------------
		사실 Action을 넣어서 바인딩된 함수를 자동으로 호출할까 생각했지만
		피버노바에서 쓰는 시간 체크 시스템은 선택, 입력에 따른 함수 호출이기때문에
		사용하는 객체마다 체크하는 형식으로 제작햇습니다.
------------------------------------------------------------------------------- */


// 쿨타임이 존재하는 기능들에 대해서
// 쿨타임 돌리는 함수, 변수 지정하기 귀찮아서 만듬
public class Ability : MonoBehaviour
{
	private float m_currentTime = 0.0f;         // 현재시간
	[SerializeField]
	private string m_name = "Unkown";
	[SerializeField]
	private float m_maxTime = 0.0f;             // 최대시간
	[SerializeField]
	private bool m_isAction = false;			// 체크

	public void Init(float p_maxTime, string p_name)
	{
		m_maxTime = p_maxTime;
		m_name = p_name;
	}

	// 이런 체크되는 것들은 함수가 편해서 썼습니다.
	public bool IsAction() 
	{ 
		return m_isAction; 
	}

	// 행동을 했으면 호출해주자.
	public void Action()
	{
		m_isAction = false;
		m_currentTime = 0.0f;
	}

	private void Update()
	{
		if(m_isAction == true) {
			return;
		}

		// 시간체크해야할때만 체크하자.
		m_currentTime += Time.deltaTime;

		if (m_currentTime < m_maxTime) {
			return;
		}

		m_isAction = true;
	}
}

