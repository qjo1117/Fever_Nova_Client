using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SkillSelector : BT_Action
{
    private GameObject m_object;
    private AI_Skill m_selectedSkill;
    private List<AI_Skill> m_skills;

    public AI_SkillSelector(GameObject _object, AI_Skill _selectedSkill)
    {
        m_object = _object;
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
        bool l_selected = false;

        if (m_skills.Count == 0)
        {
            return AI.State.FAILURE;
        }

        foreach (AI_Skill _skill in m_skills)
        {
            // 쿨타임 체크
            if (_skill.m_coolDown > 0)
            {
                continue;
            }

            // 선택된 스킬 없음 예외처리
            if (m_selectedSkill == null)
            {
                m_selectedSkill = _skill;
                l_selected = true;
            }

            // 우선순위 체크
            else if (_skill.m_priority < m_selectedSkill.m_priority)
            {
                m_selectedSkill = _skill;
                l_selected = true;
            }
        }

        return l_selected ? AI.State.SUCCESS : AI.State.FAILURE;
    }
}