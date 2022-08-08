using System.Collections.Generic;
using UnityEngine;

public class AI_Enemy_01 : AI_Enemy
{
    public float Detect_Range = 10.0f;
    public float Alarm_Range = 10.0f;
    public float Chase_MoveSpeed = 10.0f;
    public float Patrol_MoveSpeed = 10.0f;

    private List<Vector3> Patrol_WaypointList = new List<Vector3>();
    public Material LineRenderMat;
    public Material WayPointMat;

    protected override void CreateBehaviorTreeAIState()
    {
        // 현재 구조는 대략적으로 이렇다고 한다.
        //                                    Main (Selecter)
        //                Combat (Sequence)                    Patrol (Sequence)        
        //      Detect (Action)      Chase (Action)             Patrol (Action)      

        m_enemyType = AI.EnemyType.Melee;
        m_brain = new BT_Root();

        BT_Selector l_mainSelector = new BT_Selector();

        BT_Sequence l_combatSQ = new BT_Sequence();

        AI_Combat_Detect l_detect
            = new AI_Combat_Detect(gameObject, Detect_Range);

        AI_Combat_Chase l_chase
            = new AI_Combat_Chase(gameObject, Chase_MoveSpeed);

        l_combatSQ.AddChild(l_detect);
        l_combatSQ.AddChild(l_chase);

        BT_Sequence l_patrolSQ = new BT_Sequence();

        AI_Patrol_Waypoint l_patrol
            = new AI_Patrol_Waypoint(gameObject, Patrol_MoveSpeed, Patrol_WaypointList);

        l_patrolSQ.AddChild(l_patrol);

        l_mainSelector.AddChild(l_combatSQ);
        l_mainSelector.AddChild(l_patrolSQ);

        m_brain.Child = l_mainSelector;
    }

    public override void AddPatrolPoint(Vector3 _position)
    {
        Patrol_WaypointList.Add(_position);
    }
}
