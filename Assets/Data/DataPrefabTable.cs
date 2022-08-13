
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataPrefabTable
{
    public List<PrefabTable> listPrefabTable = new List<PrefabTable>();

    public void DataParsing()
    {
        var data = DataSystem.Load("Data_Table_Prefab.Ver.0.5");

        foreach(var item in data) {
            PrefabTable info = new PrefabTable();
			info.index = (System.Int32)item["index"];
			info.name = (System.String)item["name"];
			info.id = (System.Int32)item["id"];
			info.addressableName = (System.String)item["addressableName"];
			info.assetAdd = (System.String)item["assetAdd"];
			info.prefabName = (System.String)item["prefabName"];
			info.comments = (System.String)item["comments"];

        
            listPrefabTable.Add(info);
        }
	}

    public PrefabTable At(int _index)
	{
        if((0 <= _index && _index < listPrefabTable.Count) == false) {
            return null;
		}

        return listPrefabTable[_index];
	}

    public PrefabTable FindId(int _id)
    {
        foreach(PrefabTable obj in listPrefabTable) {
            if(obj.id == _id) {
                return obj;
            }
        }

        return null;
    }
}
