
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataMeleeSkillTable
{
    public List<MeleeSkillTable> listMeleeSkillTable = new List<MeleeSkillTable>();

    public void DataParsing()
    {
        var data = DataSystem.Load("Data_Table_Melee.Ver.0.5");

        foreach(var item in data) {
            MeleeSkillTable info = new MeleeSkillTable();
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
			info.skillCoolTime = (System.Single)item["skillCoolTime"];
			info.rangeType = (RT)Enum.Parse(typeof(RT), (string)item["enum|RT|rangeType"]);
			info.rangeLength = (System.Single)item["rangeLength"];
			info.rangeWidth = (System.Single)item["rangeWidth"];
			info.rangeHeight = (System.Single)item["rangeHeight"];
			info.rangeRadius = (System.Single)item["rangeRadius"];
			info.rangeAngle = (System.Single)item["rangeAngle"];
			info.hitPossible = (System.Int32)item["hitPossible"];
			info.usingOb = (System.Int32)item["usingOb"];
			info.skillDamage = (System.Int32)item["skillDamage"];
			info.kbOnOff = (System.Boolean)item["kbOnOff"];
			info.kbDistance = (System.Single)item["kbDistance"];
			info.skillAnimation1 = (System.Int32)item["skillAnimation1"];
			info.skillAnimation2 = (System.Int32)item["skillAnimation2"];
			info.eventName = (System.String)item["eventName"];
			info.fxSkillCasting = (System.Int32)item["fxSkillCasting"];
			info.fxSkillCollsion = (System.Int32)item["fxSkillCollsion"];

        
            listMeleeSkillTable.Add(info);
        }
	}

    public MeleeSkillTable At(int _index)
	{
        if((0 <= _index && _index < listMeleeSkillTable.Count) == false) {
            return null;
		}

        return listMeleeSkillTable[_index];
	}
}
