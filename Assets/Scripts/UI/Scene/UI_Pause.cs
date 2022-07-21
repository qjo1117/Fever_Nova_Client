using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pause : UI_Scene
{
    enum Buttons
    {
        PauseButton
    }


    enum Texts
    {
        PauseText
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
    }
}
