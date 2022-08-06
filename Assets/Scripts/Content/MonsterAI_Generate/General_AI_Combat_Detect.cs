using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_AI_Combat_Detect : BT_Condition
{
    private GameObject m_object;
    private float m_detectRange;

    public General_AI_Combat_Detect(GameObject _object, float _detectRange)
    {
        m_object = _object;
        m_detectRange = _detectRange;
    }
    public override AI.State Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            float Distance = Vector3.Distance(player.transform.position, m_object.transform.position);
            if (Distance < m_detectRange)
            {
                return AI.State.SUCCESS;
            }
        }
        return AI.State.FAILURE;
    }
}
