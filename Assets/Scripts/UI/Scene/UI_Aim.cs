using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Aim : UI_Scene
{
    private Camera m_mainCam;
    private Ray m_testRay;

    public override void Init()
    {
        m_mainCam = Camera.main;

        Material l_material = GetComponent<MeshRenderer>().material;
        l_material.color = Color.red;
    }

    private void Update()
    {
        PositionUpdate();
    }

    private void PositionUpdate()
    {
        //raycast »ç¿ë
        RaycastHit hit;
        m_testRay = m_mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(m_testRay, out hit, 100f))
        {
            if(transform.position!= hit.point)
            {
                transform.position = hit.point;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
