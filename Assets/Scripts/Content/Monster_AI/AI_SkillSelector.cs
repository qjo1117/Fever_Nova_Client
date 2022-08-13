using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SkillSelector : BT_Action
{
    private AI_Skill m_selectedSkill;
    private bool m_isSkillSelected;
    private List<AI_Skill> m_skills;

    public AI_SkillSelector(GameObject _object, bool _isSkillSelected, AI_Skill _selectedSkill)
    {
        m_object = _object;
        m_isSkillSelected = _isSkillSelected;
        m_selectedSkill = _selectedSkill;
        m_skills = new List<AI_Skill>();
    }

    public void AddSkill(AI_Skill _skill)
    {
        m_skills.Add(_skill);
    }

    public override AI.State Update()
    {
        return SelectSkill();
    }

    private AI.State SelectSkill()
    {
        if (m_isSkillSelected)
        {
            return AI.State.SUCCESS;
        }


        if (m_skills.Count == 0)
        {
            return AI.State.FAILURE;
        }

        foreach (AI_Skill _skill in m_skills)
        {
            // ��Ÿ�� ����
            _skill.CoolDown -= Time.deltaTime;

            // ��Ÿ�� üũ
            if (_skill.CoolDown > 0)
            {
                continue;
            }

            // ���õ� ��ų ���� ����ó��
            if (m_selectedSkill == null)
            {
                m_selectedSkill = _skill;
                m_isSkillSelected = true;
            }

            // �켱���� üũ
            else if (_skill.Priority < m_selectedSkill.Priority)
            {
                m_selectedSkill = _skill;
                m_isSkillSelected = true;
            }
        }

        return m_isSkillSelected ? AI.State.SUCCESS : AI.State.FAILURE;
    }
}