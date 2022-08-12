using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface_Enemy : MonoBehaviour
{
    [SerializeField]
    private MonsterStatTable m_stat = new MonsterStatTable();
    // TODO : 사유 찾자
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

    // 피해를 입었을때
    // Vector에 대한 Defualt정의가 안되므로 함수를 두개로 나눠서 작동시킨 것

    // Ret : 죽었는가?
    // Parameter : PlayerController / 타격한 녀석 | int 피격당한 대미지
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

    // Parameter : PlayerController / 타격한 녀석 | int 피격당한 대미지 | Vector3 힘
    public bool Damage(PlayerController _player, Vector3 _force)
    {
        m_rigid.AddForce(_force);
        return Damage(_player);
    }
}
