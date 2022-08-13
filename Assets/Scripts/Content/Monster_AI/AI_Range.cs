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

        // ���� ������
        BT_Selector l_mainSelector = new BT_Selector();


        // ���� ������
        BT_Sequence l_combatSQ = new BT_Sequence();

        // ��ų ���� �׼� �غ� �� ��ų ����
        AI_SkillSelector l_skillselector
            = new AI_SkillSelector(gameObject, m_isSkillSelected, m_SelectedSkill);

        //l_skillselector.AddSkill(����);

        // ���� ������ 1 : �÷��̾� Ž�� �׼�
        l_combatSQ.AddChild(new AI_Combat_Detect(gameObject, m_detectRange));

        // ���� ������ 2 : ��ų ���� �׼�
        l_combatSQ.AddChild(l_skillselector);

        // ���� ������ 3 : ��ų ��Ÿ����� ���� �׼�
        l_combatSQ.AddChild(new AI_Combat_Chase(gameObject, m_SelectedSkill, m_chaseMoveSpeed));

        // ���� ������ 4 : ������ ��ų ��� �׼�
        l_combatSQ.AddChild(new AI_SkillDelegator(gameObject, m_isSkillSelected, m_SelectedSkill));


        // ���� 1 : ��� üũ �׼�
        l_mainSelector.AddChild(new AI_DeathCheck(gameObject, Stat));

        // ���� 2 : ���� ������
        l_mainSelector.AddChild(l_combatSQ);

        // ���� 3 : ����(��������Ʈ�� ��ȸ�ϸ� �̵�) �׼�
        l_mainSelector.AddChild(new AI_Patrol_Waypoint(gameObject, m_patrolMoveSpeed, Patrol_WaypointList));


        // ��Ʈ�� ����
        m_brain.Child = l_mainSelector;
    }

    public override void AddPatrolPoint(Vector3 _position)
    {
        Patrol_WaypointList.Add(_position);
    }
}
