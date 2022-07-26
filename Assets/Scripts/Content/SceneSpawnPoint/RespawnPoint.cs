using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
	private int m_index = 0;
	public int Index { get => m_index; set => m_index = value; }

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer != (int)Define.Layer.Player) {
			return;
		}

		Managers.Game.RespawnIndex = m_index;
	}


}
