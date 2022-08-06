using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_AI_Condition_Die : BT_Condition
{
    private GameObject m_object;
    private uint hp;

    public General_AI_Condition_Die(GameObject p_object,uint p_hp)
    {
        m_object = p_object;
        hp = p_hp;
    }
    public override AI.State Update()
    {
        if(hp != 0)
        {
            return AI.State.FAILURE;
        }
        return AI.State.SUCCESS;
    }
}
