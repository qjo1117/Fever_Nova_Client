using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class PlayerManager : MonoBehaviour
{
	#region Variable
	public Transform                m_spawnPoint = null;
    private List<PlayerController>  m_listPlayers = new List<PlayerController>();
    private PlayerController        m_mainPlayer = null;

    private int m_id = 0;

    private PacketMoveData m_moveData = new PacketMoveData();

    #endregion

    #region Property
    // 이름 추천 받음
    public List<PlayerController> List { get => m_listPlayers; }
    public PlayerController MainPlayer { get => m_mainPlayer; }
    public Vector3 SpanwPoint { get => m_spawnPoint.position; }

	#endregion

	#region Player Variable
	[SerializeField]
    private float           m_jumpRange = 5.0f;
	[SerializeField]
    private float           m_explosionRange = 12.0f;
    #endregion

    #region Player Property
    public float JumpRange { get => m_jumpRange; }
    public float ExplosionRange { get => m_explosionRange; }
    #endregion

    void Update()
    {

        // 업데이트
        m_mainPlayer?.OnUpdate();
    }

	private void FixedUpdate()
	{
        PlayerUpdate();

    }

	private void PlayerUpdate()
	{
        if(Managers.Game.IsMulti == false){
            return;
		}

		foreach(PlayerController player in m_listPlayers) {
            if(player.StatTable.id == m_mainPlayer.StatTable.id) {
                continue;
			}

            player.NetworkUpdate();
		}
	}

    private void LateUpdate()
	{
        m_mainPlayer?.OnLateUpdate();
	}

	#region Player Spawn
	public PlayerController FindPlayer(int _id)
	{
        foreach(PlayerController l_player in m_listPlayers) {
            // 아이디 검출을 인스턴스 아이디로 검출한다.
            if (l_player.StatTable.id == _id) {
                return l_player;
			}
		}

        return null;
	}

    // 스폰을 시킨다.
    public PlayerController Spawn(Vector3 _position, PlayerStat _stat)
	{
        PlayerController l_player = Managers.Resource.Instantiate(Path.Player, transform).GetOrAddComponent<PlayerController>();
        l_player.transform.position = _position;                        // 좌표 반영
        //l_player.Stat = _stat;                                          // 스텟 반영
        l_player.StatTable = Managers.Data.PlayerStat.At(_stat.id);     // 실제 CSV
        m_listPlayers.Add(l_player);                                    // 매니저 추가

        // 메인 플레이어 설정
        if (m_mainPlayer == null) {
            m_mainPlayer = l_player;
            
            CameraController l_camera = GameObject.FindObjectOfType<CameraController>();
            l_camera.SetPlayer(l_player.gameObject);

            m_mainPlayer.ExplosionJumpRadius = m_jumpRange;
            m_mainPlayer.ExplosionRadius = m_explosionRange;

            // 플레이어 HP바 생성
            UI_PlayerHPBar l_playerHPBar = Managers.UI.ShowSceneUI<UI_PlayerHPBar>("UI_PlayerHPBar");
            l_playerHPBar.HP = m_mainPlayer.StatTable.hp;
            l_playerHPBar.MaxHP = m_mainPlayer.StatTable.hp;
            Managers.UI.SetCanvas(l_playerHPBar.gameObject, false);
            m_mainPlayer.PlayerHPBar = l_playerHPBar;

            // MainPlayer전용 UI셋팅
            UI_BombDropPoint l_bombRange = Managers.UI.Root.GetComponentInChildren<UI_BombDropPoint>();
            l_bombRange.BombJumpRange.RangeRadius = m_jumpRange;
            l_bombRange.BombRange.RangeRadius = m_explosionRange;
        }

        l_player.Init();

        return l_player;
    }

    public void DeSpawn(PlayerController _player)
    {
        m_listPlayers.Remove(_player);
    }

    public void DeSpawn(int _id)
	{
        foreach(PlayerController player in m_listPlayers) {
            if(player.StatTable.id == _id) {
                m_listPlayers.Remove(player);
                Managers.Resource.Destroy(player.gameObject);
                return;
			}
		}    
	}

	#endregion

	// 초기화 작업을 한다.
	public void Init()
    {
        Managers.Network.Register(E_PROTOCOL.STC_SPAWN, InuserProcess);
        Managers.Network.Register(E_PROTOCOL.STC_MOVE, PositionProcess);
        Managers.Network.Register(E_PROTOCOL.STC_OUT, GuestOutProcess);
    }

    public void InuserProcess()
	{
        Session l_session = Managers.Network.Session;
        PlayerManager l_playerManager = Managers.Game.Player;

        ListData l_data;
        l_session.GetData(out l_data);

        int size = l_data.m_size;
        l_playerManager.Spawn(Managers.Game.Player.SpanwPoint, new PlayerStat { id = l_data.m_list[size - 1], name = $"Sample_Player/{l_data.m_list[size - 1]}" });

        for (int i = 0; i < size - 1; ++i) {
            int l_index = l_data.m_list[i];
            int l_findIndex = l_playerManager.FindIndex(l_index);

            // -1이거나 PlayerManager에서 순회했는데 -1이 아닌경우 처음보는 녀석이다.
            if (l_index == -1 || l_findIndex != -1) {
                continue;
			}
            l_playerManager.Spawn(Managers.Game.Player.SpanwPoint, new PlayerStat { id = l_data.m_list[i], name = $"Sample_Player/{l_data.m_list[i]}" });
        }
    }

    public void PositionProcess()
	{
        PacketMoveData l_data;
        Session l_session = Managers.Network.Session;
        l_session.GetData(out l_data);

        if(m_mainPlayer.StatTable.id == l_data.m_id) {
            return;
		}

        PlayerController l_player = Managers.Game.Player.At(l_data.m_id);

        l_player.transform.position = new Vector3(l_data.m_positionX, l_data.m_positionY, l_data.m_positionZ);
        l_player.transform.rotation = new Quaternion(l_data.m_rotationX, l_data.m_rotationY, l_data.m_rotationZ, l_data.m_rotationW);
        l_player.AnimMove = new Vector3(l_data.m_moveX, 0.0f, l_data.m_moveZ);
        l_player.Aiming = l_data.m_animing;
        l_player.State = (PlayerController.PlayerState)l_data.m_state;
    }

    public void GuestOutProcess()
	{
        int l_data = -1;
        Session l_session = Managers.Network.Session;
        l_session.GetData(out l_data);

        Managers.Game.Player.DeSpawn(l_data);
    }


    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
	{
        // 멀0
        if (Managers.Game.IsMulti == true && m_mainPlayer != null) {
            m_moveData.m_id = m_mainPlayer.StatTable.id;

            Vector3 l_position = m_mainPlayer.transform.position;
            m_moveData.m_positionX = l_position.x;
            m_moveData.m_positionY = l_position.y;
            m_moveData.m_positionZ = l_position.z;

            m_moveData.m_rotationX = m_mainPlayer.transform.rotation.x;
            m_moveData.m_rotationY = m_mainPlayer.transform.rotation.y;
            m_moveData.m_rotationZ = m_mainPlayer.transform.rotation.z;
            m_moveData.m_rotationW = m_mainPlayer.transform.rotation.w;

            m_moveData.m_moveX = m_mainPlayer.AnimMove.x;
            m_moveData.m_moveZ = m_mainPlayer.AnimMove.z;

            m_moveData.m_animing = m_mainPlayer.Aiming;

            m_moveData.m_state = (int)m_mainPlayer.State;

            Managers.Network.Session.Write((int)E_PROTOCOL.CTS_MOVE, m_moveData);
        }
    }

    

    public void Clear()
	{
        //      // 만약 하이라키에 플레이어가 생존해 있으면 삭제 시켜버린다.
        //      foreach(PlayerController player in m_listPlayers) {
        //          Managers.Resource.DelPrefab(player.gameObject);
        //}
        //      // 초기화
        //      m_listPlayers.Clear();

    }

    // 대미지를 입힐때 쓰인다.
    public void Damage(int p_id, int p_attack, Vector3 p_force)
	{
		At(p_id).Damage(p_attack);
        At(p_id).Rigid.AddForce(p_force);
	}

    public void Damage(PlayerController p_player, int p_attack, Vector3 p_force)
	{
        //// 위에 있는 함수 호출
        Damage(p_player.StatTable.id, p_attack, p_force);
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(m_spawnPoint.position, Vector3.one * 3.0f);
	}

    public PlayerController At(int _index)
	{
        foreach(PlayerController player in m_listPlayers) {
            if(player.StatTable.id == _index) {
                return player;
			}
		}
        return null;
	}

    public int FindIndex(int _index)
	{
        foreach(PlayerController player in m_listPlayers){
            if(player.StatTable.id == _index) {
                return player.StatTable.id;
            }
		}
        return -1;
	}
}
