using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	private List<RespawnPoint> m_listRespawnPoint = new List<RespawnPoint>();

	public void Init()
	{
		//int l_size = transform.childCount;
		//for (int i = 0; i < l_size; ++i) {
		//	RespawnPoint l_point = transform.GetChild(i).GetComponent<RespawnPoint>();
		//	l_point.Index = i;
		//	m_listRespawnPoint.Add(l_point);
		//}
	}


	public Vector3 GetRespawnPoint(int _index)
	{
		return m_listRespawnPoint[_index].transform.position;
	}
}
