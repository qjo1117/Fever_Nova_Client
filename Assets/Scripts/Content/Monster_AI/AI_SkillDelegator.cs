using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SkillDelegator : BT_Action
{
    private bool m_isSkillSelected;
    private AI_Skill m_selectedSkill;

    public AI_SkillDelegator(GameObject _object, bool _isSkillSelected, AI_Skill _selectedSkill)
    {
        m_object = _object;
        m_isSkillSelected = _isSkillSelected;
        m_selectedSkill = _selectedSkill;
    }

    public override void Initialize()
    {

    }

    public override void Terminate() { }

    public override AI.State Update()
    {
        return UseSkill();
    }

    private AI.State UseSkill()
    {
        if (m_selectedSkill != null)
        {

            m_selectedSkill.CoolDown = m_selectedSkill.CoolTime;
            m_isSkillSelected = false;
        }

        return AI.State.RUNNING;
    }
}
