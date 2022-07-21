using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterStat : UI_Popup
{
    enum Texts
    {
        StatText
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
    }
}
