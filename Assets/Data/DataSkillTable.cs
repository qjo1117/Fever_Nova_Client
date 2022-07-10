
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
        var data = DataSystem.Load("Stage_01_SkillTable.ver.0.1");

        foreach(var item in data) {
            SkillTable info = new SkillTable();
			info.Index = (System.Int32)item["Index"];
			info.Name = (System.String)item["Name"];
			info.ID = (System.Int32)item["ID"];
			info.skill_Ob_Name = (System.String)item["skill_Ob_Name"];
			info.skill_Ob_Delay = (System.Int32)item["skill_Ob_Delay"];
			info.skill_Ob_castingPos = (System.String)item["skill_Ob_castingPos"];
			info.skill_Ob_Quantity = (System.Int32)item["skill_Ob_Quantity"];
			info.skill_Cast_Number = (System.Int32)item["skill_Cast_Number"];
			info.skill_CastInterval = (System.Int32)item["skill_CastInterval"];
			info.skill_CoolTime = (System.Int32)item["skill_CoolTime"];
			info.range_Type = (System.String)item["range_Type"];
			info.range_Length = (System.Int32)item["range_Length"];
			info.range_Width = (System.Int32)item["range_Width"];
			info.range_Height = (System.Int32)item["range_Height"];
			info.range_Radius = (System.Int32)item["range_Radius"];
			info.range_Angle = (System.Int32)item["range_Angle"];
			info.hitPossible = (System.Int32)item["hitPossible"];
			info.skill_Damage = (System.Int32)item["skill_Damage"];
			info.kbOnOff = (System.Boolean)item["kbOnOff"];
			info.kbDistance = (System.Int32)item["kbDistance"];
			info.skill_Animation1 = (System.Int16)item["skill_Animation1"];
			info.skill_Animation2 = (System.Int16)item["skill_Animation2"];
			info.eventName = (System.Int32)item["eventName"];
			info.FX_skillCasting = (System.Int16)item["FX_skillCasting"];
			info.FX_skillCollsion = (System.Int16)item["FX_skillCollsion"];

        
            listSkillTable.Add(info);
        }
	}
}
