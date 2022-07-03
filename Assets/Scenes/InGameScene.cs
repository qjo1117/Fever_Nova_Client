using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{

    protected override void Init()
	{
        base.Init();

        Managers.Game.InGameInit();


        Managers.Resource.RegisterPoolGameObject("Boom");

        Managers.Log("InGame Start");


    }

    void Update()
    {
        
    }

    public override void Clear()
    {

        Managers.Log("InGame Clear");
    }
}
