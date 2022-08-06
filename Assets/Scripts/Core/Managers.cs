using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
	private static Managers m_instance = null;
	public static Managers Instance { get { Init(); return m_instance; } }

	#region Core
	private ResourceManager		m_resource = new ResourceManager();
	private PoolManager			m_pool = new PoolManager();
	private SceneManagerEx		m_scene = new SceneManagerEx();
	private InputManager		m_input = new InputManager();
	private UIManager			m_ui = new UIManager();
	private NetWorkManager		m_network = new NetWorkManager();

	private DataManager			m_data = null;
	public static ResourceManager Resource {  get { return Instance.m_resource; } }
	public static PoolManager Pool {  get { return Instance.m_pool; } }
	public static SceneManagerEx Scene {  get { return Instance.m_scene; } }

	public static InputManager Input { get { return Instance.m_input; } }

	public static UIManager UI { get => Instance.m_ui; }
	public static DataManager Data { get => Instance.m_data; }
	public static NetWorkManager Network { get => Instance.m_network; }

	#endregion

	#region Content

	private GameManager			m_game = new GameManager();
	public static GameManager Game { get { return Instance.m_game; } }

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
		if(m_instance == null) {
			GameObject go = GameObject.Find("@Managers");
			if(go == null) {
				go = new GameObject { name = "@Managers" };
				go.GetOrAddComponent<Managers>();
			}

			// 삭제 방지
			DontDestroyOnLoad(go);
			m_instance = go.GetOrAddComponent<Managers>();

			GameObject data = GameObject.Find("@DataManager");
			if (data == null) {
				data = new GameObject { name = "@DataManager" };
			}
			m_instance.m_data = data.GetOrAddComponent<DataManager>();
			m_instance.m_data.LoadData();
			DontDestroyOnLoad(data);

			m_instance.m_resource.Init();
			m_instance.m_pool.Init();
			m_instance.m_game.Init();
			m_instance.m_input.Init();

		}

	}

	private void Update()
	{
		m_instance.m_input.Update();
		m_instance.m_resource.Update();
		if(m_instance.m_game.IsPlay == true) {
			m_instance.m_game.Update();
		}

	}

	private void FixedUpdate()
	{
		
	}


	public static void Clear()
	{
		m_instance.m_input.Clear();
		m_instance.m_pool.Clear();
		m_instance.m_game.Clear();
		m_instance.m_scene.Clear();
		m_instance.m_resource.Clear();
	}
}

