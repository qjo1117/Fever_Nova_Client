using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BombJumpArrow : UI_Scene
{
    #region UI������Ʈ_ENUM
    enum Images
    {
        Arrow                                       // ȭ��ǥ �̹���
    }
    #endregion

    #region ����
    private bool                m_isReady = false;  // Init�Լ� ���� ����
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));

        m_isReady = true;
    }

    
    // ��ź���� ȭ��ǥ UI ��ġ Set
    public void SetPosition(Vector2 _position)
    {
        if (!m_isReady)
        {
            return;
        }

        Get<Image>((int)Images.Arrow).GetComponent<RectTransform>().position = _position;
    }
}
