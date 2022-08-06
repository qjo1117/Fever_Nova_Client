using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_AI_Condition_Patrol : BT_Condition
{
    AI.EnemyState current_state;

    public General_AI_Condition_Patrol(AI.EnemyState p_state)
    {
        current_state = p_state;
    }

    public override State Update()
    {
        if(current_state !=EnemyState.Recon)
        {
            return AI.State.FAILURE;
        }
        return State.SUCCESS;
    }
}
