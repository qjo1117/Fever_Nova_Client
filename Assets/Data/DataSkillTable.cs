
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSkillTable
{
    public List<SkillTable> listSkillTable;

    public void DataParsing()
    {
        var data = DataSystem.Load("SkillTable.ver.0.3");

        foreach(var item in data) {
            SkillTable info = new SkillTable();
			info.index = (System.Int32)item["index"];
			info.name = (System.String)item["name"];
			info.id = (System.Int32)item["id"];
			info.skillRangeRadius = (System.String)item["skillRangeRadius"];
			info.skillObName = (System.String)item["skillObName"];
			info.skillObDelay = (System.Int32)item["skillObDelay"];
			info.skillObcastingPos = (System.String)item["skillObcastingPos"];
			info.skillObQuantity = (System.Int16)item["skillObQuantity"];
			info.skillCastNumber = (System.Int16)item["skillCastNumber"];
			info.skillCastInterval = (System.Int32)item["skillCastInterval"];
			info.skillCoolTime = (System.Int32)item["skillCoolTime"];
			info.rangeType = (range)Enum.Parse(typeof(range), (string)item["enum|range|rangeType"]);
			info.rangeLength = (System.String)item["rangeLength"];
			info.rangeWidth = (System.String)item["rangeWidth"];
			info.rangeHeight = (System.Int32)item["rangeHeight"];
			info.rangeRadius = (System.Int32)item["rangeRadius"];
			info.rangeAngle = (System.Int32)item["rangeAngle"];
			info.hitPossible = (System.Int32)item["hitPossible"];
			info.skillDamage = (System.Int32)item["skillDamage"];
			info.kbOnOff = (System.String)item["kbOnOff"];
			info.kbDistance = (System.Int32)item["kbDistance"];
			info.skillAnimation1 = (System.String)item["skillAnimation1"];
			info.skillAnimation2 = (System.String)item["skillAnimation2"];
			info.eventName = (System.String)item["eventName"];
			info. fxSkillCasting = (System.String)item[" fxSkillCasting"];
			info.fxSkillCollsion = (System.String)item["fxSkillCollsion"];

        
            listSkillTable.Add(info);
        }
	}
}
