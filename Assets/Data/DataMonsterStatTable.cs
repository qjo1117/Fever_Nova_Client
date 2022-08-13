
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
        var data = DataSystem.Load("Data_Table_MonsterStat.Ver.0.5");

        foreach(var item in data) {
            MonsterStatTable info = new MonsterStatTable();
			info.index = (System.Int32)item["index"];
			info.name = (System.String)item["name"];
			info.id = (System.Int32)item["id"];
			info.hp = (System.Int32)item["hp"];
			info.moveSpeed = (System.Single)item["moveSpeed"];
			info.weight = (System.Single)item["weight"];
			info.behaviorType = (bT)Enum.Parse(typeof(bT), (string)item["enum|bT|behaviorType"]);
			info.dropId = (System.Int32)item["dropId"];
			info.monsterGroupId = (System.Int32)item["monsterGroupId"];
			info.detectingRangeRadius = (System.Single)item["detectingRangeRadius"];
			info.HoldingSkillId1 = (System.Int32)item["HoldingSkillId1"];
			info.skillPriority = (System.Int16)item["skillPriority"];
			info.skillCond = (SC)Enum.Parse(typeof(SC), (string)item["enum|SC|skillCond"]);
			info.skillCondValue = (System.Single)item["skillCondValue"];
			info.skillCoolTime = (System.Single)item["skillCoolTime"];
			info.attackDelay = (System.Single)item["attackDelay"];
			info.animIdle = (System.Int32)item["animIdle"];
			info.animMove = (System.Int32)item["animMove"];
			info.animHit = (System.Int32)item["animHit"];
			info.animStun = (System.Int32)item["animStun"];
			info.animDead = (System.Int32)item["animDead"];
			info.effectMove = (System.Int32)item["effectMove"];
			info.effectHit = (System.Int32)item["effectHit"];
			info.effectStun = (System.Int32)item["effectStun"];
			info.effectDead = (System.Int32)item["effectDead"];
			info.voiceIdle = (System.Int32)item["voiceIdle"];
			info.voiceChase = (System.Int32)item["voiceChase"];
			info.voiceAttack = (System.Int32)item["voiceAttack"];
			info.voiceHit = (System.Int32)item["voiceHit"];

        
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

    public MonsterStatTable FindId(int _id)
    {
        foreach(MonsterStatTable obj in listMonsterStatTable) {
            if(obj.id == _id) {
                return obj;
            }
        }

        return null;
    }
}
