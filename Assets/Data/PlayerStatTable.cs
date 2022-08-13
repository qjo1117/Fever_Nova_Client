
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class PlayerStatTable
{
		public System.Int32 index;
		public System.String name;
		public System.Int32 id;
		public System.Int32 hp;
		public System.Single moveSpeed;
		public System.Single weight;
		public System.Single dodgeSpeed;
		public System.Single dodgeTime;
		public System.Single dodgeCoolTime;
		public System.Single explosionRangeRadius;
		public System.Single explosionDamage;
		public System.Single explosionPower;
		public System.Single kbAngle;
		public System.Single kbControl;
		public System.Int32 launchPointOb;
		public System.Single boomSpeed;
		public System.Single minRange;
		public System.Single maxRange;
		public System.Single launchCoolTime;
		public System.Single inputCheckTime;
		public System.Single explosionDelayTime;
		public System.Int32 obCharacter;
		public System.Int32 obGun;
		public System.Int32 obAmmo;
		public System.String animIdle;
		public System.String animRunToFront;
		public System.Int32 animRunToBack;
		public System.Int32 animRunToRight;
		public System.Int32 animRunToLeft;
		public System.Int32 animRunToFRDiagonal;
		public System.Int32 animRunToFLDiagonal;
		public System.Int32 animRunToBRDiagonal;
		public System.Int32 animRunToBLDiagonal;
		public System.Int32 animDodgeToFront;
		public System.Int32 animDodgeToBack;
		public System.Int32 animDodgeToRight;
		public System.Int32 animDodgeToLeft;
		public System.Int32 animDodgeToFRDiagonal;
		public System.Int32 animDodgeToFLDiagonal;
		public System.Int32 animDodgeToBRDiagonal;
		public System.Int32 animDodgeToBLDiagonal;
		public System.Int32 animLaunch;
		public System.Int32 animKbJump;
		public System.Int32 animKbHit;
		public System.Int32 animDead;
		public System.Int32 effectLaunch;
		public System.Int32 effectExplosion;
		public System.Int32 effectHit;
		public System.Int32 voiceIdle;
		public System.Int32 voiceMove;
		public System.Int32 voiceHit;
		public System.Int32 voiceDead;
		public System.Int32 soundLaunch;
		public System.Int32 soundExplosion;


public PlayerStatTable Clone() 
{
    PlayerStatTable info = new PlayerStatTable();
    		info.index = index;
		info.name = name;
		info.id = id;
		info.hp = hp;
		info.moveSpeed = moveSpeed;
		info.weight = weight;
		info.dodgeSpeed = dodgeSpeed;
		info.dodgeTime = dodgeTime;
		info.dodgeCoolTime = dodgeCoolTime;
		info.explosionRangeRadius = explosionRangeRadius;
		info.explosionDamage = explosionDamage;
		info.explosionPower = explosionPower;
		info.kbAngle = kbAngle;
		info.kbControl = kbControl;
		info.launchPointOb = launchPointOb;
		info.boomSpeed = boomSpeed;
		info.minRange = minRange;
		info.maxRange = maxRange;
		info.launchCoolTime = launchCoolTime;
		info.inputCheckTime = inputCheckTime;
		info.explosionDelayTime = explosionDelayTime;
		info.obCharacter = obCharacter;
		info.obGun = obGun;
		info.obAmmo = obAmmo;
		info.animIdle = animIdle;
		info.animRunToFront = animRunToFront;
		info.animRunToBack = animRunToBack;
		info.animRunToRight = animRunToRight;
		info.animRunToLeft = animRunToLeft;
		info.animRunToFRDiagonal = animRunToFRDiagonal;
		info.animRunToFLDiagonal = animRunToFLDiagonal;
		info.animRunToBRDiagonal = animRunToBRDiagonal;
		info.animRunToBLDiagonal = animRunToBLDiagonal;
		info.animDodgeToFront = animDodgeToFront;
		info.animDodgeToBack = animDodgeToBack;
		info.animDodgeToRight = animDodgeToRight;
		info.animDodgeToLeft = animDodgeToLeft;
		info.animDodgeToFRDiagonal = animDodgeToFRDiagonal;
		info.animDodgeToFLDiagonal = animDodgeToFLDiagonal;
		info.animDodgeToBRDiagonal = animDodgeToBRDiagonal;
		info.animDodgeToBLDiagonal = animDodgeToBLDiagonal;
		info.animLaunch = animLaunch;
		info.animKbJump = animKbJump;
		info.animKbHit = animKbHit;
		info.animDead = animDead;
		info.effectLaunch = effectLaunch;
		info.effectExplosion = effectExplosion;
		info.effectHit = effectHit;
		info.voiceIdle = voiceIdle;
		info.voiceMove = voiceMove;
		info.voiceHit = voiceHit;
		info.voiceDead = voiceDead;
		info.soundLaunch = soundLaunch;
		info.soundExplosion = soundExplosion;

    return info;
}
}
