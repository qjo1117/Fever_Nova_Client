using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BombJumpRange : UI_BombRange
{
    private Camera           m_mainCam;
    private UI_BombJumpArrow m_bombJumpArrow;


    public override void Init()
    {
        base.Init();
        m_mainCam = Camera.main;
    }


    public bool BombJumpRangeInnerCheck(Vector3 _point)
    {
        Vector3 l_subVector = _point - transform.position;
        Vector3 l_maxRangeVector = l_subVector.normalized * m_rangeRadius;

        if (l_subVector.sqrMagnitude > l_maxRangeVector.sqrMagnitude)
        {
            if (m_bombJumpArrow != null)
            {
                m_bombJumpArrow.CloseSceneUI();
                m_bombJumpArrow = null;
            }
            return false;
        }
        else
        {
            if (m_bombJumpArrow == null)
            {
                m_bombJumpArrow = Managers.UI.ShowSceneUI<UI_BombJumpArrow>("UI_BombJumpArrow");
            }

            m_bombJumpArrow.SetPosition(m_mainCam.WorldToScreenPoint(_point + new Vector3(1.0f,1.0f,1.0f)));
            return true;
        }
    }
}
