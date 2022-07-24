using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BombDropPoint : UI_Scene
{
    #region 변수
    private Camera              m_mainCam;

    
    private UI_BombRange        m_bombRange;        // 폭탄 최대 사거리 표시 UI (착탄지점 UI가 폭탄 최대 사거리를 벗어나지 못하도록 하기 위해 필요)
    private UI_BombJumpRange    m_bombJumpRange;    // 폭탄 점프 범위 표시 UI (폭탄점프 화살표 UI표시 위해 필요)
    #endregion

    #region 프로퍼티
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

    // 마우스포인터로 위치 업데이트
    private void PositionUpdate()
    {
        RaycastHit l_hit;
        Ray l_ray = m_mainCam.ScreenPointToRay(Input.mousePosition);

        // ray를 쏴봐서 마우스 위치의 월드 위치값을 구함
        if (Physics.Raycast(l_ray, out l_hit, 100f))
        {
            // 현재 마우스 위치 폭탄 사거리 범위안인지 체크
            if(BombRangeInnerCheck(l_hit.point))
            {
                transform.position = l_hit.point;
            }

            // 현재 마우스 위치 폭탄 점프 사거리 범위내인지 체크
            m_bombJumpRange.BombJumpRangeInnerCheck(l_hit.point);
        }
    }

    // 폭탄 사거리 범위내인지 체크하는 함수
    private bool BombRangeInnerCheck(Vector3 _point)
    {
        Vector3 l_subVector = _point - m_bombRange.transform.position;                  // 플레이어 위치와 마우스 위치 사이 Vector값
        Vector3 l_maxRangeVector = l_subVector.normalized * m_bombRange.RangeRadius;    // 폭탄 최대 사거리 Vector값

        // 현재 마우스 위치가 폭탄 최대 사거리를 벗어날떄
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
