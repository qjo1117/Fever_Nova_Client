
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
		public System.Int32 skillRangeRadius;
		public System.Int32 skillObName;
		public System.Int32 skillObDelay;
		public System.String skillObcastingPos;
		public System.Int16 skillObQuantity;
		public System.Int16 skillCastNumber;
		public System.Int32 skillCastInterval;
		public System.Int32 skillCoolTime;
		public range rangeType;
		public System.Int32 rangeLength;
		public System.Int32 rangeWidth;
		public System.Int32 rangeHeight;
		public System.Int32 rangeRadius;
		public System.Int32 rangeAngle;
		public System.Int32 hitPossible;
		public System.Int32 skillDamage;
		public System.String kbOnOff;
		public System.Int32 kbDistance;
		public System.Int32 skillAnimation1;
		public System.Int32 skillAnimation2;
		public System.Int32 eventName;
		public System.Int32  fxSkillCasting;
		public System.Int32 fxSkillCollsion;

}
