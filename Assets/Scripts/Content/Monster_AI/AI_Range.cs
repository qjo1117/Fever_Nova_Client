using System.Collections.Generic;
using UnityEngine;

public class AI_Range : AI_Enemy
{
    public float m_detectRange = 10.0f;
    public float m_alarmRange = 10.0f;
    public float m_chaseMoveSpeed = 10.0f;
    public float m_patrolMoveSpeed = 10.0f;

    private AI_Skill m_SelectedSkill;
    private bool m_isSkillSelected;

    private List<Vector3> Patrol_WaypointList = new List<Vector3>();

    protected override void CreateBehaviorTreeAIState()
    {
        m_enemyType = AI.EnemyType.Melee;
        m_brain = new BT_Root();
        m_SelectedSkill = null;
        m_isSkillSelected = false;

        // 메인 셀렉터
        BT_Selector l_mainSelector = new BT_Selector();


        // 전투 시퀀스
        BT_Sequence l_combatSQ = new BT_Sequence();

        // 스킬 선정 액션 준비 및 스킬 부착
        AI_SkillSelector l_skillselector
            = new AI_SkillSelector(gameObject, m_isSkillSelected, m_SelectedSkill);

        //l_skillselector.AddSkill(근접);

        // 전투 시퀀스 1 : 플레이어 탐지 액션
        l_combatSQ.AddChild(new AI_Combat_Detect(gameObject, m_detectRange));

        // 전투 시퀀스 2 : 스킬 선정 액션
        l_combatSQ.AddChild(l_skillselector);

        // 전투 시퀀스 3 : 스킬 사거리까지 추적 액션
        l_combatSQ.AddChild(new AI_Combat_Chase(gameObject, m_SelectedSkill, m_chaseMoveSpeed));

        // 전투 시퀀스 4 : 선정된 스킬 사용 액션
        l_combatSQ.AddChild(new AI_SkillDelegator(gameObject, m_isSkillSelected, m_SelectedSkill));


        // 메인 1 : 사망 체크 액션
        l_mainSelector.AddChild(new AI_DeathCheck(gameObject, Stat));

        // 메인 2 : 전투 시퀀스
        l_mainSelector.AddChild(l_combatSQ);

        // 메인 3 : 순찰(웨이포인트를 순회하며 이동) 액션
        l_mainSelector.AddChild(new AI_Patrol_Waypoint(gameObject, m_patrolMoveSpeed, Patrol_WaypointList));


        // 루트에 연결
        m_brain.Child = l_mainSelector;
    }

    public override void AddPatrolPoint(Vector3 _position)
    {
        Patrol_WaypointList.Add(_position);
    }
}
