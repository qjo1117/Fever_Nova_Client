using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_AI_Action_Die : BT_Action
{
    private GameObject m_object;

    public General_AI_Action_Die(GameObject p_object)
    {
        m_object = p_object;
    }
    public override State Update()
    {
        //Destroy(m_object);
        //���ӿ�����Ʈ �ı� Ȥ�� Ǯ��
        return State.RUNNING;
    }
}
