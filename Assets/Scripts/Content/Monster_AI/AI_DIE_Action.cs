using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DIE_Action : BT_Action
{
    private GameObject m_object;

    public AI_DIE_Action(GameObject p_object)
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
