using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_AI:MonoBehaviour
{
    //스탯(변경예정)
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
    //AI의 뇌가 될 루트
    protected BT_Root m_brain;
    //적의 타입
    [SerializeField]
    protected AI.EnemyType enemy_type;
    //적의 현재 상태
    [SerializeField]
    protected AI.EnemyState enemy_state;

    //트리 만들기
    protected virtual void CreateTree()
    {

    }

   
    
}
