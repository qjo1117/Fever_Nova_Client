using System.Collections;
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
}