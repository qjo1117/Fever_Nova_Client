using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Skill : BT_Action
{
    protected int m_id;            //��ų ID
    protected float m_coolTime;    //��ü ��Ÿ��
    protected float m_coolDown;    //���� ��ٿ�
    protected float m_range;       //��ų ��Ÿ� - ��Ÿ� ���°Ÿ� -1������
    protected int m_priority;      //��ų �켱��

    public int ID => m_id;
    public float CoolTime => m_coolTime;
    public float CoolDown { get => m_coolDown; set => m_coolDown = value; }

    public float Range => m_range;
    public int Priority => m_priority;
}