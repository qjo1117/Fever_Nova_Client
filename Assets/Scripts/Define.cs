﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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