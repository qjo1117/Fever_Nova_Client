using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_PlayerDetect : BT_Condition
{
    private GameObject m_object;
    private float m_detectRange;

    public Condition_PlayerDetect(GameObject _object, float _detectRange)
    {
        m_object = _object;
        m_detectRange = _detectRange;
    }

    public override AI.State Update()
    {

        foreach (PlayerController player in Managers.Game.Player.List)
        {
            float distance = Vector3.Distance(player.transform.position, m_object.transform.position);
            if (distance < m_detectRange)
            {
                return AI.State.SUCCESS;
            }
            
        }
        return AI.State.FAILURE;
    }
}
