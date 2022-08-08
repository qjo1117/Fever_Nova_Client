using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Boss : AI_Enemy
{
    private float m_chaseDistance;
    public float m_chaseMoveSpeed = 10.0f;
    private AI_Skill m_SelectedSkill;
    private bool m_isSkillSelected;

    protected override void CreateBehaviorTreeAIState()
    {
        m_enemyType = AI.EnemyType.Boss;
        m_brain = new BT_Root();


        m_SelectedSkill = null;
        m_isSkillSelected = false;


        BT_Selector l_mainSelector = new BT_Selector();
        BT_Sequence l_combatSQ = new BT_Sequence();

        AI_SkillSelector l_skillselector
            = new AI_SkillSelector(gameObject, m_SelectedSkill);

        //l_skillselector.AddSkill(근접);
        //l_skillselector.AddSkill(원거리);
        //l_skillselector.AddSkill(쉴드);
        //l_skillselector.AddSkill(돌진);
        //l_skillselector.AddSkill(폭격);

        l_combatSQ.AddChild(l_skillselector);
        l_combatSQ.AddChild(new AI_Combat_Chase(gameObject, m_chaseDistance, m_chaseMoveSpeed);


        l_mainSelector.AddChild(new AI_DeathCheck(gameObject, Stat));
        l_mainSelector.AddChild(l_combatSQ);

        m_brain.Child = l_combatSQ;
    }
}
