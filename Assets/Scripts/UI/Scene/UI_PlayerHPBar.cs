using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHPBar : UI_Scene
{
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
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        GetHpBoost();
        HpBarUpdate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            Managers.Game.Player.MainPlayer.Stat.maxHp += 100;
            Debug.Log($"current maxhp : {Managers.Game.Player.MainPlayer.Stat.maxHp}");
            GetHpBoost();
        }
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
        PlayerController l_player = Managers.Game.Player.MainPlayer;
        Get<Image>((int)Images.hpBar).fillAmount = l_player.Stat.hp / (float)l_player.Stat.maxHp;
    }
}
