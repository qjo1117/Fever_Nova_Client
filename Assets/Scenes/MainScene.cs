using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    protected override void LoadGameObject()
    {
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_MainMenu");
        Managers.Resource.RegisterPoolGameObject("UI/Popup/UI_Option");
        Managers.Resource.RegisterPoolGameObject("UI/Popup/UI_KeyInput");
    }

    protected override void Init()
    {
        Managers.UI.ShowSceneUI<UI_MainMenu>("UI_MainMenu");
        //Managers.UI.ShowPopupUI<UI_Option>("UI_Option");
        //Managers.UI.ShowPopupUI<UI_KeyInput>("UI_KeyInput");
    }

    public override void Clear()
    {
        Managers.Log("Clear");
    }
}
