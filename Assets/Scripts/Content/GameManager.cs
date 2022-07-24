using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private PlayerManager m_player = null;
    private MonsterManager m_monster = null;
    private BoomManager m_boom = null;
    private int m_score = 0;

    private int m_beginPlayTime = 0;
    private int m_endPlayTime = 0;

    // 몬스터들에 대한 프리팹을 들고 있는다.
    private List<GameObject> m_listPrefab = new List<GameObject>();

    #region Property
    public PlayerManager Player { get => m_player; }
    public MonsterManager Monster { get => m_monster; }
    public BoomManager Boom { get => m_boom; }
    public List<GameObject> Prefab { get => m_listPrefab; }
    public int Score { get => m_score; set => m_score = value; }
    #endregion



    public void Init()
    {
         // 오브젝트가 처음 생성되엇을때 필요한 정보를 가지고 있는다.

    }

    // 점수 초기화 (각 씬에서 전환때 처리해주자)
    public void ScoreInit()
	{
        m_score = 0;

    }

    // 게임이 시작할때 가장 필요한 요소들을 셋팅한다.
    public void StartGame()
	{
        {
            GameObject obj = GameObject.Find("@PlayerManager");
            if (obj == null) {
                obj = new GameObject { name = "@PlayerManager" };
            }
            m_player = obj.GetOrAddComponent<PlayerManager>();
        }

        {
            GameObject obj = GameObject.Find("@MonsterManager");
            if (obj == null) {
                obj = new GameObject { name = "@MonsterManager" };
            }
            m_monster = obj.GetOrAddComponent<MonsterManager>();
        }

        {
            GameObject obj = GameObject.Find("@BoomManager");
            if (obj == null)
            {
                obj = new GameObject { name = "@BoomManager" };
            }
            m_boom = obj.GetOrAddComponent<BoomManager>();
        }

        Managers.Game.Player.Init();
        Managers.Game.Monster.Init();

        // 시작 시간을 가져온다.
        m_beginPlayTime = DateTime.Now.Second + DateTime.Now.Month * 60;
    }

    public void Clear()
	{

	}


	public void Update()
    {
        
    }
}
