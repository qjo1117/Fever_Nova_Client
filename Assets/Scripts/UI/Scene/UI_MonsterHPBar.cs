using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterHPBar : UI_HPBar
{

    [SerializeField]
    private Camera m_mainCamera;
    private MonsterController m_target;
    private bool m_isReady;

    public int m_verticalOffset;

    public MonsterController Target { get => m_target; set => m_target = value; }


    public override void Init()
    {
        base.Init();

        if(m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }

        HpBarUpdate();

        m_isReady = true;
    }

    public void SetHpBarPosition()
    {
        if (!m_isReady)
        {
            return;
        }

        Vector3 l_screenPos = m_mainCamera.WorldToScreenPoint(m_target.transform.position);
        RectTransform l_sliderRectTrans = Get<Image>((int)Images.hpBarFrame).GetComponent<RectTransform>();
        l_sliderRectTrans.position = new Vector2(l_screenPos.x, l_screenPos.y + m_verticalOffset);
    }

    public void HpBarUpdate()
    {
        Get<Image>((int)Images.hpBar).fillAmount = m_target.Stat.Hp / (float)m_target.Stat.MaxHp;
    }
}
