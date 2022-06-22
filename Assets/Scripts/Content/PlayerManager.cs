using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerController> m_listPlayers = new List<PlayerController>();
    private PlayerController m_mainPlayer = null;

    void Update()
    {

    }

    // 초기화 작업을 한다.
    public void Init()
    {
        // TODO : 서버일 경우 플레이어접속을 확인해서 추가하자.

        // 해당하는 플레이어 프리팹을 생성한다.
        // 다중 접속으로 변경할 경우 Pool등록하면 된다.
        PlayerController player = Managers.Resource.NewPrefab("Player", transform).GetComponent<PlayerController>();

        // 리스트에 넣어주고 처음 생성한 녀석은 메인으로 등록시킨다.
        m_listPlayers.Add(player);
        m_mainPlayer = player;

        // 아이디 발급을 해준다.

        // 순회하면서 초기화 작업을 해준다.
        int size = m_listPlayers.Count;
        for (int i = 0; i < size; ++i) {
            // 살짝 수정하는게 나을꺼같긴한데 일단 판별을 이렇게
            if(m_mainPlayer == m_listPlayers[i]) {
                m_listPlayers[i].Init(i, true);
            }
            else {
                m_listPlayers[i].Init(i, false);
            }
        }
    }

    public void Clear()
	{
        // 만약 하이라키에 플레이어가 생존해 있으면 삭제 시켜버린다.
        foreach(PlayerController player in m_listPlayers) {
            Managers.Resource.DelPrefab(player.gameObject);
		}
        // 초기화
        m_listPlayers.Clear();

    }




}
