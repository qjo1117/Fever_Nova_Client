using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{

    protected override void LoadGameObject()
	{
        Managers.Resource.RegisterPoolGameObject("Boom");
        Managers.Resource.RegisterPoolGameObject("Monster");
        Managers.Resource.RegisterPoolGameObject("Asset/Prefabs/Player");

        Managers.Resource.RegisterPoolGameObject("UI/Popup/UI_PopupMsg");

        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_Aim");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_BombRange");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_BombJumpRange");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_BombDropPoint");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_MonsterHPBar");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_PlayerHPBar");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_Goal");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_Score");
        Managers.Resource.RegisterPoolGameObject("UI/Scene/UI_Pause");
    }

	protected override void Init()
	{
        Managers.Game.InGameInit();

        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform);
        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform);
        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform);

        UIInit();
    }

    private void UIInit()
    {
        Managers.UI.ShowPopupUI<UI_PopupMsg>("UI_PopupMsg");

        Managers.UI.ShowSceneUI<UI_Aim>("UI_Aim");

        Managers.UI.ShowSceneUI<UI_Goal>("UI_Goal");
        Managers.UI.ShowSceneUI<UI_Score>("UI_Score");
        Managers.UI.ShowSceneUI<UI_Pause>("UI_Pause");

        // ÇÃ·¹ÀÌ¾î ÆøÅº »ç°Å¸® »ý¼º
        UI_BombRange l_bombRange = Managers.UI.ShowSceneUI<UI_BombRange>("UI_BombRange");
        l_bombRange.RangeRadius = 5.0f; // ?

        UI_BombRange l_bombJumpRange = Managers.UI.ShowSceneUI<UI_BombRange>("UI_BombJumpRange");
        l_bombJumpRange.RangeRadius = 2.0f;

        // ÂøÅºÁöÁ¡ ui »ý¼º 
        UI_BombDropPoint l_dropPoint = Managers.UI.ShowSceneUI<UI_BombDropPoint>("UI_BombDropPoint");
        l_dropPoint.BombRange = l_bombRange;
    }


    public override void Clear()
    {

        Managers.Log("InGame Clear");
    }
}
