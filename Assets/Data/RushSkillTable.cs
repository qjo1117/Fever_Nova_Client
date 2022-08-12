
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum bT { 
 	stay,
 };
public enum SC { 
 	stay,
 };

[System.Serializable]
public class RushSkillTable
{
		public System.Int32 index;
		public System.String name;
		public System.Int32 id;
		public System.Int32 hp;
		public System.Single moveSpeed;
		public System.Single weight;
		public bT behaviorType;
		public System.Int32 dropId;
		public System.Int32 monsterGroupId;
		public System.Single detectingRangeRadius;
		public System.Int32 HoldingSkillId1;
		public System.Int16 skillPriority;
		public SC skillCond;
		public System.Single skillCondValue;
		public System.Single skillCoolTime;
		public System.Single attackDelay;
		public System.Int32 animIdle;
		public System.Int32 animMove;
		public System.Int32 animHit;
		public System.Int32 animStun;
		public System.Int32 animDead;
		public System.Int32 effectMove;
		public System.Int32 effectHit;
		public System.Int32 effectStun;
		public System.Int32 effectDead;
		public System.Int32 voiceIdle;
		public System.Int32 voiceChase;
		public System.Int32 voiceAttack;
		public System.Int32 voiceHit;


public RushSkillTable Clone() 
{
    RushSkillTable info = new RushSkillTable();
    		info.index = index;
		info.name = name;
		info.id = id;
		info.hp = hp;
		info.moveSpeed = moveSpeed;
		info.weight = weight;
		info.behaviorType = behaviorType;
		info.dropId = dropId;
		info.monsterGroupId = monsterGroupId;
		info.detectingRangeRadius = detectingRangeRadius;
		info.HoldingSkillId1 = HoldingSkillId1;
		info.skillPriority = skillPriority;
		info.skillCond = skillCond;
		info.skillCondValue = skillCondValue;
		info.skillCoolTime = skillCoolTime;
		info.attackDelay = attackDelay;
		info.animIdle = animIdle;
		info.animMove = animMove;
		info.animHit = animHit;
		info.animStun = animStun;
		info.animDead = animDead;
		info.effectMove = effectMove;
		info.effectHit = effectHit;
		info.effectStun = effectStun;
		info.effectDead = effectDead;
		info.voiceIdle = voiceIdle;
		info.voiceChase = voiceChase;
		info.voiceAttack = voiceAttack;
		info.voiceHit = voiceHit;

    return info;
}
}
