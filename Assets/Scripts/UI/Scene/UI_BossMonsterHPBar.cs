using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossMonsterHPBar : UI_Scene
{

    [SerializeField]
    private Camera m_mainCamera;
    private MonsterController m_target;

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

    public void HpBarUpdate()
    {
        Get<Image>((int)Images.hpBar).fillAmount = m_target.Stat.Hp / (float)m_target.Stat.MaxHp;
    }
}
