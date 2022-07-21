using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Score : UI_Scene
{
    enum Texts
    {
        CurrentScoreText,
        ScoreLogText
    }

    enum Images
    {
        Background
    }


    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

}
