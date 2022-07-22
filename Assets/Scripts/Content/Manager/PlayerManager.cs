using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerController>  m_listPlayers = new List<PlayerController>();
    private PlayerController        m_mainPlayer = null;

    // 플레이어 체력바 업데이트 함수 호출시 사용
    private UI_PlayerHPBar m_playerHPBar = null;

    // 이름 추천 받음
    public List<PlayerController>   List { get => m_listPlayers; }

    public PlayerController MainPlayer { get => m_mainPlayer; }


    void Update()
    {
        // 플레이어 체력바 테스트용 데미지 주기
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            m_mainPlayer.Demege(10);
            m_playerHPBar.HpBarUpdate();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            m_mainPlayer.Recover(10);
            m_playerHPBar.HpBarUpdate();
        }
    }

    // 초기화 작업을 한다.
    public void Init()
    {
        PlayerController l_player;
        l_player = FindObjectOfType<PlayerController>();
        m_mainPlayer = l_player;

        // 플레이어 hp 바 생성
        m_playerHPBar = Managers.UI.ShowSceneUI<UI_PlayerHPBar>("UI_PlayerHPBar");
        Managers.UI.SetCanvas(m_playerHPBar.gameObject, false);

        //// TODO : 서버일 경우 플레이어접속을 확인해서 추가하자.

        //// 해당하는 플레이어 프리팹을 생성한다.
        //// 다중 접속으로 변경할 경우 Pool등록하면 된다.
        //PlayerController player = Managers.Resource.NewPrefab("Player", transform).GetComponent<PlayerController>();

        //// 리스트에 넣어주고 처음 생성한 녀석은 메인으로 등록시킨다.
        //m_listPlayers.Add(player);
        //m_mainPlayer = player;

        //// 아이디 발급을 해준다.

        //// 순회하면서 초기화 작업을 해준다.
        //int size = m_listPlayers.Count;
        //for (int i = 0; i < size; ++i) {
        //    // 살짝 수정하는게 나을꺼같긴한데 일단 판별을 이렇게
        //    if(m_mainPlayer == m_listPlayers[i]) {
        //        m_listPlayers[i].Init(i, true);
        //    }
        //    else {
        //        m_listPlayers[i].Init(i, false);
        //    }
        //}
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



}
