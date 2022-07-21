using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopupMsg : UI_Popup
{
    enum Texts
    {
        PopupMsg
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
    }

}
