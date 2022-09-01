using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : BaseController
{

    [SerializeField]
    private MonsterStat         m_stat = new MonsterStat();

    private BehaviorTree        m_behaviorTree = null;

    // UI 상태 갱신 위한 UI오브젝트들
    private UI_MonsterHPBar     m_monsterHPBar;
    private UI_BossMonsterHPBar m_bossMonsterHPBar;
    private UI_Score            m_score;

    // boss 몬스터 체력바 테스트용
    private bool                m_isBoss;

    public MonsterStat          Stat { get => m_stat; set => m_stat = value; }
    public bool                 IsBoss { get => m_isBoss; set => m_isBoss = value; }

    void Start()
    {
        Init();

        // 보스몹일 경우, 보스 몬스터 hp바도 생성
        if (m_isBoss)
        {
            m_bossMonsterHPBar = Managers.UI.MakeWorldSpaceUI<UI_BossMonsterHPBar>(transform,"UI_BossMonsterHPBar");
        }

        // 몬스터 hp바 생성
        m_monsterHPBar = Managers.UI.MakeWorldSpaceUI<UI_MonsterHPBar>(transform,"UI_MonsterHPBar");

        // 이미 생성되어있는 UI_Score ui 가져옴
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


    // 대미지를 입었을때 근데 피격 노드가 따로 들어가므로 어떻게 할지 고민중
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
            // 사망 처리
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
