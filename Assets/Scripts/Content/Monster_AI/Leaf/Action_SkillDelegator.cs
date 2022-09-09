using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_SkillDelegator : BT_Action
{
    public Action_SkillDelegator(GameObject _object)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<MyAnimator>();
    }

    protected override AI.State Function()
    {
        if (m_object.m_selectedSkill != null)
        {
            if(!m_object.m_isPlayingChaseAnimation && m_animator.GetBool(AI.Enemy_AniParametar.MoveFlag))
            {
                m_animator.SetBool(AI.Enemy_AniParametar.MoveFlag, false);
            }

            if (m_object.m_selectedSkill.Tick() == AI.State.SUCCESS)
            {
                m_object.m_selectedSkill.CoolDown = m_object.m_selectedSkill.CoolTime;
                m_object.m_isSkillSelected = false;
                m_animator.AnimationSpeedReset();

                // go to idle
                m_animator.FlagClear();
            }
        }
        return AI.State.RUNNING;
    }
}