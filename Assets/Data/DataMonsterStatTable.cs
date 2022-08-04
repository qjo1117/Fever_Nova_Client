
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataMonsterStatTable
{
    public List<MonsterStatTable> listMonsterStatTable = new List<MonsterStatTable>();

    public void DataParsing()
    {
        var data = DataSystem.Load("Data_Table.Ver0.4");

        foreach(var item in data) {
            MonsterStatTable info = new MonsterStatTable();
			info.index = (System.Int32)item["index"];
			info.name = (System.String)item["name"];
			info.id = (System.Int32)item["id"];
			info.HP = (System.Int32)item["HP"];
			info.moveSpeed = (System.Int32)item["moveSpeed"];
			info.weight = (System.Int32)item["weight"];

        
            listMonsterStatTable.Add(info);
        }
	}

    public MonsterStatTable At(int _index)
	{
        if((0 <= _index && _index < listMonsterStatTable.Count) == false) {
            return null;
		}

        return listMonsterStatTable[_index];
	}
}
