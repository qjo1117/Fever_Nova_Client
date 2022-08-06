
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class MonsterStatTable
{
		public System.Int32 index;
		public System.String name;
		public System.Int32 id;
		public System.Int32 HP;
		public System.Single moveSpeed;
		public System.Single weight;


public MonsterStatTable Clone() 
{
    MonsterStatTable info = new MonsterStatTable();
    		info.index = index;
		info.name = name;
		info.id = id;
		info.HP = HP;
		info.moveSpeed = moveSpeed;
		info.weight = weight;

    return info;
}
}
