using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface_Enemy : MonoBehaviour
{
    [SerializeField]
    private MonsterStatTable m_stat = new MonsterStatTable();
    // TODO : ���� ã��
    volatile private UI_MonsterHPBar m_hpBar = null;
    protected BT_Root m_brain;
    protected AI.EnemyType m_enemyType;
    public Interface_Skill m_selectedSkill;
    public bool m_isSkillSelected;
    public bool m_isChaseComplete;
    public bool m_isPlayingChaseAnimation;

    private Rigidbody m_rigid = null;

    public MonsterStatTable Stat { get => m_stat; set => m_stat = value; }
    public UI_MonsterHPBar HpBar { get => m_hpBar; set => m_hpBar = value; }

    public void Init()
    {
        CreateBehaviorTreeAIState();
        m_rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Init();
    }

    public virtual void AddPatrolPoint(Vector3 _position) { }

    protected virtual void CreateBehaviorTreeAIState() { }

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
        m_stat.HP -= _player.Stat.attack;
        m_hpBar.HP = m_stat.HP;
        // Die
        if (m_stat.HP <= 0)
        {
            _player.MonsterKillCount += 1;
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
