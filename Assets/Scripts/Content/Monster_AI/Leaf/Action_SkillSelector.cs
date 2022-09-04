using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_SkillSelector : BT_Action
{
    private List<Interface_Skill> m_skills = new List<Interface_Skill>();

    public Action_SkillSelector(Interface_Enemy _object, params Interface_Skill[] _skills)
    {
        m_object = _object;
        for (int i = 0; i < _skills.Length; i++)
        {
            m_skills.Add(_skills[i]);
        }
    }

    public void AddSkill(Interface_Skill _skill)
    {
        m_skills.Add(_skill);
    }

    protected override AI.State Function()
    {
        foreach (Interface_Skill _skill in m_skills)
        {
            // 쿨타임 갱신
            _skill.CoolDown -= Time.deltaTime;
        }


        if (m_object.GetComponent<Interface_Enemy>().m_isSkillSelected)
        {
            return AI.State.SUCCESS;
        }


        if (m_skills.Count == 0)
        {
            return AI.State.FAILURE;
        }

        foreach (Interface_Skill _skill in m_skills)
        {
            // 쿨타임 체크
            if (_skill.CoolDown <= 0)
            {
                // 선택된 스킬 없음 예외처리
                if (m_object.GetComponent<Interface_Enemy>().m_selectedSkill == null)
                {
                    m_object.GetComponent<Interface_Enemy>().m_selectedSkill = _skill;
                    m_object.GetComponent<Interface_Enemy>().m_isSkillSelected = true;
                    m_object.GetComponent<Interface_Enemy>().m_isChaseComplete = false;
                }

                if (m_object.GetComponent<Interface_Enemy>().m_selectedSkill.CoolDown > 0)
                {
                    m_object.GetComponent<Interface_Enemy>().m_selectedSkill = _skill;
                    m_object.GetComponent<Interface_Enemy>().m_isSkillSelected = true;
                    m_object.GetComponent<Interface_Enemy>().m_isChaseComplete = false;
                }

                // 우선순위 체크
                if (_skill.Priority <= m_object.GetComponent<Interface_Enemy>().m_selectedSkill.Priority)
                {
                    m_object.GetComponent<Interface_Enemy>().m_selectedSkill = _skill;
                    m_object.GetComponent<Interface_Enemy>().m_isSkillSelected = true;
                    m_object.GetComponent<Interface_Enemy>().m_isChaseComplete = false;
                }
            }
        }

        return m_object.GetComponent<Interface_Enemy>().m_isSkillSelected ? AI.State.SUCCESS : AI.State.FAILURE;
    }
}