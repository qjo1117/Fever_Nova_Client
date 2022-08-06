
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum range { 
 	circle,
	box,
 };

[System.Serializable]
public class SkillTable
{
		public System.Int32 index;
		public System.String name;
		public System.Int32 id;
		public System.Single skillRangeRadius;
		public System.Int32 skillObName;
		public System.Single skillObDelay;
		public System.String skillObcastingPos;
		public System.Int16 skillObQuantity;
		public System.Int16 skillCastNumber;
		public System.Single skillCastInterval;
		public System.Single skillCoolTime;
		public range rangeType;
		public System.Single rangeLength;
		public System.Single rangeWidth;
		public System.Single rangeHeight;
		public System.Single rangeRadius;
		public System.Single rangeAngle;
		public System.Int32 hitPossible;
		public System.Int32 skillDamage;
		public System.String kbOnOff;
		public System.Single kbDistance;
		public System.Int32 skillAnimation1;
		public System.Int32 skillAnimation2;
		public System.Single eventName;
		public System.Int32  fxSkillCasting;
		public System.Int32 fxSkillCollsion;


public SkillTable Clone() 
{
    SkillTable info = new SkillTable();
    		info.index = index;
		info.name = name;
		info.id = id;
		info.skillRangeRadius = skillRangeRadius;
		info.skillObName = skillObName;
		info.skillObDelay = skillObDelay;
		info.skillObcastingPos = skillObcastingPos;
		info.skillObQuantity = skillObQuantity;
		info.skillCastNumber = skillCastNumber;
		info.skillCastInterval = skillCastInterval;
		info.skillCoolTime = skillCoolTime;
		info.rangeType = rangeType;
		info.rangeLength = rangeLength;
		info.rangeWidth = rangeWidth;
		info.rangeHeight = rangeHeight;
		info.rangeRadius = rangeRadius;
		info.rangeAngle = rangeAngle;
		info.hitPossible = hitPossible;
		info.skillDamage = skillDamage;
		info.kbOnOff = kbOnOff;
		info.kbDistance = kbDistance;
		info.skillAnimation1 = skillAnimation1;
		info.skillAnimation2 = skillAnimation2;
		info.eventName = eventName;
		info. fxSkillCasting =  fxSkillCasting;
		info.fxSkillCollsion = fxSkillCollsion;

    return info;
}
}
