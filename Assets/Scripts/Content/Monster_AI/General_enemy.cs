using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_enemy : Object_AI
{
    

    public void Awake()
    {
        //임시적으로 초기화
        this.m_brain = new BT_Root();
        this.enemy_type = AI.EnemyType.Melee;
        this.enemy_state = AI.EnemyState.Recon;
        CreateTree();
    }
    protected override void CreateTree()
    {
        BT_Selector bT_Selector = new BT_Selector();
       
        BT_Sequence bT_DieCheck_Sequence = new BT_Sequence();
        Condition_DIe condition_DIe = new Condition_DIe(this.gameObject, m_stat.Hp);
        AI_DIE_Action aI_DIE_Action = new AI_DIE_Action(this.gameObject);
        bT_DieCheck_Sequence.AddChild(condition_DIe);
        bT_DieCheck_Sequence.AddChild(aI_DIE_Action);


        BT_Selector bT_MonsterBehavior = new BT_Selector();

        BT_Sequence bT_Attack_Sequence = new BT_Sequence();
        Attack_Condition attack_Condition = new Attack_Condition(this.gameObject,this.attack_range);
        AI_Combat_attack aI_Combat_Attack = new AI_Combat_attack(this.gameObject, this.skill_cooltime, this.gameObject.transform);
        bT_Attack_Sequence.AddChild(attack_Condition);
        bT_Attack_Sequence.AddChild(aI_Combat_Attack);

        //행동에 관한 시퀀스의 큰 틀
        BT_Sequence bT_Move_Sequence = new BT_Sequence();
        //알람 추적등을 실행할 시퀀스
        BT_Sequence bT_Move_Sequence2 = new BT_Sequence();
        AI_Combat_Detect aI_Combat_Detect = new AI_Combat_Detect(this.gameObject, this.detect_range);
        
        
        General_AI_Combat_Alarm aI_Combat_Alarm = new General_AI_Combat_Alarm(this.gameObject, this.alarm_range);
        AI_Combat_Chase aI_Combat_Chase = new AI_Combat_Chase(this.gameObject, m_stat.MoveSpeed);

        //시퀀스에 알람을 부착
        bT_Move_Sequence2.AddChild(aI_Combat_Alarm);
        //추적노드를 부착
        bT_Move_Sequence2.AddChild(aI_Combat_Chase);
        //감시 노드를 부착
        bT_Move_Sequence.AddChild(aI_Combat_Detect);
        //알람 추적에 관한 시퀀스를 부착
        bT_Move_Sequence.AddChild(bT_Move_Sequence2);

        BT_Sequence bT_Patrol_Sequence = new BT_Sequence();
        Patrol_Condition patrol_Condition = new Patrol_Condition(this.enemy_state);
        General_AI_Patrol_Waypoint aI_Patrol_Waypoint = new General_AI_Patrol_Waypoint(this.gameObject, m_stat.MoveSpeed);

        bT_Patrol_Sequence.AddChild(patrol_Condition);
        bT_Patrol_Sequence.AddChild(aI_Patrol_Waypoint);

        bT_MonsterBehavior.AddChild(bT_Attack_Sequence);
        bT_MonsterBehavior.AddChild(bT_Move_Sequence);
        bT_Move_Sequence.AddChild(bT_Patrol_Sequence);

        bT_Selector.AddChild(bT_DieCheck_Sequence);
        bT_Selector.AddChild(bT_MonsterBehavior);

        this.m_brain.Child = bT_Selector;
    }

    public void Update()
    {
        //트리를 틱으로 돌린다.
        this.m_brain.Tick();
    }
}
