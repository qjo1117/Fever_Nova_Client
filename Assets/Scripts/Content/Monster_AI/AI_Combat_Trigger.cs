using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Combat_Trigger : BT_Condition
{
    private GameObject m_object;

    public AI_Combat_Trigger(GameObject _object)
    {
        m_object = _object;
    }

    public override AI.State Update()
    {
        if (m_object.GetComponent<AI_Enemy_Boss>().GetTrigger())
        {
            return AI.State.SUCCESS;
        }
        return AI.State.FAILURE;
    }
}