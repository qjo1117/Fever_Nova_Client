using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// -- 몬스터 상태 표시 UI, 현재 사용되지 않음 --
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
