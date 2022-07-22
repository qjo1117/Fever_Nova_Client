using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private PlayerManager m_player = null;

    public PlayerManager Player { get { return m_player; } }

    private MonsterManager m_monster = null;

    public MonsterManager Monster { get { return m_monster; } }


    // 몬스터들에 대한 프리팹을 들고 있는다.
    private List<GameObject> m_listPrefab = new List<GameObject>();
    public List<GameObject> Prefab { get => m_listPrefab; }

    
    private int m_score = 0;
    public int Score { get => m_score; set => m_score = value; }

    public void Init()
    {
        //DataSystem.Load("MonsterData");
        // Prefab에 등록 및 풀링 신청(비동기이기 때문)

        // 몬스터의 데이터를 셋팅합니다.
        Managers.Resource.RegisterPoolGameObject("Monster");

    }

    // 점수 초기화 (각 씬에서 전환때 처리해주자)
    public void ScoreInit()
	{
        m_score = 0;

    }


    public void InGameInit()
	{
        // 이건 조금 생각해야할게 객체로 들고 있을 필요가 있을지가 의문
        // 구조적으로 이건 조금 생각해보자
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

        Managers.Game.Player.Init();
        Managers.Game.Monster.Init();
    }

	public void Update()
    {
        
    }
}
