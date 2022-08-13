using System.Collections.Generic;
using UnityEngine;

public class AI_Boss : Interface_Enemy
{
    public float m_detectRange = 10.0f;
    public float m_alarmRange = 10.0f;
    public float m_chaseMoveSpeed = 10.0f;
    public float m_patrolMoveSpeed = 10.0f;
    private List<Vector3> Patrol_WaypointList = new List<Vector3>();

    protected override void CreateBehaviorTreeAIState()
    {
        m_enemyType = AI.EnemyType.Melee;
        m_brain = new BT_Root();
        m_selectedSkill = null;
        m_isSkillSelected = false;
        m_isChaseComplete = false;
        m_isPlayingChaseAnimation = false;

        // ∏ﬁ¿Œ ºø∑∫≈Õ
        BT_Selector l_mainSelector = new BT_Selector();

        BT_Sequence l_DeathSQ = new BT_Sequence();
        l_DeathSQ.AddChild(new Condition_IsDeath(gameObject, Stat));
        l_DeathSQ.AddChild(new Action_Death(gameObject, 10));
        l_mainSelector.AddChild(l_DeathSQ);

        // Gameobject
        // SkillId
        // CoolTime
        // Range
        // Priority
        // ETC
        Condition_SkillSelector l_skillselector = new Condition_SkillSelector(gameObject);
        l_skillselector.AddSkill(new Skill_Bombarment(gameObject, 1001, 20, 8, 1,
            20, 8, 0.5f, 6));
        l_skillselector.AddSkill(new Skill_Charge(gameObject, 1002, 15, 16, 3,
            30, 25, 2, new Vector3(2, 2, 2)));
        l_skillselector.AddSkill(new Skill_Range(gameObject, 1003, 0.9f, 16, 4,
            15, 0.8f, 15, 70.0f, "Pistol-Attack-R1", Path.FX_GlowSpot_01));
        l_skillselector.AddSkill(new Skill_Melee(gameObject, 1004, 0.25f, 8, 5,
            20, 1, "Shield-Attack1", Path.FX_SwordStab_01));


        BT_Sequence l_ReadyforSkillSQ = new BT_Sequence();
        l_ReadyforSkillSQ.AddChild(new Condition_PlayerDetect(gameObject, m_detectRange));
        l_ReadyforSkillSQ.AddChild(l_skillselector);
        l_ReadyforSkillSQ.AddChild(new Condition_IsOutofSkillRange(gameObject));
        l_ReadyforSkillSQ.AddChild(new Action_Chase(gameObject, m_chaseMoveSpeed));
        l_mainSelector.AddChild(l_ReadyforSkillSQ);

        BT_Sequence l_UseSkillSQ = new BT_Sequence();
        l_UseSkillSQ.AddChild(new Condition_IsSkillRuning(gameObject));
        l_UseSkillSQ.AddChild(new Action_SkillDelegator(gameObject));
        l_mainSelector.AddChild(l_UseSkillSQ);

        l_mainSelector.AddChild(new Action_WayPointPatrol(gameObject, m_patrolMoveSpeed, Patrol_WaypointList));

        m_brain.Child = l_mainSelector;
    }

}
