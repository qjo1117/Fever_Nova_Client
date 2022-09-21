using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Aim : UI_Scene
{
    #region ����
    private Camera m_mainCam;
    private UI_BombJumpRange m_bombJumpRange;
    #endregion

    public override void Init()
    {
        m_mainCam = Camera.main;

        // ũ�ν���� ���� ����
        Material l_material = GetComponent<MeshRenderer>().material;
    }

    private void LateUpdate()
    {
        PositionUpdate();
    }

    // ���콺�����ͷ� ��ġ ������Ʈ
    private void PositionUpdate()
    {
        RaycastHit hit;

        // ���콺 ��ġ�� ray�� ���� ���� ��ġ�� ����
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
