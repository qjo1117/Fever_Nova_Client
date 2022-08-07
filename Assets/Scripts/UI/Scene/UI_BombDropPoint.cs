using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BombDropPoint : UI_Scene
{
    #region ����
    private Camera              m_mainCam = null;
    private UI_BombRange        m_bombRange = null;        // ��ź �ִ� ��Ÿ� ǥ�� UI (��ź���� UI�� ��ź �ִ� ��Ÿ��� ����� ���ϵ��� �ϱ� ���� �ʿ�)
    private UI_BombJumpRange    m_bombJumpRange = null;    // ��ź ���� ���� ǥ�� UI (��ź���� ȭ��ǥ UIǥ�� ���� �ʿ�)
    #endregion

    #region ������Ƽ
    public UI_BombRange BombRange { get => m_bombRange; set => m_bombRange = value; }
    public UI_BombJumpRange BombJumpRange { get => m_bombJumpRange; set => m_bombJumpRange = value; }
    #endregion

    public override void Init()
    {
        m_mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        PositionUpdate();
    }

    // ���콺�����ͷ� ��ġ ������Ʈ
    private void PositionUpdate()
    {
        RaycastHit l_hit;
        Ray l_ray = m_mainCam.ScreenPointToRay(Input.mousePosition);

        // ray�� ������ ���콺 ��ġ�� ���� ��ġ���� ����
        if (Physics.Raycast(l_ray, out l_hit, 100.0f, 1 << (int)Define.Layer.Ground)) {
            // ���� ���콺 ��ġ ��ź ��Ÿ� ���������� üũ
            if(BombRangeInnerCheck(l_hit.point)) {
                transform.position = l_hit.point;
            }

            // ���� ���콺 ��ġ ��ź ���� ��Ÿ� ���������� üũ
            if(m_bombJumpRange != null) {
                m_bombJumpRange.BombJumpRangeInnerCheck(l_hit.point);
            }
            
        }
    }

    // ��ź ��Ÿ� ���������� üũ�ϴ� �Լ�
    private bool BombRangeInnerCheck(Vector3 _point)
    {
        Vector3 l_subVector = _point - m_bombRange.transform.position;                  // �÷��̾� ��ġ�� ���콺 ��ġ ���� Vector��
        Vector3 l_maxRangeVector = l_subVector.normalized * m_bombRange.RangeRadius;    // ��ź �ִ� ��Ÿ� Vector��

        // ���� ���콺 ��ġ�� ��ź �ִ� ��Ÿ��� �����
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
