using System.Collections.Generic;
using UnityEngine;

public class AI_Melee : Interface_Enemy
{
    public float m_detectRange = 10.0f;
    public float m_alarmRange = 10.0f;
    public float m_chaseMoveSpeed = 10.0f;
    public float m_patrolMoveSpeed = 10.0f;

    protected override void CreateBrain()
    {
        m_enemyType = AI.EnemyType.Melee;
        m_selectedSkill = null;
        m_isSkillSelected = false;
        m_isChaseComplete = false;
        m_brain = new BT_Root(
            new BT_Selector(
                new BT_Sequence(
                    new Condition_IsDeath(gameObject, Stat,
                    new Action_Death(gameObject, 2.0f))),

                new BT_Sequence(
                    new Action_SkillSelector(this,
                        new Skill_Melee(gameObject, 0011, 0.1f, 3.0f, 1, 10, 1)),

                    new Condition_IsOutofSkillRange(gameObject,
                    new Action_Chase(gameObject, m_chaseMoveSpeed))),

                new BT_Sequence(
                    new Condition_IsSkillRuning(gameObject,
                    new Action_SkillDelegator(gameObject)))
                ));
    }
}
