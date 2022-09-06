using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    protected override void LoadGameObject()
    {
        Managers.Resource.RegisterPoolGameObject(Path.UI_MainMenu);
        Managers.Resource.RegisterPoolGameObject(Path.UI_Option);
        Managers.Resource.RegisterPoolGameObject(Path.UI_KeyInput);
        Managers.Resource.RegisterPoolGameObject(Path.UI_GamePlaySelect);
        Managers.Resource.RegisterPoolGameObject(Path.UI_StageSelect);

    }

    protected override void Init()
    {
        Managers.UI.ShowSceneUI<UI_MainMenu>("UI_MainMenu");

        Managers.Sound.Play(Path.Main_BGM, Define.Sound.Bgm);
    }

    public override void Clear()
    {
        Managers.Log("Clear");
    }
}
