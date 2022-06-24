using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------------------------------------------------------------------------------
		사실 Action을 넣어서 바인딩된 함수를 자동으로 호출할까 생각했지만
		피버노바에서 쓰는 시간 체크 시스템은 선택, 입력에 따른 함수 호출이기때문에
		사용하는 객체마다 체크하는 형식으로 제작햇습니다.


		현재 Component를 일일히 등록하는 건 낭비라고 생각해서
		Ability를 여러가지로 쓸수 있게 만들었습니다.
------------------------------------------------------------------------------- */



// TODO :  이거 클래스 매니저로 묶자

[System.Serializable]
public class Ability 
{
	protected float m_currentTime = 0.0f;         // 현재시간
	[SerializeField]
	protected string m_name = "Unkown";
	[SerializeField]
	protected float m_maxTime = 0.0f;             // 최대시간
	[SerializeField]
	protected bool m_isAction = false;            // 체크

	public float MaxTime { get => m_maxTime; set => m_maxTime = value; }
	public string Name { get => m_name; set => m_name = value; }
	public bool IsAction { get => m_isAction; set => m_isAction = value; }

	virtual public void Init(float p_maxTime, string p_name)
	{
		m_maxTime = p_maxTime;
		m_name = p_name;
	}

	// 행동을 했으면 호출해주자.
	virtual public void Action()
	{
		m_isAction = false;
		m_currentTime = 0.0f;
	}

	virtual public void Update()
	{
		if (m_isAction == true) {
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


// 굳이 이렇게 하는 이유는 코루틴을 못믿어서

// 쿨타임이 존재하는 기능들에 대해서
// 쿨타임 돌리는 함수, 변수 지정하기 귀찮아서 만듬
public class AbilitySystem : MonoBehaviour
{
	[SerializeField]
	private List<Ability> m_listAbility = new List<Ability>();

	public List<Ability> Ability { get => m_listAbility; }

	// 리스트
	//		- Ability
	//			- Name
	//			- MaxTime
	//			- IsAction


	public void Start()
	{
		
	}

	public void Clear()
	{
		// C#은 애매한게 이래도 가비지에 넣어지는건가?
		m_listAbility.Clear();
	}


	public void AddAbility(string p_name, float p_maxTime)
	{
		Ability ability = new Ability();

		ability.Init(p_maxTime, p_name);

		m_listAbility.Add(ability);
	}

	public void AddSkill(string p_name, float p_maxTime,  Action p_action, string p_particle = "None")
	{
		Skill skill = new Skill();

		skill.Init(p_maxTime, p_name, p_action, p_particle);

		m_listAbility.Add(skill);
	}

	public void DelAbility(string p_name)
	{
		foreach(Ability ability in m_listAbility) {
			if(ability.Name.Contains(p_name) == true) {
				m_listAbility.Remove(ability);
				break;
			}
		}
	}


	// 체크함수
	public bool IsAction(string p_name)
	{
		foreach (Ability ability in m_listAbility) {
			if (ability.Name.Contains(p_name) == true) {
				return ability.IsAction;
			}
		}

		return false;
	}

	public void Update()
	{
		// 전체적으로 검사해준다.
		foreach(Ability ability in m_listAbility) {
			ability.Update();
		}
	}

}

