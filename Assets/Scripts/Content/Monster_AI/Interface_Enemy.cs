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
        // Death Flag 지우기 위함.
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

    // 피해를 입었을때
    // Vector에 대한 Defualt정의가 안되므로 함수를 두개로 나눠서 작동시킨 것

    // Ret : 죽었는가?
    // Parameter : PlayerController / 타격한 녀석 | int 피격당한 대미지
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

    // Parameter : PlayerController / 타격한 녀석 | int 피격당한 대미지 | Vector3 힘
    public bool Damage(PlayerController _player, Vector3 _force)
    {
        m_rigid.AddForce(_force);
        return Damage(_player);
    }


}
