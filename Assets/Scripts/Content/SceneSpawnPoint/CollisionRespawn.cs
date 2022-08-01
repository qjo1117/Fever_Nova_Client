using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionRespawn : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		int l_layer = other.gameObject.layer;

		// 플레이어일 경우
		if (l_layer == (int)Define.Layer.Player) {
			other.transform.position = Managers.Game.Respawn.GetRespawnPoint(Managers.Game.RespawnIndex);
		}
		
	}
}
