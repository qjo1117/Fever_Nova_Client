using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour 
{
    // 현재 소환할 인덱스
    public int m_index = -1;

	public Color GetColor()
	{
		// -1 일 경우, 더 경우가 없는 경우 magenta / 설정해야한다고 알리는 용도
		switch (m_index) {
			case -1:
				return Color.magenta;
			case 0:
				return Color.red;
			case 1:
				return Color.green;
			case 2:
				return Color.blue;
			case 3:
				return Color.cyan;
			case 4:
				return Color.yellow;
			case 5:
				return Color.white;
			default:
				return Color.magenta;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = GetColor();

		Gizmos.DrawWireSphere(transform.position, 2.0f);
	}
}
