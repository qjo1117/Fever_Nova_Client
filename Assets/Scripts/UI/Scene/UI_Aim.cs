using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Aim : UI_Scene
{
    #region 변수
    private Camera m_mainCam;
    private UI_BombJumpRange m_bombJumpRange;
    #endregion

    public override void Init()
    {
        m_mainCam = Camera.main;

        // 크로스헤어 색깔 변경
        Material l_material = GetComponent<MeshRenderer>().material;
    }

    private void LateUpdate()
    {
        PositionUpdate();
    }

    // 마우스포인터로 위치 업데이트
    private void PositionUpdate()
    {
        RaycastHit hit;

        // 마우스 위치에 ray를 쏴서 월드 위치값 도출
        Ray l_Ray = m_mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(l_Ray, out hit, 100.0f, 1 << (int)Define.Layer.Ground))
        {
            transform.position = hit.point;
        }
    }

    public void ColorChange(Color _color)
    {
        GetComponent<MeshRenderer>().material.color = _color;
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
