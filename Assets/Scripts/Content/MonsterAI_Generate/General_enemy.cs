using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_enemy : Object_AI
{
    
    protected override void CreateTree()
    {
        BT_Selector bT_Selector = new BT_Selector();

        BT_Sequence bT_DieCheck_Sequence = new BT_Sequence();
        General_AI_Condition_Die condition_DIe = new General_AI_Condition_Die(this.gameObject, this.hp);
        General_AI_Action_Die aI_DIE_Action = new General_AI_Action_Die(this.gameObject);
        bT_DieCheck_Sequence.AddChild(condition_DIe);
        bT_DieCheck_Sequence.AddChild(aI_DIE_Action);

        BT_Selector bT_MonsterBehavior = new BT_Selector();

        BT_Sequence bT_Attack_Sequence = new BT_Sequence();
        General_AI_Condition_Attack attack_Condition = new General_AI_Condition_Attack(this.gameObject,this.attack_range);
        General_AI_Action_Attack aI_Combat_Attack = new General_AI_Action_Attack(this.gameObject, this.skill_cooltime, this.gameObject.transform);
        bT_Attack_Sequence.AddChild(attack_Condition);
        bT_Attack_Sequence.AddChild(aI_Combat_Attack);

        //행동에 관한 시퀀스의 큰 틀
        BT_Sequence bT_Move_Sequence = new BT_Sequence();
        //알람 추적등을 실행할 시퀀스
        BT_Sequence bT_Move_Sequence2 = new BT_Sequence();
        General_AI_Combat_Detect aI_Combat_Detect = new General_AI_Combat_Detect(this.gameObject, this.detect_range);


        General_AI_Combat_Alarm aI_Combat_Alarm = new General_AI_Combat_Alarm(this.gameObject, this.alarm_range);
        General_AI_Combat_Chase aI_Combat_Chase = new General_AI_Combat_Chase(this.gameObject, this.MoveSpeed);

        //시퀀스에 알람을 부착
        bT_Move_Sequence2.AddChild(aI_Combat_Alarm);
        //추적노드를 부착
        bT_Move_Sequence2.AddChild(aI_Combat_Chase);
        //감시 노드를 부착
        bT_Move_Sequence.AddChild(aI_Combat_Detect);
        //알람 추적에 관한 시퀀스를 부착
        bT_Move_Sequence.AddChild(bT_Move_Sequence2);

        BT_Sequence bT_Patrol_Sequence = new BT_Sequence();
        General_AI_Condition_Patrol patrol_Condition = new General_AI_Condition_Patrol(this.enemy_state);
        General_AI_Patrol_WayPoint aI_Patrol_Waypoint = new General_AI_Patrol_WayPoint(this.gameObject,this.MoveSpeed);

        bT_Patrol_Sequence.AddChild(patrol_Condition);
        bT_Patrol_Sequence.AddChild(aI_Patrol_Waypoint);

        bT_MonsterBehavior.AddChild(bT_Attack_Sequence);
        bT_MonsterBehavior.AddChild(bT_Move_Sequence);
        bT_Move_Sequence.AddChild(bT_Patrol_Sequence);

        bT_Selector.AddChild(bT_DieCheck_Sequence);
        bT_Selector.AddChild(bT_MonsterBehavior);

        this.m_brain.Child = bT_Selector;
    }
}
