using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class Boom : MonoBehaviour
{
    public enum BoomState
	{
        Default,
        Delay,
        Jump,
	}

	[SerializeField]
    private BoomState           m_state = BoomState.Default;

    [SerializeField]
    private float               m_explosionForce = 1000.0f;

    [SerializeField]
    private float               m_moveSpeed = 10000.0f;               // 폭탄의 가해지는 힘
    [SerializeField]
    private float               m_explosionRange = 5.0f;            // 폭발 반경
    [SerializeField]
    private float               m_detectRange = 2.0f;               // 감지 반경

    private float               m_explosionDelayTime = 0.0f;        // 현재 지연 시간
    [SerializeField]
    private float               m_explosionMaxDelayTime = 5.0f;     // 최대 지연 시간
    private bool                m_isExplosion = false;

    private PlayerController    m_player = null;
    private Rigidbody           m_rigid = null;

    private int                 m_layer = 1 << (int)Layer.Monster | 1 << (int)Layer.Player;

    private bool                m_isDelayState = false;

    private bool                m_isGround = false;
    private Vector3             m_groundPoint = Vector3.zero;
    private Vector3             m_reflectionNormal = Vector3.zero;


    #region 프로퍼티

    public BoomState State { get => m_state; set => m_state = value; }

    #endregion

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckExplosionTime();
    }

    // 발사를 했을때 필요한 정보를 수집중
    public void Shoot(Vector3 _position, Vector3 _direction)
	{
        transform.position = _position;
        m_rigid.AddForce(_direction * m_moveSpeed);
	}

    public void Jump(Vector3 _position)
	{
        transform.position = _position;
        m_rigid.AddForce(-Vector3.up * m_moveSpeed);
        m_state = BoomState.Jump;
    }

    public void Explosion()
    {
        RaycastHit[] l_colliders =  Physics.SphereCastAll(transform.position, m_explosionRange, Vector3.up, 1.0f, m_layer);

        // 순회를 하여 체크를 진행
        foreach(RaycastHit l_hit in l_colliders) {
            Transform l_trans = l_hit.transform;

            // 현재 몬스터와 폭탄간의 거리를 알아낸다.
            Vector3 l_subVec = l_trans.position - transform.position;
            l_subVec.y = 0;           // 2D로써의 거리만 계산한다.
            l_subVec /= l_hit.collider.GetComponent<Rigidbody>().mass;         // 몬스터의 질량을 참고해서 계산한다.
            l_subVec *= l_subVec.magnitude;             // 거리가 멀 수록 더 크게 힘을 받는다.

            // TODO : 대미지 히트 값을 전달하면 됨
            int l_layer = l_hit.collider.gameObject.layer;
            if (l_layer == (int)Define.Layer.Player) {
                PlayerController l_player = Managers.Game.Player.FindPlayer(l_hit.collider.gameObject.GetInstanceID());
                l_player.GetComponent<Rigidbody>().AddForce(m_explosionForce * l_subVec);

            }
            else if (l_layer == (int)Define.Layer.Monster) {
                BehaviorTree l_monster = l_hit.collider.GetComponent<BehaviorTree>();
            }
        }

        m_state = BoomState.Default;
        m_explosionDelayTime = 0.0f;
        m_rigid.velocity = Vector3.zero;

        // 파티클
        GameObject particle = Managers.Resource.Instantiate(Path.Boom_Particle, Managers.Game.Boom.transform);
        particle.transform.position = transform.position;
        Managers.Resource.Destroy(particle, 7.0f);

        Managers.Resource.Destroy(gameObject);
    }


    // 현재 폭발이 될 것인지를 카운팅한다.
    private void CheckExplosionTime()
	{
        // 만약 시간이 부족하면 추가한다.
        m_explosionDelayTime += Time.deltaTime;

        // 지연상태일 경우 체크 자체를 안한다.
        if (m_state == BoomState.Delay || m_explosionDelayTime < m_explosionMaxDelayTime) {
            return;
		}

        // 대기 시간이 다끝나면 종료
        Explosion();
    }

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.gameObject.layer != (int)Define.Layer.Monster) {
            return;
		}

        Explosion();
	}

	private void OnDrawGizmos()
	{

    }
}
