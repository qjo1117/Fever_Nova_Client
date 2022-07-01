using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension 
{
	// 나중에 좀 귀찮다 싶으면 다시 넣어볼 생각
	//public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	//{
	//	return GetOrAddComponent<T>(go);
	//}

	public static bool IsValid(this GameObject go)
	{
		return go != null && go.activeSelf;
	}

}