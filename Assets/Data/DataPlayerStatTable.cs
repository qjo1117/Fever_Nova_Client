
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataPlayerStatTable
{
    public List<PlayerStatTable> listPCSkillTable = new List<PlayerStatTable>();

    public void DataParsing()
    {
        var data = DataSystem.Load("Data_Table_PC.Ver.0.5");

        foreach(var item in data) {
			PlayerStatTable info = new PlayerStatTable();
			info.index = (System.Int32)item["index"];
			info.name = (System.String)item["name"];
			info.id = (System.Int32)item["id"];
			info.hp = (System.Int32)item["hp"];
			info.moveSpeed = (System.Single)item["moveSpeed"];
			info.weight = (System.Single)item["weight"];
			info.dodgeSpeed = (System.Single)item["dodgeSpeed"];
			info.dodgeTime = (System.Single)item["dodgeTime"];
			info.dodgeCoolTime = (System.Single)item["dodgeCoolTime"];
			info.explosionRangeRadius = (System.Single)item["explosionRangeRadius"];
			info.explosionDamage = (System.Single)item["explosionDamage"];
			info.explosionPower = (System.Single)item["explosionPower"];
			info.kbAngle = (System.Single)item["kbAngle"];
			info.kbControl = (System.Single)item["kbControl"];
			info.launchPointOb = (System.Int32)item["launchPointOb"];
			info.boomSpeed = (System.Single)item["boomSpeed"];
			info.minRange = (System.Single)item["minRange"];
			info.maxRange = (System.Single)item["maxRange"];
			info.launchCoolTime = (System.Single)item["launchCoolTime"];
			info.inputCheckTime = (System.Single)item["inputCheckTime"];
			info.explosionDelayTime = (System.Single)item["explosionDelayTime"];
			info.obCharacter = (System.Int32)item["obCharacter"];
			info.obGun = (System.Int32)item["obGun"];
			info.obAmmo = (System.Int32)item["obAmmo"];
			info.animIdle = (System.String)item["animIdle"];
			info.animRunToFront = (System.String)item["animRunToFront"];
			info.animRunToBack = (System.Int32)item["animRunToBack"];
			info.animRunToRight = (System.Int32)item["animRunToRight"];
			info.animRunToLeft = (System.Int32)item["animRunToLeft"];
			info.animRunToFRDiagonal = (System.Int32)item["animRunToFRDiagonal"];
			info.animRunToFLDiagonal = (System.Int32)item["animRunToFLDiagonal"];
			info.animRunToBRDiagonal = (System.Int32)item["animRunToBRDiagonal"];
			info.animRunToBLDiagonal = (System.Int32)item["animRunToBLDiagonal"];
			info.animDodgeToFront = (System.Int32)item["animDodgeToFront"];
			info.animDodgeToBack = (System.Int32)item["animDodgeToBack"];
			info.animDodgeToRight = (System.Int32)item["animDodgeToRight"];
			info.animDodgeToLeft = (System.Int32)item["animDodgeToLeft"];
			info.animDodgeToFRDiagonal = (System.Int32)item["animDodgeToFRDiagonal"];
			info.animDodgeToFLDiagonal = (System.Int32)item["animDodgeToFLDiagonal"];
			info.animDodgeToBRDiagonal = (System.Int32)item["animDodgeToBRDiagonal"];
			info.animDodgeToBLDiagonal = (System.Int32)item["animDodgeToBLDiagonal"];
			info.animLaunch = (System.Int32)item["animLaunch"];
			info.animKbJump = (System.Int32)item["animKbJump"];
			info.animKbHit = (System.Int32)item["animKbHit"];
			info.animDead = (System.Int32)item["animDead"];
			info.effectLaunch = (System.Int32)item["effectLaunch"];
			info.effectExplosion = (System.Int32)item["effectExplosion"];
			info.effectHit = (System.Int32)item["effectHit"];
			info.voiceIdle = (System.Int32)item["voiceIdle"];
			info.voiceMove = (System.Int32)item["voiceMove"];
			info.voiceHit = (System.Int32)item["voiceHit"];
			info.voiceDead = (System.Int32)item["voiceDead"];
			info.soundLaunch = (System.Int32)item["soundLaunch"];
			info.soundExplosion = (System.Int32)item["soundExplosion"];

        
            listPCSkillTable.Add(info);
        }
	}

    public PlayerStatTable At(int _index)
	{
        if((0 <= _index && _index < listPCSkillTable.Count) == false) {
            return null;
		}

        return listPCSkillTable[_index];
	}
}
