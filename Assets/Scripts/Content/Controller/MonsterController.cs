using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : BaseController
{

    [SerializeField]
    private MonsterStat         m_stat = new MonsterStat();

    private BehaviorTree        m_behaviorTree = null;

    // UI ���� ���� ���� UI������Ʈ��
    private UI_MonsterHPBar     m_monsterHPBar;
    private UI_BossMonsterHPBar m_bossMonsterHPBar;
    private UI_Score            m_score;

    // boss ���� ü�¹� �׽�Ʈ��
    private bool                m_isBoss;

    public MonsterStat          Stat { get => m_stat; set => m_stat = value; }
    public bool                 IsBoss { get => m_isBoss; set => m_isBoss = value; }

    void Start()
    {
        Init();

        // �������� ���, ���� ���� hp�ٵ� ����
        if (m_isBoss)
        {
            m_bossMonsterHPBar = Managers.UI.MakeWorldSpaceUI<UI_BossMonsterHPBar>(transform,"UI_BossMonsterHPBar");
        }

        // ���� hp�� ����
        m_monsterHPBar = Managers.UI.MakeWorldSpaceUI<UI_MonsterHPBar>(transform,"UI_MonsterHPBar");

        // �̹� �����Ǿ��ִ� UI_Score ui ������
        m_score = Managers.UI.Root.GetComponentInChildren<UI_Score>();

        m_behaviorTree = GetComponent<BehaviorTree>();
    }

	public void FixedUpdate()
	{
        OnUpdate();
    }

	public void PlayerAttack()
	{
	}


    // ������� �Ծ����� �ٵ� �ǰ� ��尡 ���� ���Ƿ� ��� ���� �����
    public void Damege(int p_hp, Vector3 p_force)
    {

    }
    
    public void Damege(int p_hp)
	{
        m_stat.Hp -= p_hp;

        if(m_isBoss)
        {
            m_bossMonsterHPBar.HP = m_stat.Hp;
        }
        m_monsterHPBar.HP = m_stat.Hp;

        if (m_stat.Hp <= 0)
        {
            // ��� ó��
            Managers.Game.Monster.MonsterKillCount += 1;
            Managers.Resource.Destroy(gameObject);
            if(m_isBoss)
            {
                m_bossMonsterHPBar.CloseSceneUI();
            }
            m_score.ScoreLogCreate(m_stat.Score);
            return;
        }
    }

    void Update()
    {
    }



}
