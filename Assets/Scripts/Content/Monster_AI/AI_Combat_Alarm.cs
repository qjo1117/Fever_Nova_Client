using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Combat_Alarm : BT_Action
{
    private GameObject m_object;
    private float m_alarmRange;

    private AI_Enemy m_root = null;

    public AI_Combat_Alarm(GameObject _object, float _alarmRange)
    {
        m_object = _object;
        m_alarmRange = _alarmRange;
        m_root = _object.GetComponent<AI_Enemy>();
    }

    public override void Initialize() { }

    public override void Terminate() { }

    public override AI.State Update()
    {
        OnAlarm();
        return AI.State.RUNNING;
    }

    private void OnAlarm()
    {
        //monster list �����ͼ� distance ����
        //���� �� �Ÿ� ���� �ִ� ��� ������ chase ���� ���Խ�Ŵ

    }
}
