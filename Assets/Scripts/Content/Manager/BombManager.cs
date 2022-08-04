using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 매니저로 굳이 뺀 이유 : 서버
// 패킷 묶어서 보낼려고

public class BombManager : MonoBehaviour
{
    private List<Bomb> m_listBoom = new List<Bomb>();

	[SerializeField]
    private float m_speed = 600;                         // 
	[SerializeField]
    private float m_explosionMaxDelayTime = 5.0f;           // 최대 지연시간
	[SerializeField]
    private float m_explosionForce = 1000.0f;           //  일반적인 힘
	[SerializeField]
	private float m_jumpForce = 3000.0f;
	[SerializeField]
	private float m_ratio = 9.0f;

	public void OnUpdate()
	{
		
	}

	public Bomb ShootSpawn(PlayerController _owner, Vector3 _position, Vector3 _direction, float _dist)
	{
		Bomb l_boom = Managers.Resource.Instantiate("Bomb", transform).GetComponent<Bomb>();
		
		// 기본적인 스탯 셋팅
		l_boom.Speed = m_speed;
		l_boom.MaxDelayTime = m_explosionMaxDelayTime;
		l_boom.ExplosionForce = m_explosionForce;

		// 폭탄을 날린다.
		l_boom.Shoot(_owner, _position, _direction, _dist / m_ratio);
		m_listBoom.Add(l_boom);

		return l_boom;
	}

	public Bomb JumpSpawn(PlayerController _owner, Vector3 _position)
	{
		Bomb l_boom = Managers.Resource.Instantiate("Bomb", transform).GetComponent<Bomb>();

		// 기본적인 스탯 셋팅
		l_boom.Speed = m_speed;
		l_boom.ExplosionForce = m_jumpForce;

		// 폭탄을 날린다.
		l_boom.JumpShoot(_owner, _position);
		m_listBoom.Add(l_boom);

		return l_boom;
	}

	public void DeSpawn(Bomb _boom)
	{
		m_listBoom.Remove(_boom);

		// 파티클
		GameObject particle = Managers.Resource.Instantiate(Path.Bomb_Particle, transform);
		particle.transform.position = _boom.transform.position;
		Managers.Resource.Destroy(particle, 7.0f);

		Managers.Resource.Destroy(_boom.gameObject);
	}
}
