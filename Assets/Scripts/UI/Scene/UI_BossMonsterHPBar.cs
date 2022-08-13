using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossMonsterHPBar : UI_MonsterHPBar
{
    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        m_isReady = true;


        GetHpBoost();
        HpBarUpdate();
    }
}
