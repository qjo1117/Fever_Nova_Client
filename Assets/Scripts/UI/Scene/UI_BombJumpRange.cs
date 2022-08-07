using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BombJumpRange : UI_BombRange
{
    #region 변수
    private Camera           m_mainCam = null;
    private UI_BombJumpArrow m_bombJumpArrow = null;           // 폭탄 점프 화살표 UI (폭탄 점프 화살표 UI 표시위해 필요함)
    #endregion

    public override void Init()
    {
        base.Init();
        m_mainCam = Camera.main;
    }


    // 마우스 위치 폭탄점프 사거리 안인지 체크
    public bool BombJumpRangeInnerCheck(Vector3 _point)
    {
        Vector3 l_subVector = _point - transform.position;                      // 플레이어와 마우스 위치간의 거리 Vector
        Vector3 l_maxRangeVector = l_subVector.normalized * m_rangeRadius;      // 폭탄 점프 최대 사거리 Vector

        l_subVector.y = 0.0f;

        // 현재 마우스의 위치가 폭탄점프 사거리 밖인경우
        if (l_subVector.sqrMagnitude > m_rangeRadius * m_rangeRadius)
        {
            // 화살표 이미지가 보여지고 있는경우 Close
            if (m_bombJumpArrow != null)
            {
                m_bombJumpArrow.CloseSceneUI();
                m_bombJumpArrow = null;
            }
            return false;
        }
        else
        {
            // 화살표 이미지가 표시되지 않는경우 Show하고, 마우스포인터 위치 Vector + 임의의 Vector값 (offset) 위치로 Position 조정
            if (m_bombJumpArrow == null)
            {
                m_bombJumpArrow = Managers.UI.ShowSceneUI<UI_BombJumpArrow>("UI_BombJumpArrow");
            }

            m_bombJumpArrow.SetPosition(m_mainCam.WorldToScreenPoint(_point + new Vector3(1.0f,1.0f,1.0f)));
            return true;
        }
    }
}
