using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 해당 영상 참조하여 제작
// https://www.youtube.com/watch?v=zt_2eSM582c&ab_channel=%EC%84%A4%ED%9B%84%EA%B0%9C%EC%9D%98GameDev.

public class UI_HPBar : UI_Scene
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
}
