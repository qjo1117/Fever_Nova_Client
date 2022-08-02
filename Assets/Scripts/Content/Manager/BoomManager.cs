using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 매니저로 굳이 뺀 이유 : 서버
// 패킷 묶어서 보낼려고

public class BoomManager : MonoBehaviour
{
    private List<Boom> m_listBoom = new List<Boom>();

	[SerializeField]
    private float m_speed = 600;                         // 
	[SerializeField]
    private float m_explosionMaxDelayTime = 5.0f;           // 최대 지연시간
	[SerializeField]
    private float m_explosionForce = 1000.0f;           //  일반적인 힘
	[SerializeField]
	private float m_jumpForce = 3000.0f;
	[SerializeField]
	private float m_ratio = 10.0f;

    public Boom ShootSpawn(Vector3 _position, Vector3 _direction, float _dist)
	{
		Boom l_boom = Managers.Resource.Instantiate("Boom", transform).GetComponent<Boom>();
		
		// 기본적인 스탯 셋팅
		l_boom.Speed = m_speed;
		l_boom.MaxDelayTime = m_explosionMaxDelayTime;
		l_boom.ExplosionForce = m_explosionForce;

		// 폭탄을 날린다.
		l_boom.Shoot(_position, _direction, _dist / m_ratio);
		m_listBoom.Add(l_boom);

		// TODO : 테스트
		Managers.UI.Root.GetComponentInChildren<UI_BombRange>().RangeRadius = _dist;

		return l_boom;
	}

	public Boom JumpSpawn(Vector3 _position)
	{
		Boom l_boom = Managers.Resource.Instantiate("Boom", transform).GetComponent<Boom>();

		// 기본적인 스탯 셋팅
		l_boom.Speed = m_speed;
		l_boom.ExplosionForce = m_jumpForce;

		// 폭탄을 날린다.
		l_boom.JumpShoot(_position);
		m_listBoom.Add(l_boom);

		return l_boom;
	}

	public void DeSpawn(Boom _boom)
	{
		m_listBoom.Remove(_boom);

		// 파티클
		GameObject particle = Managers.Resource.Instantiate(Path.Boom_Particle, transform);
		particle.transform.position = _boom.transform.position;
		Managers.Resource.Destroy(particle, 7.0f);

		Managers.Resource.Destroy(_boom.gameObject);
	}
}
