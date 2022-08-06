using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_AI_Combat_Alarm : BT_Action
{
    private GameObject m_object;
    private bool isalarm = true;
    private float alarm_range;

    public General_AI_Combat_Alarm(GameObject p_object,float p_alarm_range)
    {
        m_object = p_object;
        alarm_range = p_alarm_range;
    }
    public override State Update()
    {
        return OnAlarm();
    }

    public State OnAlarm()
    {
        if(isalarm)
        {
            //���� �׷��� �ҷ��ͼ�
            //�Ÿ��� ���
            //�װŸ��� �ִ� ���͸� ������Ʈ�� �ٲ۴�.
            return State.RUNNING;
        }
        return State.FAILURE;
    }
}
