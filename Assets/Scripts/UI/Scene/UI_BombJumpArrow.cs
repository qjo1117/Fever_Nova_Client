using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BombJumpArrow : UI_Scene
{
    enum Images
    {
        Arrow
    }

    private bool                m_isReady = false;

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));

        m_isReady = true;
    }

    public void SetPosition(Vector2 _position)
    {
        if (!m_isReady)
        {
            return;
        }

        Get<Image>((int)Images.Arrow).GetComponent<RectTransform>().position = _position;
    }
}
