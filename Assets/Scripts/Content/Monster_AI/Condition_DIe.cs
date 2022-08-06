using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_DIe : BT_Condition
{
    private GameObject m_object;
    private int hp;

    public Condition_DIe(GameObject p_object,int p_hp)
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
