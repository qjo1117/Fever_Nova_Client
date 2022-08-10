using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Boss : AI_Enemy
{
    public float m_chaseMoveSpeed = 10.0f;
    private AI_Skill m_SelectedSkill;
    private bool m_isSkillSelected;

    protected override void CreateBehaviorTreeAIState()
    {
        m_enemyType = AI.EnemyType.Boss;
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
        //l_skillselector.AddSkill(원거리);

        // 돌진 스킬 추가 
        l_skillselector.AddSkill(new Skill_Charge(gameObject,
            new Vector3(5, 5, 5), 20, 5, 30));

        //l_skillselector.AddSkill();

        // 전투 시퀀스 1 : 스킬 선정 액션
        l_combatSQ.AddChild(l_skillselector);

        // 전투 시퀀스 2 : 스킬 사거리까지 추적 액션
        l_combatSQ.AddChild(new AI_Combat_Chase(gameObject, m_SelectedSkill, m_chaseMoveSpeed));

        // 전투 시퀀스 3 : 선정된 스킬 사용 액션
        l_combatSQ.AddChild(new AI_SkillDelegator(gameObject, m_isSkillSelected, m_SelectedSkill));


        // 메인 1 : 사망 체크 액션
        l_mainSelector.AddChild(new AI_DeathCheck(gameObject, Stat));

        // 메인 2 : 전투 시퀀스
        l_mainSelector.AddChild(l_combatSQ);

        m_brain.Child = l_mainSelector;
    }
}
