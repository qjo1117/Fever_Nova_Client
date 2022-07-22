using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Path {
	public const string Player = "Assets/05Prefabs/Player";
	public const string Monster = "Monster";
	public const string Boom = "Boom";
	public const string Boom_Particle = "Assets/05Prefabs/Boom_Particle";
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