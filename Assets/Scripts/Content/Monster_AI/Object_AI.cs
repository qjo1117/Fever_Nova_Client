using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_AI:MonoBehaviour
{
    //����(���濹��)
    [SerializeField]
    protected float alarm_range;
    [SerializeField]
    protected float detect_range;
    [SerializeField]
    protected float skill_cooltime;
    [SerializeField]
    protected float attack_range;
    [SerializeField]
    protected MonsterStat m_stat;
    [SerializeField]
    protected UI_MonsterHPBar m_hpbar;
    //AI�� ���� �� ��Ʈ
    protected BT_Root m_brain;
    //���� Ÿ��
    [SerializeField]
    protected AI.EnemyType enemy_type;
    //���� ���� ����
    [SerializeField]
    protected AI.EnemyState enemy_state;

    //Ʈ�� �����
    protected virtual void CreateTree()
    {

    }

   
    
}
