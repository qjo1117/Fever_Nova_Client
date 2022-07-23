using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BombJumpArrow : UI_Scene
{
    #region UI컴포넌트_ENUM
    enum Images
    {
        Arrow                                       // 화살표 이미지
    }
    #endregion

    #region 변수
    private bool                m_isReady = false;  // Init함수 실행 여부
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));

        m_isReady = true;
    }

     
    // 폭탄점프 화살표 UI 위치 Set
    public void SetPosition(Vector2 _position)
    {
        if (!m_isReady)
        {
            return;
        }

        Get<Image>((int)Images.Arrow).GetComponent<RectTransform>().position = _position;
    }
}
