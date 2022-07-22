using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{

    protected override void LoadGameObject()
    {
        // Pool를 등록하는 방식이 필요함
        Managers.Resource.RegisterPoolGameObject(Path.Boom);
        Managers.Resource.RegisterPoolGameObject(Path.Boom_Particle);
        Managers.Resource.RegisterPoolGameObject(Path.Monster);
        Managers.Resource.RegisterPoolGameObject(Path.Player);
	}

	protected override void Init()
	{
        Managers.Game.InGameInit();

        Managers.Game.Player.Spawn(Vector3.zero, new PlayerStat { id = 0, name = "Sample_Player" });
        Managers.Resource.Instantiate(Path.Monster, Managers.Game.Monster.transform);
        Managers.Resource.Instantiate(Path.Monster, Managers.Game.Monster.transform);
        Managers.Resource.Instantiate(Path.Monster, Managers.Game.Monster.transform);
    }


    public override void Clear()
    {

        Managers.Log("InGame Clear");
    }
}
