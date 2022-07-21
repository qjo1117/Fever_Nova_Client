using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BombDropPoint : UI_Scene
{
    private Camera              m_mainCam;
    private Ray                 m_testRay;
    private UI_BombRange        m_bombRange;


    public UI_BombRange BombRange { get => m_bombRange; set => m_bombRange = value; }

    public override void Init()
    {
        m_mainCam = Camera.main;
    }

    private void Update()
    {
        PositionUpdate();
    }

    private void PositionUpdate()
    {
        //raycast »ç¿ë
        RaycastHit l_hit;
        m_testRay = m_mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(m_testRay, out l_hit, 100f))
        {
            //Debug.Log($"hit x {hit.point.x}, y {hit.point.y} , z {hit.point.z}");
            if(BombRangeInnerCheck(l_hit.point))
            {
                transform.position = l_hit.point;
            }
        }
    }

    private bool BombRangeInnerCheck(Vector3 _point)
    {
        Vector3 l_subVector = _point - m_bombRange.transform.position;
        Vector3 l_maxRangeVector = l_subVector.normalized * m_bombRange.RangeRadius;

        if(l_subVector.sqrMagnitude > l_maxRangeVector.sqrMagnitude)
        {
            transform.position = m_bombRange.transform.position + l_maxRangeVector;
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
