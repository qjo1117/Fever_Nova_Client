using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_SkillDelegator : BT_Action
{
    public Action_SkillDelegator(GameObject _object)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<Animator>();
    }

    protected override AI.State Function()
    {
        if (m_object.m_selectedSkill != null)
        {
            if (m_object.m_selectedSkill.Tick() == AI.State.SUCCESS)
            {
                m_object.m_selectedSkill.CoolDown = m_object.m_selectedSkill.CoolTime;
                m_object.m_isSkillSelected = false;
                m_animator.speed = 1;
                m_animator.CrossFade("Idle", 0.1f);
            }
        }
        return AI.State.RUNNING;
    }
}