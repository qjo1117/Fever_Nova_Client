using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ------------------------------------------ 
 *          스포너에게 필요한 것
 * 
 * 
------------------------------------------ */

// 스폰될 녀석들의 정보를 들고 있는다.
public struct SpawnerInfo 
{
    public int      Index;            // 몬스터 스텟 인덱스
    public Vector3  Position;         // 스폰될 위치 (기획자쪽에서 직접하기로 해서)
}

public class Spawner : MonoBehaviour 
{
    // --------- 기획자 전용 UI ---------
    public Transform                            m_parentMonsterPoint = null; 

    // --------- Spawn에 관련된 변수 ---------
    [SerializeField]
    private List<SpawnerInfo>                   m_listSpawnerInfo = new List<SpawnerInfo>();
	[SerializeField]
    private SpawnerTrigger                      m_trigger = null;
    [SerializeField]
    private Transform                           m_monsters = null;

    public void Start()
    {
        // 트리거가 없을 경우 맵핑해본다.
        if (m_trigger.gameObject.IsValid() == false) {
            m_trigger = Util.FindChild<SpawnerTrigger>(gameObject);
        }
        m_trigger?.SetSpawner(this);

        // 없을 경우 맵핑
        if (m_monsters == null) {
            m_monsters = Util.FindChild<Transform>(gameObject, "Monsters");
        }

        RegisterMonster();
    }

    public void Update()
    {

        
    }

    // 총 3번을 걸쳐서 진행된다.
    // 1. Parent에 접근해서 Spawner 컴포넌트를 가져온 후
    // 2. SpawnerPoint에서 실제로 필요한 정보만 추출한다. => SpawnInfo
    // 3. UI적으로 보여주던 Transform을 모두 삭제한다.
    // 설명 : 게임 시작을 했을때 Trasnform => Vec으로 바꿔야한다.
	public void TransformToPosition()
	{
        List<MonsterSpawnPoint> l_listMonsterPoint = new List<MonsterSpawnPoint>();
        // 현재 Transform에 있는 SpawnerPoint를 가져온다.
        int l_size = m_parentMonsterPoint.childCount;
        for (int i = 0; i < l_size; ++i) {
            MonsterSpawnPoint l_spawnInfo = null;
            if(m_parentMonsterPoint.GetChild(i).TryGetComponent(out l_spawnInfo) == true) {
                l_listMonsterPoint.Add(l_spawnInfo);
            }
        }

        // 데이터 이전 이후 삭제
        l_size = l_listMonsterPoint.Count;
        for (int i = 0; i < l_size; ++i) {
            m_listSpawnerInfo.Add(new SpawnerInfo { Index =  l_listMonsterPoint[i].m_index, Position =  l_listMonsterPoint[i].transform.position });
        }

        // Transform을 표시하던 오브젝트를 전부 삭제시킨다.
        l_size = m_parentMonsterPoint.childCount;
        for (int i = 0; i < l_size; ++i) {
            Managers.Resource.DelPrefab(m_parentMonsterPoint.GetChild(i).gameObject);
        }
        Managers.Resource.DelPrefab(m_parentMonsterPoint.gameObject);

        // 필요가 없으므로 삭제한다.
        l_listMonsterPoint.Clear();
    }
    

    // 플레이어가 트리거에 접촉했을때 정보를 갱신하며 몬스터를 생성한다.
	public void StartSpawn()
	{
        // 정보가 아예 없으면 취소
        if(m_listSpawnerInfo.Count < 0) {
            return;
		}

        // 해당 인덱스에 맞게 스폰을 시작한다.
        int l_size = m_listSpawnerInfo.Count;
        for(int i = 0; i < l_size; ++i) {
			var l_monster = Managers.Game.Monster.Spawn(m_listSpawnerInfo[i].Index);
            l_monster.transform.position = m_listSpawnerInfo[i].Position;
            l_monster.Init();
        }

        l_size = m_monsters.transform.childCount;
        for (int i = 0; i < l_size; ++i) {
            BehaviorTree l_monster = null;
            if (m_monsters.GetChild(i).TryGetComponent(out l_monster) == true) {
                Managers.Game.Monster.Register(l_monster);
            }
            l_monster.Init();
        }


        // TODO : 해당 UI에 전달해야하는 것
        // 목표 숫자
    }


    private void RegisterMonster()
	{
        int l_size = m_monsters.childCount;
        for (int i = 0; i < l_size; ++i){
            m_monsters.GetChild(i).gameObject.SetActive(false);
        }
    }


}
