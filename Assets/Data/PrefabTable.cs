
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class PrefabTable
{
		public System.Int32 index;
		public System.String name;
		public System.Int32 id;
		public System.String addressableName;
		public System.String assetAdd;
		public System.String prefabName;
		public System.String comments;


public PrefabTable Clone() 
{
    PrefabTable info = new PrefabTable();
    		info.index = index;
		info.name = name;
		info.id = id;
		info.addressableName = addressableName;
		info.assetAdd = assetAdd;
		info.prefabName = prefabName;
		info.comments = comments;

    return info;
}
}
