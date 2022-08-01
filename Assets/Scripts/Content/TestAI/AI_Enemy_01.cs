using System.Collections.Generic;
using UnityEngine;

public class AI_Enemy_01 : AI_Enemy
{
    public float Detect_Range;
    public float Alarm_Range;
    public float Chase_MoveSpeed;
    public float Patrol_MoveSpeed;

    private List<Vector3> Patrol_WaypointList = new List<Vector3>();
    public Material LineRenderMat;
    public Material WayPointMat;

    public void Debug()
    {
        string l_detectRangeName = "Detect Range";
        //if (transform.Find(l_detectRangeName))
        //{
        //    DestroyImmediate(transform.Find(l_detectRangeName).gameObject);
        //}

        Transform l_detectRange = new GameObject(l_detectRangeName).transform;
        l_detectRange.parent = transform;
        l_detectRange.gameObject.AddComponent<LineRenderer>();

        LineRenderer Line_DetectRange = l_detectRange.GetComponent<LineRenderer>();
        Line_DetectRange.useWorldSpace = false;
        Line_DetectRange.startColor = Color.red;
        Line_DetectRange.endColor = Color.red;
        Line_DetectRange.startWidth = 1;
        Line_DetectRange.endWidth = 1;
        Line_DetectRange.loop = true;
        Line_DetectRange.positionCount = 128;
        Line_DetectRange.material = LineRenderMat;

        float deltaTheta = (float)(2.0 * Mathf.PI) / 128;
        float theta = 0f;

        for (int i = 0; i < 128; i++)
        {
            float x = Detect_Range * Mathf.Cos(theta);
            float z = Detect_Range * Mathf.Sin(theta);
            Vector3 Pos = new Vector3(x, 0, z);
            Line_DetectRange.SetPosition(i, Pos);
            theta += deltaTheta;
        }

        Line_DetectRange.gameObject.transform.position = transform.position;
        Line_DetectRange.gameObject.transform.rotation = Quaternion.identity;


        string l_alarmRangeName = "Alarm Range";
        //if (transform.Find(l_alarmRangeName))
        //{
        //    DestroyImmediate(transform.Find(l_alarmRangeName).gameObject);
        //}

        Transform l_alarmRange = new GameObject(l_alarmRangeName).transform;
        l_alarmRange.parent = transform;
        l_alarmRange.gameObject.AddComponent<LineRenderer>();

        LineRenderer Line_AlarmRange = l_alarmRange.GetComponent<LineRenderer>();
        Line_AlarmRange.useWorldSpace = false;
        Line_AlarmRange.startColor = Color.yellow;
        Line_AlarmRange.endColor = Color.yellow;
        Line_AlarmRange.startWidth = 1;
        Line_AlarmRange.endWidth = 1;
        Line_AlarmRange.loop = true;
        Line_AlarmRange.positionCount = 128;
        Line_AlarmRange.material = LineRenderMat;

        deltaTheta = (float)(2.0 * Mathf.PI) / 128;
        theta = 0f;

        for (int i = 0; i < 128; i++)
        {
            float x = Alarm_Range * Mathf.Cos(theta);
            float z = Alarm_Range * Mathf.Sin(theta);
            Vector3 Pos = new Vector3(x, 0, z);
            Line_AlarmRange.SetPosition(i, Pos);
            theta += deltaTheta;
        }

        Line_AlarmRange.gameObject.transform.position = transform.position;
        Line_AlarmRange.gameObject.transform.rotation = Quaternion.identity;


        string l_wayPointHolderName = "WayPoints";
        //if (transform.parent.Find(l_wayPointHolderName))
        //{
        //    DestroyImmediate(transform.parent.Find(l_wayPointHolderName).gameObject);
        //}

        Transform l_wayPointHolder = new GameObject(l_wayPointHolderName).transform;
        l_wayPointHolder.parent = transform.parent;

        for (int i = 0; i < Patrol_WaypointList.Count; i++)
        {
            GameObject Obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Obj.GetComponent<SphereCollider>().enabled = false;
            Obj.name = "Way Point " + (i + 1);
            Obj.transform.parent = l_wayPointHolder.transform;
            Obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Obj.transform.position = Patrol_WaypointList[i];
            Obj.GetComponent<MeshRenderer>().material = WayPointMat;


            Obj.AddComponent<LineRenderer>();
            LineRenderer Line_WayPoint = Obj.GetComponent<LineRenderer>();
            Line_WayPoint.useWorldSpace = true;
            Line_WayPoint.startColor = Color.cyan;
            Line_WayPoint.endColor = Color.cyan;
            Line_WayPoint.startWidth = 1f;
            Line_WayPoint.endWidth = 1f;
            Line_WayPoint.loop = false;
            Line_WayPoint.positionCount = 2;
            Line_WayPoint.material = LineRenderMat;

            Line_WayPoint.SetPosition(0, Obj.transform.position);

            if (i == Patrol_WaypointList.Count - 1)
            {
                Line_WayPoint.SetPosition(1, Patrol_WaypointList[0]);
            }
            else
            {
                Line_WayPoint.SetPosition(1, Patrol_WaypointList[i + 1]);
            }
        }
    }

    protected override void CreateBehaviorTreeAIState()
    {
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

        Debug();
    }

    public override void AddPatrolPoint(Vector3 _position)
    {
        Patrol_WaypointList.Add(_position);
    }
}
