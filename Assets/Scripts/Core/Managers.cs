using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
	private static Managers _instance = null;
	public static Managers Instance { get { Init(); return _instance; } }

	#region Core
	ResourceManager		_resource = new ResourceManager();
	PoolManager			_pool = new PoolManager();
	SceneManagerEx		_scene = new SceneManagerEx();
	InputManager		_input = new InputManager();
	UIManager			_ui = new UIManager();

	public static ResourceManager Resource {  get { return Instance._resource; } }
	public static PoolManager Pool {  get { return Instance._pool; } }
	public static SceneManagerEx Scene {  get { return Instance._scene; } }

	public static InputManager Input { get { return Instance._input; } }

	public static UIManager UI { get => Instance._ui; }
	#endregion

	#region Content

	GameManager _game = new GameManager();
	public static GameManager Game { get { return Instance._game; } }
	#endregion

	private void Start()
	{
		Init();
	}

	static public void Log(object obj)
	{
#if DEBUG
		Debug.Log(obj);
#endif
	}

	static void Init()
	{
		if(_instance == null) {
			GameObject go = GameObject.Find("@Managers");
			if(go == null) {
				go = new GameObject { name = "@Managers" };
				go.AddComponent<Managers>();
			}

			// 삭제 방지
			DontDestroyOnLoad(go);
			_instance = go.GetComponent<Managers>();

			_instance._input.Init();
			_instance._pool.Init();
			_instance._game.Init();
		}

	}

	private void Update()
	{
		_instance._input.Update();
	}


	public static void Clear()
	{
		_instance._pool.Clear();
		_instance._scene.Clear();
		_instance._resource.Clear();
	}
}

