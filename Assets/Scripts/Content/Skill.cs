using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 이거 스킬 조금 더 생각해보자
// 자신이 가리키는 객체한테 있어야하지 않을까
// 그러면 갈아야한다는 생각도 해야봐야한다.

[System.Serializable]
public class Skill : Ability 
{
	[SerializeField]
	private float m_detectRange = 2.0f;         // 감지 범위

	private ParticleSystem m_particle = null;
	private Action m_action = null;

	public float DectedRange { get => m_detectRange; set => m_detectRange = value; }
	
	public void Init(float p_maxTime, string p_name, Action p_action, string p_particle = "None")
	{
		base.Init(p_maxTime, p_name);
		m_action = p_action;

		if(p_particle.Contains("None") == false){ 
			m_particle = Managers.Resource.NewPrefab(p_particle).GetComponent<ParticleSystem>();
		}
	}

	public override void Action()
	{
		m_particle.gameObject.SetActive(true);
		m_particle.Play();
		base.Action();
		m_action();
	}

	public override void Update()
	{	
		base.Update();		// 쿨 타임은 돌린다.

		// 땜빵
		if(m_isAction == true && m_particle != null) {
			m_particle.gameObject.SetActive(false);
		}
	}


}
