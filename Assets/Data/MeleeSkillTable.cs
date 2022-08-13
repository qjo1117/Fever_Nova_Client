
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeleeSkillTable
{
		public System.Int32 index;
		public System.String name;
		public System.Int32 id;
		public System.Single skillRangeRadius;
		public System.Int32 skillObName;
		public System.Single skillObDelay;
		public System.String skillObCastingPos;
		public System.Int16 skillObQuantity;
		public System.Int16 skillCastNumber;
		public System.Single skillCastInterval;
		public System.Single skillCoolTime;
		public RT rangeType;
		public System.Single rangeLength;
		public System.Single rangeWidth;
		public System.Single rangeHeight;
		public System.Single rangeRadius;
		public System.Single rangeAngle;
		public System.Int32 hitPossible;
		public System.Int32 usingOb;
		public System.Int32 skillDamage;
		public System.Boolean kbOnOff;
		public System.Single kbDistance;
		public System.Int32 skillAnimation1;
		public System.Int32 skillAnimation2;
		public System.String eventName;
		public System.Int32 fxSkillCasting;
		public System.Int32 fxSkillCollsion;


public MeleeSkillTable Clone() 
{
    MeleeSkillTable info = new MeleeSkillTable();
    		info.index = index;
		info.name = name;
		info.id = id;
		info.skillRangeRadius = skillRangeRadius;
		info.skillObName = skillObName;
		info.skillObDelay = skillObDelay;
		info.skillObCastingPos = skillObCastingPos;
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
		info.usingOb = usingOb;
		info.skillDamage = skillDamage;
		info.kbOnOff = kbOnOff;
		info.kbDistance = kbDistance;
		info.skillAnimation1 = skillAnimation1;
		info.skillAnimation2 = skillAnimation2;
		info.eventName = eventName;
		info.fxSkillCasting = fxSkillCasting;
		info.fxSkillCollsion = fxSkillCollsion;

    return info;
}
}
