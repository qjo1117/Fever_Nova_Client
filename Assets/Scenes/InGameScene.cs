using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{
    private CameraController m_camera = null;

    protected override void LoadGameObject()
    {
        Managers.Resource.RegisterPoolGameObject(Path.Bomb, 20);
        Managers.Resource.RegisterPoolGameObject(Path.Bomb_Particle, 20);
        Managers.Resource.RegisterPoolGameObject(Path.Monster, 30);
        Managers.Resource.RegisterPoolGameObject(Path.Player, 5);

        int l_count = 2;
        Managers.Resource.RegisterPoolGameObject(Path.UI_PopupMsg, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_Aim, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BombRange, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BombJumpRange, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BombJumpArrow, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BombDropPoint, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_BossMonsterHPBar, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_PlayerHPBar, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_Goal, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_Score, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_Pause, l_count);

        Managers.Resource.RegisterPoolGameObject(Path.UI_MonsterHPBar, 100);

        Managers.Resource.RegisterPoolGameObject(Path.UI_MonsterHPBar, 100);
        Managers.Resource.RegisterPoolGameObject(Path.UI_PauseScreen, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_ResultScreen, l_count);
        Managers.Resource.RegisterPoolGameObject(Path.UI_Result, l_count);

        Managers.Resource.RegisterPoolGameObject(Path.Health, 20);
    }

	protected override void Init()
	{
        InitUI();
        Managers.Game.StartGame();

        // Init Camera
        m_camera = GameObject.FindObjectOfType<CameraController>();
        m_camera.SetPlayer(Managers.Game.Player.Spawn(Managers.Game.Player.SpanwPoint, new PlayerStat { id = 0, name = "Sample_Player" }).gameObject);
    }

    private void InitUI()
    {
        UI_PopupMsg l_popupMsg = Managers.UI.ShowPopupUI<UI_PopupMsg>("UI_PopupMsg");
        l_popupMsg.Message = "�� ����";
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



    protected override void OnUpdate()
	{
		if(Input.GetKeyDown(KeyCode.V) == true) {
            Managers.Input.ChangeKey(Define.UserKey.Forward, KeyCode.UpArrow);
        }
	}

	public override void Clear()
    {

        Managers.Log("InGame Clear");
    }

}
