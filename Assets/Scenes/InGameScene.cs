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
	}

	protected override void Init()
	{
        Managers.Game.InGameInit();

        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform);
        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform);
        Managers.Resource.Instantiate("Monster", Managers.Game.Monster.transform);
    }


    public override void Clear()
    {

        Managers.Log("InGame Clear");
    }
}
