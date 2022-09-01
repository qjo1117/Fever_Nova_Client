using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
	#region Variable
	private PlayerManager       m_player = null;
    private MonsterManager      m_monster = null;
    private BombManager         m_boom = null;
    private RespawnManager      m_respawn = null;
    private ItemManager         m_item = null;

    private int                 m_respawnIndex = 0;


    private int                 m_score = 0;
    private int                 m_allMonsterCount = 0;

    // Unity의 Time을 이용하여 플레이타임 구현
    private float               m_beginPlayTime = 0;
    private float               m_endPlayTime = 0;

    // 몬스터들에 대한 프리팹을 들고 있는다.
    private List<GameObject>    m_listPrefab = new List<GameObject>();

    private bool                m_isPlay = false;
    private bool                m_isMulti = false;
    #endregion



    #region Property
    public PlayerManager Player { get => m_player; }
    public MonsterManager Monster { get => m_monster; }
    public BombManager Boom { get => m_boom; }
    public RespawnManager Respawn { get => m_respawn; }
    // Item Manager 추가 (문제 될시 삭제 바람)
    public ItemManager Item { get => m_item; }

    public List<GameObject> Prefab { get => m_listPrefab; }
    public int Score { get => m_score; set => m_score = value; }
    public int MonsterCount { get => m_allMonsterCount; set => m_allMonsterCount = value; }
    public int RespawnIndex { get => m_respawnIndex; set => m_respawnIndex = value; }
    // 플레이 타임 UI에 출력시키기 위해
    public float BeginPlayTime { get => m_beginPlayTime; set => m_beginPlayTime = value; }
    public float EndPlayTime { get => m_endPlayTime; set => m_endPlayTime = value; }
    public bool IsPlay { get => m_isPlay; set => m_isPlay = value; }
    public bool IsMulti { get => m_isMulti; set => m_isMulti = value; }
    #endregion



    public void Init()
    {
         // 오브젝트가 처음 생성되엇을때 필요한 정보를 가지고 있는다.

    }

    // 참견 : 차라리 각 씬을 로드했을때 시작하는 함수를 정하는게 어떨까 예를들어 StartGame처럼
    // 점수 초기화 (각 씬에서 전환때 처리해주자)
    public void ScoreInit()
	{
        m_score = 0;
        m_allMonsterCount = 0;
    }


    // 게임이 시작할때 가장 필요한 요소들을 셋팅한다.
    public void StartGame()
	{
        ScoreInit();

        // Manager 맵핑
        m_player = Util.FindGetOrAddGameObject<PlayerManager>("PlayerManager");
        m_monster = Util.FindGetOrAddGameObject<MonsterManager>("MonsterManager");
        m_boom = Util.FindGetOrAddGameObject<BombManager>("BombManager");
        m_respawn = Util.FindGetOrAddGameObject<RespawnManager>("RespawnManager");
        m_item = Util.FindGetOrAddGameObject<ItemManager>("ItemManager");

        if (m_isMulti == true) {
            Managers.Network.Init();
        }

        m_player.Init();
        m_monster.Init();
        m_respawn.Init();
        m_item.Init();

        // 중력을 조금 강하게 셋팅한다. 움직임에 대한 기본 마찰력이 강하기 때문
        Physics.gravity = -9.8f * Vector3.up * 5.0f;

        // 시작 시간을 가져온다.
        m_beginPlayTime = Time.time;

        Managers.Game.IsPlay = true;
        if(m_isMulti == false) {
            m_player.Spawn(m_player.SpanwPoint, new PlayerStat { id = 0, name = "SamplePlayer" });
		}

    }

    public void Clear()
	{
        Managers.Game.IsPlay = false;
        if (m_isMulti == true) {
            Managers.Network.End();
            m_isMulti = false;
        }
    }


	public void Update()
    {
        if (m_isPlay == false) {
            return;
        }
        
        m_monster.OnUpdate();

        if (m_isMulti == true) {
            Managers.Network.Update();
        }
        else {
            // 게임을 끝내자
            if(m_monster.MonsterKillCount >= m_allMonsterCount) {
                if (Util.FindChild<UI_ResultScreen>(Managers.UI.Root, "UI_ResultScreen") != null)
                {
                    return;
                }

                // 게임 결과창 출력
                Managers.UI.ShowPopupUI<UI_ResultScreen>("UI_ResultScreen");
                UI_Result l_result = Managers.UI.ShowPopupUI<UI_Result>("UI_Result");

                PlayerController player = m_player.MainPlayer;

                l_result.PlayerId = 1;
                l_result.Result = "Defeat...";
                l_result.Score = player.Stat.score;
                l_result.GameStartTime = Managers.Game.BeginPlayTime;
                l_result.KillCount = player.MonsterKillCount;
                l_result.MultiKillCount = player.MonsterMultiKillCount;
                l_result.HitCount = player.HitCount;
                l_result.TotalScore = player.Stat.totalScore;
            }
		}

    }

    // 12FPS
    private float m_currentTime = 0.0f;
    private float m_currentMaxTime = 5.0f;
    public void FixedUpdate()
	{
        m_currentTime += 1;
        if(m_currentTime < m_currentMaxTime) {
            return;
		}
        m_currentTime -= m_currentMaxTime;

        if (m_isPlay == false) {
            return;
        }

        m_player.OnFixedUpdate();
	}
}
