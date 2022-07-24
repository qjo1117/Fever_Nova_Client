using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_PlayerHPBar : UI_SceneHPBar
{
    private void Update()
    {
        // 플레이어의 최대 체력증가 (test용)
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            Managers.Game.Player.MainPlayer.Stat.maxHp += 100;
            Debug.Log($"current maxhp : {m_maxHp}");
            GetHpBoost();
        }
    }

}
