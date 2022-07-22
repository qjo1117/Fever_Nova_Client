using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterHPBar : UI_Base
{

    [SerializeField]
    private Camera m_mainCamera;
    private MonsterController m_target;

    public int m_verticalOffset;

    public MonsterController Target { get => m_target; set => m_target = value; }


    protected enum Images
    {
        hpBarFrame,
        hpBar
    }

    protected enum GameObjects
    {
        hpLine
    }

    public int m_unitHp;

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        GetHpBoost();

        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }

        HpBarUpdate();
        CanvasScaleChange();
        
    }

    private void Update()
    {
        HpBarPositionUpdate();
    }

    public void GetHpBoost()
    {
        PlayerController l_player = Managers.Game.Player.MainPlayer;
        float l_scaleX = (float)m_unitHp / l_player.Stat.maxHp;

        GameObject l_hpLine = Get<GameObject>((int)GameObjects.hpLine);
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in l_hpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(l_scaleX, 1, 1);
        }
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }

    private void HpBarPositionUpdate()
    {

        RectTransform l_canvas = GetComponent<RectTransform>();
        Collider l_parentCollider = transform.parent.GetComponent<Collider>();

        l_canvas.position = transform.parent.position + Vector3.up * (l_parentCollider.bounds.size.y + m_verticalOffset);
        transform.LookAt(m_mainCamera.transform);
        transform.Rotate(Vector3.up, -180.0f);
    }

    public void HpBarUpdate()
    {
        Get<Image>((int)Images.hpBar).fillAmount = m_target.Stat.Hp / (float)m_target.Stat.MaxHp;
    }

    private void CanvasScaleChange()
    {
        RectTransform l_hpbarRect = Get<Image>((int)Images.hpBarFrame).GetComponent<RectTransform>();
        Vector2 l_newCanvasSize = l_hpbarRect.sizeDelta * l_hpbarRect.localScale.x;

        GetComponent<RectTransform>().sizeDelta = l_newCanvasSize;
    }
}
