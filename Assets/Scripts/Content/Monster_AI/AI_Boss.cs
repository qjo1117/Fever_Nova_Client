using System.Collections.Generic;
using UnityEngine;

public class AI_Boss : Interface_Enemy
{
    public float m_detectRange = 10.0f;
    public float m_alarmRange = 10.0f;
    public float m_chaseMoveSpeed = 10.0f;
    public float m_patrolMoveSpeed = 10.0f;
    private List<Vector3> Patrol_WaypointList = new List<Vector3>();
    public Transform m_muzzle = null;

    protected override void CreateBrain()
    {
        m_enemyType = AI.EnemyType.Melee;
        m_selectedSkill = null;
        m_isSkillSelected = false;
        m_isChaseComplete = false;
        m_isPlayingChaseAnimation = false;
        m_brain = new BT_Root(
            new BT_Selector(new BT_Sequence(
                new Condition_IsDeath(gameObject, Stat,
                new Action_Death(gameObject, 5.0f))),

                new BT_Sequence(
                    new Action_SkillSelector(this,
                        new Skill_Bombarment(gameObject, 1001, 20.0f, 8, 1, 20, 8, 0.5f, 6)),
                        new Skill_Charge(gameObject, 1002, 15.0f, 16, 3, 30, 25, 2, new Vector3(2, 2, 2)),
                        new Skill_BossRange(gameObject, 1003, 0.9f, 16, 4, 15, 0.8f, 15, 70.0f, m_muzzle, "Pistol-Attack-R1", Path.Robot_Bullet),
                        new Skill_Melee(gameObject, 1004, 0.25f, 8, 5, 20, 1, "Shield-Attack1", Path.FX_SwordStab_01),                 
                    new Condition_IsOutofSkillRange(gameObject,
                    new Action_Chase(gameObject, m_chaseMoveSpeed))),

                new BT_Sequence(
                    new Condition_IsSkillRuning(gameObject,
                    new Action_SkillDelegator(gameObject)))
                 ));     
    }
}