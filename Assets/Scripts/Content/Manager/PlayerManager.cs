using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class PlayerManager : MonoBehaviour
{
    public Transform                m_spawnPoint = null;

    private List<PlayerController>  m_listPlayers = new List<PlayerController>();
    private PlayerController        m_mainPlayer = null;

    // 이름 추천 받음
    public List<PlayerController> List { get => m_listPlayers; }

    public PlayerController MainPlayer { get => m_mainPlayer; }

    public Vector3 SpanwPoint { get => m_spawnPoint.position; }


    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            m_mainPlayer.Demege(10);
            m_mainPlayer.PlayerHPBar.HP= m_mainPlayer.Stat.hp;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            m_mainPlayer.Recover(10);
            m_mainPlayer.PlayerHPBar.HP = m_mainPlayer.Stat.hp;
        }

        // 업데이트
        m_mainPlayer?.OnUpdate();
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
        PlayerController l_player = Managers.Resource.Instantiate(Path.Player, transform).GetComponent<PlayerController>();
        l_player.transform.position = _position;        // 좌표 반영
        l_player.Stat.id = m_listPlayers.Count;         // 아이디 발급
        l_player.Stat = _stat;                          // 스텟 반영
        m_listPlayers.Add(l_player);                    // 매니저 추가

        // 메인 플레이어 설정
        if (m_mainPlayer == null) {
            m_mainPlayer = l_player;
        }

        // 플레이어 HP바 생성
        UI_PlayerHPBar l_playerHPBar = Managers.UI.ShowSceneUI<UI_PlayerHPBar>("UI_PlayerHPBar");
        l_playerHPBar.HP = l_player.Stat.hp;
        l_playerHPBar.MaxHP = l_player.Stat.maxHp;
        Managers.UI.SetCanvas(l_playerHPBar.gameObject, false);
        l_player.PlayerHPBar = l_playerHPBar;

        return l_player;
    }

    // 초기화 작업을 한다.
    public void Init()
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
    public void Demege(int p_id, int p_attack, Vector3 p_force)
	{
        //m_listPlayers[p_id].Demege(p_attack);
        //m_listPlayers[p_id].Rigid.AddForce(p_force);
    }

    public void Demege(PlayerController p_player, int p_attack, Vector3 p_force)
	{
        //// 위에 있는 함수 호출
        //Demege(p_player.ID, p_attack, p_force);
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(m_spawnPoint.position, Vector3.one * 3.0f);
	}

}
