using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_AI:MonoBehaviour
{
    //스탯(변경예정)
    public uint hp;
    public float skill_cooltime;
    public float attack_range;
    public float detect_range;
    public float alarm_range;
    public float MoveSpeed;
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

    public void Awake()
    {
        //임시적으로 초기화
        m_brain = new BT_Root();
        enemy_type = AI.EnemyType.Melee;
        enemy_state = AI.EnemyState.Recon;
    }
    public void Update()
    {
        //트리를 틱으로 돌린다.
        m_brain.Tick();
    }
}
