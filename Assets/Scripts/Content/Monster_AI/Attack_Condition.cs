using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Condition : BT_Condition
{
    private GameObject m_object;
    private float m_attackRange;

    public Attack_Condition(GameObject _object, float attackrange)
    {
        m_object = _object;
        m_attackRange = attackrange;
    }
    public override void Initialize()
    {
        SetStateColor();
    }

    private void SetStateColor()
    {
        m_object.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public override void Terminate() { }

    public override AI.State Update()
    {
        return COnAttack();
    }

    private AI.State COnAttack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            float Distance = Vector3.Distance(player.transform.position, m_object.transform.position);
            if (Distance < m_attackRange)
            {
                return AI.State.SUCCESS;
            }
        }
        return AI.State.FAILURE;
    }
}
