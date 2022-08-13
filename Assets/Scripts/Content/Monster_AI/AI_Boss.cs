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


        // ���� ������
        BT_Selector l_mainSelector = new BT_Selector();


        // ���� ������
        BT_Sequence l_combatSQ = new BT_Sequence();

        // ��ų ���� �׼� �غ� �� ��ų ����
        AI_SkillSelector l_skillselector
            = new AI_SkillSelector(gameObject, m_isSkillSelected, m_SelectedSkill);

        //l_skillselector.AddSkill(����);
        //l_skillselector.AddSkill(���Ÿ�);

        // ���� ��ų �߰� 
        l_skillselector.AddSkill(new Skill_Charge(gameObject,
            new Vector3(5, 5, 5), 20, 5, 30));

        //l_skillselector.AddSkill();

        // ���� ������ 1 : ��ų ���� �׼�
        l_combatSQ.AddChild(l_skillselector);

        // ���� ������ 2 : ��ų ��Ÿ����� ���� �׼�
        l_combatSQ.AddChild(new AI_Combat_Chase(gameObject, m_SelectedSkill, m_chaseMoveSpeed));

        // ���� ������ 3 : ������ ��ų ��� �׼�
        l_combatSQ.AddChild(new AI_SkillDelegator(gameObject, m_isSkillSelected, m_SelectedSkill));


        // ���� 1 : ��� üũ �׼�
        l_mainSelector.AddChild(new AI_DeathCheck(gameObject, Stat));

        // ���� 2 : ���� ������
        l_mainSelector.AddChild(l_combatSQ);

        m_brain.Child = l_mainSelector;
    }
}
