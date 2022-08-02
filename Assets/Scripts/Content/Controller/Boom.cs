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
    private float               m_explosionForce = 1000.0f;
    private float               m_moveSpeed = 100.0f;               // 폭탄의 가해지는 힘
    private float               m_explosionRange = 5.0f;            // 폭발 반경
    private float               m_explosionDelayTime = 0.0f;        // 현재 지연 시간
    private float               m_explosionMaxDelayTime = 5.0f;     // 최대 지연 시간
    private Rigidbody           m_rigid = null;
    private int                 m_layer = 1 << (int)Layer.Monster | 1 << (int)Layer.Player;


    #region 프로퍼티

    public BoomState State { get => m_state; set => m_state = value; }
    public float Speed { get => m_moveSpeed; set => m_moveSpeed = value; }
    public float MaxDelayTime { get => m_explosionMaxDelayTime; set => m_explosionMaxDelayTime = value; }
    public float ExplosionForce { get => m_explosionForce; set => m_explosionForce = value; }

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
    public void Shoot(Vector3 _position, Vector3 _direction, float _dist)
	{
        _direction *= _dist * m_moveSpeed;
        transform.position = _position;
        m_rigid.velocity = Vector3.zero;
        m_rigid.AddForce(_direction);
	}

    public void JumpShoot(Vector3 _position)
	{
        transform.position = _position;
        m_rigid.velocity = Vector3.zero;
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
            l_subVec *= m_explosionForce / l_subVec.magnitude;             // 거리가 멀 수록 더 적게 힘을 받는다.

            // TODO : 대미지 히트 값을 전달하면 됨
            int l_layer = l_hit.collider.gameObject.layer;
            if (l_layer == (int)Define.Layer.Player) {
                PlayerController l_player = Managers.Game.Player.FindPlayer(l_hit.collider.gameObject.GetInstanceID());
                l_player.GetComponent<Rigidbody>().AddExplosionForce(l_subVec.magnitude, transform.position, 100.0f);

            }
            else if (l_layer == (int)Define.Layer.Monster) {
                AI_Enemy l_monster = l_hit.collider.GetComponent<AI_Enemy>();
                // TODO : 일단 직접적으로 대미지를 가하는 형식으로 테스트
                l_monster.Demege(80, l_subVec / 2.0f);
            }
        }

        m_state = BoomState.Default;
        m_explosionDelayTime = 0.0f;
        m_rigid.velocity = Vector3.zero;

        Managers.Game.Boom.DeSpawn(this);
    }

    public void JumpExplosion()
	{
        RaycastHit[] l_colliders = Physics.SphereCastAll(transform.position, m_explosionRange, Vector3.up, 1.0f, m_layer);

        // 순회를 하여 체크를 진행
        foreach (RaycastHit l_hit in l_colliders) {
            Transform l_trans = l_hit.transform;

            // 현재 몬스터와 폭탄간의 거리를 알아낸다.
            Vector3 l_subVec = l_trans.position - transform.position;
            l_subVec.y += 10.0f;
            l_subVec *= m_explosionForce / l_subVec.magnitude;             // 거리가 멀 수록 더 적게 힘을 받는다.


            // TODO : 대미지 히트 값을 전달하면 됨
            int l_layer = l_hit.collider.gameObject.layer;
            if (l_layer == (int)Define.Layer.Player) {
                PlayerController l_player = Managers.Game.Player.FindPlayer(l_hit.collider.gameObject.GetInstanceID());
                l_player.GetComponent<Rigidbody>().AddForce(l_subVec * 2.0f);

            }
            else if (l_layer == (int)Define.Layer.Monster) {
                AI_Enemy l_monster = l_hit.collider.GetComponent<AI_Enemy>();
            }
        }

        m_state = BoomState.Default;
        m_explosionDelayTime = 0.0f;
        m_rigid.velocity = Vector3.zero;

        Managers.Game.Boom.DeSpawn(this);
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
        int l_layer = collision.collider.gameObject.layer;
        // 점프 폭발을 일으킬때
        if (m_state == BoomState.Jump && l_layer == (int)Define.Layer.Ground) {
            JumpExplosion();
        }
        // 몬스터에게 부딪혔을때 (기본 상태)
        else if(m_state == BoomState.Default && l_layer == (int)Define.Layer.Monster) {
            Explosion();
		}
	}

	private void OnDrawGizmos()
	{

    }
}
