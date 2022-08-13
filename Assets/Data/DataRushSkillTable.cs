
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataRushSkillTable
{
    public List<RushSkillTable> listRushSkillTable = new List<RushSkillTable>();

    public void DataParsing()
    {
        var data = DataSystem.Load("Data_Table_Rush.Ver.0.5");

        foreach(var item in data) {
            RushSkillTable info = new RushSkillTable();
			info.index = (System.Int32)item["index"];
			info.name = (System.String)item["name"];
			info.id = (System.Int32)item["id"];
			info.skillRangeRadius = (System.Single)item["skillRangeRadius"];
			info.skillObName = (System.Int32)item["skillObName"];
			info.skillObDelay = (System.Single)item["skillObDelay"];
			info.skillObCastingPos = (System.String)item["skillObCastingPos"];
			info.skillObQuantity = (System.Int16)item["skillObQuantity"];
			info.skillCastNumber = (System.Int16)item["skillCastNumber"];
			info.skillCastInterval = (System.Single)item["skillCastInterval"];
			info.skillCoolTinme = (System.Single)item["skillCoolTinme"];
			info.rushDistance = (System.Single)item["rushDistance"];
			info.rushSpeed = (System.Single)item["rushSpeed"];
			info.decalTime = (System.Single)item["decalTime"];
			info.stunDuration = (System.Single)item["stunDuration"];
			info.skillDamage = (System.Int32)item["skillDamage"];
			info.kbOnOff = (System.Boolean)item["kbOnOff"];
			info.kbDistance = (System.Single)item["kbDistance"];
			info.skillAnimation1 = (System.Int32)item["skillAnimation1"];
			info.skillAnimation2 = (System.Int32)item["skillAnimation2"];
			info.eventName = (System.String)item["eventName"];
			info.fxSkillCasting = (System.Int32)item["fxSkillCasting"];
			info.fxSkillCollsion = (System.Int32)item["fxSkillCollsion"];

        
            listRushSkillTable.Add(info);
        }
	}

    public RushSkillTable At(int _index)
	{
        if((0 <= _index && _index < listRushSkillTable.Count) == false) {
            return null;
		}

        return listRushSkillTable[_index];
	}

    public RushSkillTable FindId(int _id)
    {
        foreach(RushSkillTable obj in listRushSkillTable) {
            if(obj.id == _id) {
                return obj;
            }
        }

        return null;
    }
}
