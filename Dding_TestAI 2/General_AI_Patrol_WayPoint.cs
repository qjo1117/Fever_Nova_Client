using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_AI_Patrol_WayPoint : BT_Action
{
    private GameObject m_object;
    private float m_moveSpeed;
    private List<Vector3> waypointList = new List<Vector3>();

    private int currentWayPoint = 0;
    private float wayPointRadius = 0.25f;

    public General_AI_Patrol_WayPoint(GameObject _object, float _moveSpeed)
    {
        m_object = _object;
        m_moveSpeed = _moveSpeed;
    }

    public override void Initialize()
    {
        SetWayPoint();
        SetStateColor();
    }

    private void SetStateColor()
    {
        m_object.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    private void SetWayPoint()
    {
        if(waypointList==null)
        {
            GameObject[] Point = GameObject.FindGameObjectsWithTag("Point");
            for(int i=0;i<2;i++)
            {
                waypointList[i] = Point[Random.Range(0, Point.Length)].transform.position;
            }
            //범위에 있는 물체를 탐색?(본프로젝트 보고 작업)
            //오브젝트 중에서 2개를 선정해 위치를 지정
        }
    }

    public override void Terminate() { }

    public override AI.State Update()
    {
        OnMove();
        return AI.State.RUNNING;
    }
    private void OnMove()
    {
        Vector3 WayPoint = waypointList[currentWayPoint];

        float Distance = Vector3.Distance(WayPoint, m_object.transform.position);
        if (Distance < wayPointRadius)
        {
            if (++currentWayPoint >= waypointList.Count)
            {
                currentWayPoint = 0;
            }
            WayPoint = waypointList[currentWayPoint];
        }
        Vector3 Dir = WayPoint - m_object.transform.position;
        m_object.transform.rotation = Quaternion.Slerp
            (m_object.transform.rotation,
            Quaternion.LookRotation(Dir),
            Time.deltaTime * 8);

        m_object.transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);

    }
}
