using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_IsSkillRuning : BT_Condition
{
    private Interface_Enemy m_object;
    public Condition_IsSkillRuning(GameObject _object)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
    }

    public override AI.State Update()
    {
        return IsSkillRuning();
    }

    private AI.State IsSkillRuning()
    {
        if (m_object.m_isSkillSelected)
        {
            return AI.State.SUCCESS;
        }

        return AI.State.FAILURE;
    }
}
