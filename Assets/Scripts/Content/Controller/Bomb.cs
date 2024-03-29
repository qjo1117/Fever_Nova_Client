using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class Bomb : MonoBehaviour
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
    private float               m_moveSpeed = 100.0f;               // ��ź�� �������� ��
    private float               m_explosionRange = 7.0f;            // ���� �ݰ�
    private float               m_explosionDelayTime = 0.0f;        // ���� ���� �ð�
    private float               m_explosionMaxDelayTime = 5.0f;     // �ִ� ���� �ð�
    private float               m_explosionDamage = 100.0f;
    private Rigidbody           m_rigid = null;
    private int                 m_layer = 1 << (int)Layer.Monster | 1 << (int)Layer.Player;

    private PlayerController    m_owner = null;

    #region Property

    public BoomState State { get => m_state; set => m_state = value; }
    public float Speed { get => m_moveSpeed; set => m_moveSpeed = value; }
    public float MaxDelayTime { get => m_explosionMaxDelayTime; set => m_explosionMaxDelayTime = value; }
    public float ExplosionForce { get => m_explosionForce; set => m_explosionForce = value; }
    public float ExplosionRange { get => m_explosionRange; set => m_explosionRange = value; }
    public float ExplosionDamage { get => m_explosionDamage; set => m_explosionDamage = value; }

    #endregion

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckExplosionTime();
    }

    // �߻縦 ������ �ʿ��� ������ ������
    public void Shoot(PlayerController _owner, Vector3 _position, Vector3 _direction, float _dist)
	{
        _direction *= _dist * m_moveSpeed;
        transform.position = _position;
        m_rigid.velocity = Vector3.zero;
        m_rigid.AddForce(_direction);

        m_owner = _owner;

        transform.rotation = Quaternion.identity;
    }

    public void JumpShoot(PlayerController _owner, Vector3 _position)
	{
        transform.position = _position;
        m_rigid.velocity = Vector3.zero;
        m_rigid.AddForce(-Vector3.up * m_moveSpeed);
        m_state = BoomState.Jump;

        m_owner = _owner;

        transform.rotation = Quaternion.identity;
    }

    public void Explosion()
    {
		//      // OverlapSphere가 제대로 안됨
		//      List<PlayerController> l_players = Managers.Game.Player.List;
		//foreach (PlayerController l_player in l_players) {
		//          Vector3 l_destPos = l_player.transform.position;      // 플레이어 위치
		//          Vector3 l_pos = transform.position;                 // 폭탄 위치

		//          Vector3 l_dist = l_destPos - l_pos;
		//          // 충돌 검사
		//          if (l_dist.sqrMagnitude < m_explosionRange * m_explosionRange) {
		//              l_dist *= m_explosionForce / l_dist.magnitude;             // �Ÿ��� �� ���� �� ���� ���� �޴´�.
		//              l_player.GetComponent<Rigidbody>().AddForce(l_dist * 2.0f);
		//          }
		//      }

		//      List<AI_Enemy> l_monsters = Managers.Game.Monster.List;
		//      List<TargetData> l_datas = new List<TargetData>();
		//      int l_size = l_monsters.Count;
		//      for (int i = 0; i < l_size; ++i) {
		//          AI_Enemy l_monster = l_monsters[i];
		//          Vector3 l_dist = l_monster.transform.position - transform.position;
		//          // 충돌 검사
		//          if (l_dist.sqrMagnitude < m_explosionRange * m_explosionRange) {
		//              l_dist *= m_explosionForce / l_dist.magnitude;             // �Ÿ��� �� ���� �� ���� ���� �޴´�.
		//              l_datas.Add(new TargetData(i, m_owner.Stat.id, m_owner.Stat.attack, l_dist));
		//          }
		//      }
		//      Managers.Game.Monster.Demege(l_datas);

		List<TargetData> l_datas = new List<TargetData>();
		Collider[] l_colliders = Physics.OverlapSphere(transform.position, m_explosionRange, m_layer);
		int l_multikillCount = 0;
		foreach (Collider hit in l_colliders) {
			Transform l_trans = hit.transform;

			Vector3 l_subVec = l_trans.position - transform.position;
			l_subVec *= m_explosionForce / l_subVec.magnitude;             // �Ÿ��� �� ���� �� ���� ���� �޴´�.

			int l_layer = hit.GetComponent<Collider>().gameObject.layer;
			if (l_layer == (int)Define.Layer.Player) {
				PlayerController l_player = hit.GetComponent<PlayerController>();
                l_player.GetComponent<Rigidbody>().AddExplosionForce(l_subVec.magnitude, transform.position, 100.0f);
			}
			else if (l_layer == (int)Define.Layer.Monster){
                Interface_Enemy l_monster = hit.GetComponent<Collider>().GetComponent<Interface_Enemy>();
                l_datas.Add(new TargetData(l_monster.Stat.index, m_owner.StatTable.id, (int)m_explosionDamage, l_subVec / 2.0f));
			}
		}
		if (m_owner.MonsterMultiKillCount < l_multikillCount) {
			m_owner.MonsterMultiKillCount = l_multikillCount;
		}
		Managers.Game.Monster.Demege(l_datas);

		m_state = BoomState.Default;
        m_explosionDelayTime = 0.0f;
        m_rigid.velocity = Vector3.zero;
        m_owner.NullBomb();
        
        Managers.Game.Boom.DeSpawn(this);
    }

    public void JumpExplosion()
	{
        //      // OverlapSphere가 제대로 안됨
        //      List<PlayerController> l_players = Managers.Game.Player.List;
        //      foreach(PlayerController l_player in l_players) {
        //          Vector3 l_destPos = l_player.transform.position;      // 플레이어 위치
        //          Vector3 l_pos = transform.position;                 // 폭탄 위치

        //          Vector3 l_dist = l_destPos - l_pos;
        //          // 충돌 검사
        //          if(l_dist.sqrMagnitude < m_explosionRange * m_explosionRange) {
        //              l_dist.y += 10.0f;
        //              l_dist *= m_explosionForce / l_dist.magnitude;             // �Ÿ��� �� ���� �� ���� ���� �޴´�.
        //              l_player.GetComponent<Rigidbody>().AddForce(l_dist * 2.0f);
        //          }
        //}

        //      List<AI_Enemy> l_monsters = Managers.Game.Monster.List;
        //      List<TargetData> l_datas = new List<TargetData>();
        //      int l_size = l_monsters.Count;
        //      for (int i = 0; i < l_size; ++i) {
        //          AI_Enemy l_monster = l_monsters[i];
        //          Vector3 l_dist = l_monster.transform.position - transform.position;
        //          // 충돌 검사
        //          if (l_dist.sqrMagnitude < m_explosionRange * m_explosionRange) {
        //              l_dist *= m_explosionForce / l_dist.magnitude;             // �Ÿ��� �� ���� �� ���� ���� �޴´�.
        //              l_datas.Add(new TargetData(i, m_owner.Stat.id, m_owner.Stat.attack, l_dist));
        //          }
        //      }
        //      Managers.Game.Monster.Damege(l_datas);


        List<TargetData> l_datas = new List<TargetData>();
        Collider[] l_colliders = Physics.OverlapSphere(transform.position, m_explosionRange, m_layer);
		foreach (Collider hit in l_colliders) {
			Transform l_trans = hit.transform;
			Vector3 l_subVec = l_trans.position - transform.position;
			l_subVec.y += 10.0f;
			l_subVec *= m_explosionForce / l_subVec.magnitude;             // �Ÿ��� �� ���� �� ���� ���� �޴´�.

			int l_layer = hit.gameObject.layer;
			if (l_layer == (int)Define.Layer.Player) {
				PlayerController l_player = hit.GetComponent<PlayerController>();
                l_player.GetComponent<Rigidbody>().AddForce(l_subVec * 2.0f);

			}
			else if (l_layer == (int)Define.Layer.Monster) {
                Interface_Enemy l_monster = hit.GetComponent<Interface_Enemy>();
                l_subVec *= m_explosionForce / l_subVec.magnitude;
                l_datas.Add(new TargetData(l_monster.Stat.index, m_owner.StatTable.id, (int)m_explosionDamage, l_subVec));
            }
        }
        Managers.Game.Monster.Demege(l_datas);

		m_state = BoomState.Default;
        m_explosionDelayTime = 0.0f;
        m_rigid.velocity = Vector3.zero;
        m_owner.NullBomb();

        Managers.Game.Boom.DeSpawn(this);
    }

    // ���� ������ �� �������� ī�����Ѵ�.
    private void CheckExplosionTime()
	{
        m_explosionDelayTime += Time.deltaTime;
        if (m_explosionDelayTime < m_explosionMaxDelayTime) {
            return;
		}
        Explosion();
    }

	private void OnCollisionEnter(Collision collision)
	{
        int l_layer = collision.collider.gameObject.layer;
        if (m_state == BoomState.Jump && l_layer == (int)Define.Layer.Ground) {
            JumpExplosion();
        }
        else if(m_state == BoomState.Default && l_layer == (int)Define.Layer.Monster) {
            Explosion();
		}
	}

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_explosionRange);
    }
}
