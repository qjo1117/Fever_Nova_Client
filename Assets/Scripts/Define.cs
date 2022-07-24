using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Path {
	public const string Player = "Assets/05Prefabs/Player";
	public const string Monster = "Monster";
	public const string Boom = "Boom";
	public const string Boom_Particle = "Assets/05Prefabs/Boom_Particle";

	public const string UI_PopupMsg = "UI/Popup/UI_PopupMsg";

	public const string UI_MonsterHPBar = "UI/WorldSpace/UI_MonsterHPBar";

	public const string UI_Aim = "UI/Scene/UI_Aim";
	public const string UI_BombRange = "UI/Scene/UI_BombRange";
	public const string UI_BombJumpRange = "UI/Scene/UI_BombJumpRange";
	public const string UI_BombJumpArrow = "UI/Scene/UI_BombJumpArrow";
	public const string UI_BombDropPoint = "UI/Scene/UI_BombDropPoint";
	public const string UI_BossMonsterHPBar = "UI/Scene/UI_BossMonsterHPBar";
	public const string UI_PlayerHPBar = "UI/Scene/UI_PlayerHPBar";
	public const string UI_Goal = "UI/Scene/UI_Goal";
	public const string UI_Score = "UI/Scene/UI_Score";
	public const string UI_Pause = "UI/Scene/UI_Pause";
	public const string UI_PauseScreen = "UI/Popup/UI_PauseScreen";
}


namespace Define 
{
	public enum Layer {
		Ground = 6,
		Monster = 7,
		Player = 8,
		Block = 9,

	}

	public enum Scene {
		Unknown,
		Game,
	}


	public enum UserKey {
		Forward,
		Backward,
		Right,
		Left,
		Evasion,
		Shoot,
		Esc,
		End
	}

	public enum Mouse {
		PointerDown,
		Click,
		Press,
		PointerUp
	}

	public enum UIEvent {
		Click,
		Drag,
	}

	public enum BoomState {
		Default,
		Daley,
		Player
	}

	public enum SpawnType {
		Player,
		Monster
	}

}