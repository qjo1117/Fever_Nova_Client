using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : BaseController
{
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
    }

	private void FixedUpdate()
	{
        base.OnUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey == false) {
            return;
		}

        Vector3 move = Vector3.zero;

        if (Managers.Input.GetKey(Define.UserKey.Forward)) {
            move.z += 1.0f;
        }
        if (Managers.Input.GetKey(Define.UserKey.Backward)) {
            move.z -= 1.0f;
        } 
        if (Managers.Input.GetKey(Define.UserKey.Right)) {
            move.x += 1.0f;
        }
        if (Managers.Input.GetKey(Define.UserKey.Left)) {
            move.x -= 1.0f;
        }
        AddMovement(move);
        
        if (Managers.Input.GetKeyDown(Define.UserKey.Evasion)) {
            AddForce(Vector3.right * 10.0f);
		}
    }
}
