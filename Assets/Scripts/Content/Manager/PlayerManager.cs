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
    #endregion

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            m_mainPlayer.Damage(10);
            m_mainPlayer.PlayerHPBar.HP= m_mainPlayer.Stat.hp;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            m_mainPlayer.Recover(10);
            m_mainPlayer.PlayerHPBar.HP = m_mainPlayer.Stat.hp;
        }

        // 업데이트
        m_mainPlayer?.OnUpdate();
    }

	private void FixedUpdate()
	{
        MoveUpdate();

    }

	private void MoveUpdate()
	{
		//bool l_isMove = true;
  //      float x = 0, y = 0;

		//if (l_isMove == true) {
  //          foreach(PlayerController player in m_listPlayers) {
  //              Vector3 l_position = player.transform.position;
  //              Managers.Network.Session.Write((int)E_PROTOCOL.MOVE, l_position.x, l_position.z);
  //          }
		//}
	}

    private void LateUpdate()
	{
        m_mainPlayer?.OnLateUpdate();
	}

	public PlayerController FindPlayer(int _id)
	{
        foreach(PlayerController l_player in m_listPlayers) {
            // 아이디 검출을 인스턴스 아이디로 검출한다.
            if (l_player.gameObject.GetInstanceID() == _id) {
                return l_player;
			}
		}

        return null;
	}

    // 스폰을 시킨다.
    public PlayerController Spawn(Vector3 _position, PlayerStat _stat)
	{
        PlayerController l_player = Managers.Resource.Instantiate(Path.Player, transform).GetOrAddComponent<PlayerController>();
        l_player.transform.position = _position;        // 좌표 반영
        l_player.Stat.id = m_listPlayers.Count;         // 아이디 발급
        l_player.Stat = _stat;                          // 스텟 반영
        m_listPlayers.Add(l_player);                    // 매니저 추가

        // 메인 플레이어 설정
        if (m_mainPlayer == null) {
            m_mainPlayer = l_player;
            
            CameraController l_camera = GameObject.FindObjectOfType<CameraController>();
            l_camera.SetPlayer(l_player.gameObject);

            m_mainPlayer.ExplosionRadius = m_explosionRange;
            m_mainPlayer.ExplosionJumpRadius = m_jumpRange;
        }

        // 플레이어 HP바 생성
        UI_PlayerHPBar l_playerHPBar = Managers.UI.ShowSceneUI<UI_PlayerHPBar>("UI_PlayerHPBar");
        l_playerHPBar.HP = l_player.Stat.hp;
        l_playerHPBar.MaxHP = l_player.Stat.maxHp;
        Managers.UI.SetCanvas(l_playerHPBar.gameObject, false);
        l_player.PlayerHPBar = l_playerHPBar;

        l_player.Init();

        return l_player;
    }

    // 초기화 작업을 한다.
    public void Init()
    {
        Managers.Network.Register(E_PROTOCOL.INUSER, InuserProcess);
    }

    public void InuserProcess()
	{
        int l_id = 0;
        Managers.Network.Session.GetInData(out l_id);
        Managers.Game.Player.Spawn(Managers.Game.Player.SpanwPoint, new PlayerStat { id = l_id, name = "Sample_Player" });
    }

    public void OnUpdate()
    {

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
        //m_listPlayers[p_id].Damage(p_attack);
        //m_listPlayers[p_id].Rigid.AddForce(p_force);
    }

    public void Damage(PlayerController p_player, int p_attack, Vector3 p_force)
	{
        //// 위에 있는 함수 호출
        //Damage(p_player.ID, p_attack, p_force);
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(m_spawnPoint.position, Vector3.one * 3.0f);
	}

    public PlayerController At(int _index)
	{
        foreach(PlayerController player in m_listPlayers) {
            if(player.Stat.id == _index) {
                return player;
			}
		}
        return null;
	}

}
