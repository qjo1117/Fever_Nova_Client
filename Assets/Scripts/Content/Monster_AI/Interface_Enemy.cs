using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface_Enemy : MonoBehaviour
{
    [SerializeField]
    private MonsterStatTable    m_stat = null;
    private UI_MonsterHPBar     m_hpBar = null;
    protected BT_Root           m_brain = null;
    protected AI.EnemyType      m_enemyType = AI.EnemyType.Melee;
    public Interface_Skill      m_selectedSkill = null;
    public bool                 m_isSkillSelected = false;
    public bool                 m_isChaseComplete = false;
    public bool                 m_isPlayingChaseAnimation = false;
    public float                m_unitHp = 0;

    [SerializeField]
    protected List<Vector3>     m_patrolWayPoint = new List<Vector3>();
    private Rigidbody           m_rigid = null;

    public MonsterStatTable Stat { get => m_stat; set => m_stat = value; }
    public UI_MonsterHPBar HpBar { get => m_hpBar; set => m_hpBar = value; }

    public void Init()
    {
        m_isPlayingChaseAnimation = false;
        m_isChaseComplete = false;
        m_isSkillSelected = false;
        // Death Flag ����� ����.
        GetComponent<MyAnimator>().FlagClear();

        CreateBrain();
        m_rigid = GetComponent<Rigidbody>();

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().isTrigger = false;
    }

    private void Start()
    {
        //Init();
    }

    public void AddPatrolPoint(Vector3 _position)
    {
        m_patrolWayPoint.Add(_position);
    }

    public void ClearPatrolPoint()
    {
        m_patrolWayPoint.Clear();
    }

    protected virtual void CreateBrain() { }

    protected void Update()
    {
        m_brain.Tick();
    }

    // ���ظ� �Ծ�����
    // Vector�� ���� Defualt���ǰ� �ȵǹǷ� �Լ��� �ΰ��� ������ �۵���Ų ��

    // Ret : �׾��°�?
    // Parameter : PlayerController / Ÿ���� �༮ | int �ǰݴ��� �����
    public bool Damage(PlayerController _player)
    {
        m_stat.hp -= _player.Stat.attack;
        m_hpBar.HP = m_stat.hp;
        // Die
        if (m_stat.hp <= 0) {
            if(m_hpBar is UI_BossMonsterHPBar)
            {
                m_hpBar.CloseSceneUI();
            }
            _player.MonsterKillCount += 1;
            Managers.Game.Monster.MonsterKillCount += 1;
            ClearPatrolPoint();
            return true;
        }
        return false;
    }

    // Parameter : PlayerController / Ÿ���� �༮ | int �ǰݴ��� ����� | Vector3 ��
    public bool Damage(PlayerController _player, Vector3 _force)
    {
        m_rigid.AddForce(_force);
        return Damage(_player);
    }


}
