using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Define 
{

	public enum Layer {
		Ground = 1 << 6,
		Bullet = 1 << 7,
		Player = 1 << 8,
		Monster = 1 << 9,
		Block = 1 << 10,
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
		Esc,
		End
	}
}