using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{

    protected override void LoadGameObject()
    {
        Managers.Resource.RegisterPoolGameObject(Path.Boom);
        Managers.Resource.RegisterPoolGameObject(Path.Boom_Particle);
        Managers.Resource.RegisterPoolGameObject(Path.Monster);
        Managers.Resource.RegisterPoolGameObject(Path.Player);

        Managers.Resource.RegisterPoolGameObject(Path.UI_PopupMsg);

        Managers.Resource.RegisterPoolGameObject(Path.UI_Aim);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BombRange);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BombJumpRange);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BombJumpArrow);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BombDropPoint);
        Managers.Resource.RegisterPoolGameObject(Path.UI_MonsterHPBar);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BossMonsterHPBar);
        Managers.Resource.RegisterPoolGameObject(Path.UI_PlayerHPBar);
        Managers.Resource.RegisterPoolGameObject(Path.UI_Goal);
        Managers.Resource.RegisterPoolGameObject(Path.UI_Score);
        Managers.Resource.RegisterPoolGameObject(Path.UI_Pause);
    }

	protected override void Init()
	{
        UIInit();

        Managers.Game.InGameInit();

        Managers.Game.Player.Spawn(Vector3.zero, new PlayerStat { id = 0, name = "Sample_Player" });
        Managers.Resource.Instantiate(Path.Monster, Managers.Game.Monster.transform);
        Managers.Resource.Instantiate(Path.Monster, Managers.Game.Monster.transform);
        Managers.Resource.Instantiate(Path.Monster, Managers.Game.Monster.transform);

    }

    private void UIInit()
    {
        UI_PopupMsg l_popupMsg = Managers.UI.ShowPopupUI<UI_PopupMsg>("UI_PopupMsg");
        l_popupMsg.Message = "Àû ÃâÇö";
        l_popupMsg.DelayDeleteTime = 3.0f;

        Managers.UI.ShowSceneUI<UI_Aim>("UI_Aim");

        Managers.UI.ShowSceneUI<UI_Goal>("UI_Goal");

        Managers.UI.ShowSceneUI<UI_Score>("UI_Score");
        Managers.UI.ShowSceneUI<UI_Pause>("UI_Pause");


        UI_BombRange l_bombRange = Managers.UI.ShowSceneUI<UI_BombRange>("UI_BombRange");
        l_bombRange.RangeRadius = 5.0f; 

        UI_BombJumpRange l_bombJumpRange = Managers.UI.ShowSceneUI<UI_BombJumpRange>("UI_BombJumpRange");
        l_bombJumpRange.RangeRadius = 2.0f;


        UI_BombDropPoint l_dropPoint = Managers.UI.ShowSceneUI<UI_BombDropPoint>("UI_BombDropPoint");
        l_dropPoint.BombRange = l_bombRange;
        l_dropPoint.BombJumpRange = l_bombJumpRange;
    }


    public override void Clear()
    {

        Managers.Log("InGame Clear");
    }
}
