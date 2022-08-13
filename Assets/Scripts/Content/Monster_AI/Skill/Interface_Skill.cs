using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface_Skill : BT_Action
{
    protected int m_id = -1;            //��ų ID
    protected float m_coolTime = -1;      //��ü ��Ÿ��
    protected float m_coolDown = -1;      //���� ��ٿ�
    protected float m_rangeForCastSkill = -1;         //��ų ��Ÿ� - ��Ÿ� ���°Ÿ� -1������
    protected int m_priority = -1;        //��ų �켱��

    public int ID => m_id;
    public float CoolTime => m_coolTime;
    public float CoolDown { get => m_coolDown; set => m_coolDown = value; }

    public float Range => m_rangeForCastSkill;
    public int Priority => m_priority;
}