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
    private float               m_explosionRange = 5.0f;            // ���� �ݰ�
    private float               m_explosionDelayTime = 0.0f;        // ���� ���� �ð�
    private float               m_explosionMaxDelayTime = 5.0f;     // �ִ� ���� �ð�
    private Rigidbody           m_rigid = null;
    private int                 m_layer = 1 << (int)Layer.Monster | 1 << (int)Layer.Player;

    private PlayerController    m_owner = null;

    #region ������Ƽ

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

    // �߻縦 ������ �ʿ��� ������ ������
    public void Shoot(PlayerController _owner, Vector3 _position, Vector3 _direction, float _dist)
	{
        _direction *= _dist * m_moveSpeed;
        transform.position = _position;
        m_rigid.velocity = Vector3.zero;
        m_rigid.AddForce(_direction);

        m_owner = _owner;
    }

    public void JumpShoot(PlayerController _owner, Vector3 _position)
	{
        transform.position = _position;
        m_rigid.velocity = Vector3.zero;
        m_rigid.AddForce(-Vector3.up * m_moveSpeed);
        m_state = BoomState.Jump;

        m_owner = _owner;
    }

    public void Explosion()
    {
        RaycastHit[] l_colliders =  Physics.SphereCastAll(transform.position, m_explosionRange, Vector3.up, 1.0f, m_layer);

        int l_multikillCount = 0;
        // ��ȸ�� �Ͽ� üũ�� ����
        foreach(RaycastHit l_hit in l_colliders) {
            Transform l_trans = l_hit.transform;

            // ���� ���Ϳ� ��ź���� �Ÿ��� �˾Ƴ���.
            Vector3 l_subVec = l_trans.position - transform.position;
            l_subVec *= m_explosionForce / l_subVec.magnitude;             // �Ÿ��� �� ���� �� ���� ���� �޴´�.

            // TODO : ����� ��Ʈ ���� �����ϸ� ��
            int l_layer = l_hit.collider.gameObject.layer;
            if (l_layer == (int)Define.Layer.Player) {
                PlayerController l_player = Managers.Game.Player.FindPlayer(l_hit.collider.gameObject.GetInstanceID());
                l_player.GetComponent<Rigidbody>().AddExplosionForce(l_subVec.magnitude, transform.position, 100.0f);
            }
            else if (l_layer == (int)Define.Layer.Monster) {
                AI_Enemy l_monster = l_hit.collider.GetComponent<AI_Enemy>();
                // ���� ���Ͱ� �׾��ٸ�
                if(l_monster.Demege(m_owner, 80, l_subVec / 2.0f) == true) {
                    l_multikillCount += 1;
                }
            }
        }

        if(m_owner.MonsterMultiKillCount < l_multikillCount) {
            m_owner.MonsterMultiKillCount = l_multikillCount;
        }

        m_state = BoomState.Default;
        m_explosionDelayTime = 0.0f;
        m_rigid.velocity = Vector3.zero;

        Managers.Game.Boom.DeSpawn(this);
    }

    public void JumpExplosion()
	{
        RaycastHit[] l_colliders = Physics.SphereCastAll(transform.position, m_explosionRange, Vector3.up, 1.0f, m_layer);

        // ��ȸ�� �Ͽ� üũ�� ����
        foreach (RaycastHit l_hit in l_colliders) {
            Transform l_trans = l_hit.transform;

            // ���� ���Ϳ� ��ź���� �Ÿ��� �˾Ƴ���.
            Vector3 l_subVec = l_trans.position - transform.position;
            l_subVec.y += 10.0f;
            l_subVec *= m_explosionForce / l_subVec.magnitude;             // �Ÿ��� �� ���� �� ���� ���� �޴´�.


            // TODO : ����� ��Ʈ ���� �����ϸ� ��
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

    // ���� ������ �� �������� ī�����Ѵ�.
    private void CheckExplosionTime()
	{
        // ���� �ð��� �����ϸ� �߰��Ѵ�.
        m_explosionDelayTime += Time.deltaTime;

        // ���������� ��� üũ ��ü�� ���Ѵ�.
        if (m_state == BoomState.Delay || m_explosionDelayTime < m_explosionMaxDelayTime) {
            return;
		}

        // ��� �ð��� �ٳ����� ����
        Explosion();
    }

	private void OnCollisionEnter(Collision collision)
	{
        int l_layer = collision.collider.gameObject.layer;
        // ���� ������ ����ų��
        if (m_state == BoomState.Jump && l_layer == (int)Define.Layer.Ground) {
            JumpExplosion();
        }
        // ���Ϳ��� �ε������� (�⺻ ����)
        else if(m_state == BoomState.Default && l_layer == (int)Define.Layer.Monster) {
            Explosion();
		}
	}

	private void OnDrawGizmos()
	{

    }
}
