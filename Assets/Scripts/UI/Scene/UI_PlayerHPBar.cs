using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHPBar : UI_HPBar
{
    public override void Init()
    {
        base.Init();
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

    public void HpBarUpdate()
    {
        PlayerController l_player = Managers.Game.Player.MainPlayer;
        Get<Image>((int)Images.hpBar).fillAmount = l_player.Stat.hp / (float)l_player.Stat.maxHp;
    }
}
