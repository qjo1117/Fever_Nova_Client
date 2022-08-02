using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ------------------------------------------ 
 *          �����ʿ��� �ʿ��� ��
 * 
 * 
------------------------------------------ */

// ������ �༮���� ������ ��� �ִ´�.
[System.Serializable]
public struct SpawnerInfo
{
    public int Index;                   // ���� ���� �ε���
    public Vector3 Position;            // ������ ��ġ (��ȹ���ʿ��� �����ϱ�� �ؼ�)
}

public class Spawner : MonoBehaviour
{

    #region Variable
    // --------- UI ---------
    public Transform m_parentMonsterPoint = null;

    // --------- Spawn�� ���õ� ���� ---------
    [SerializeField]
    private List<SpawnerInfo> m_listSpawnerInfo = new List<SpawnerInfo>();
    [SerializeField]
    private List<List<Vector3>> m_listPatrolInfo = new List<List<Vector3>>();
    [SerializeField]
    private SpawnerTrigger m_trigger = null;
    [SerializeField]
    private Transform m_monsters = null;

	#endregion

	#region Property
	public SpawnerTrigger Trigger { get => m_trigger; }
	public List<SpawnerInfo> ListSpawnerInfo { get => m_listSpawnerInfo; }

	#endregion

	public void Start()
    {
        // Ʈ���Ű� ���� ��� �����غ���.
        if (m_trigger.gameObject.IsValid() == false)
        {
            m_trigger = Util.FindChild<SpawnerTrigger>(gameObject);
        }
        m_trigger?.SetSpawner(this);

        // ���� ��� ����
        if (m_monsters == null)
        {
            m_monsters = Util.FindChild<Transform>(gameObject, "Monsters");
        }

        RegisterMonster();
    }

    public void Update()
    {


    }

    // �� 3���� ���ļ� ����ȴ�.
    // 1. Parent�� �����ؼ� Spawner ������Ʈ�� ������ ��
    // 2. SpawnerPoint���� ������ �ʿ��� ������ �����Ѵ�. => SpawnInfo
    // 3. UI������ �����ִ� Transform�� ��� �����Ѵ�.
    // ���� : ���� ������ ������ Trasnform => Vec���� �ٲ���Ѵ�.
    public void TransformToPosition()
    {
        List<MonsterSpawnPoint> l_listMonsterPoint = new List<MonsterSpawnPoint>();
        // ���� Transform�� �ִ� SpawnerPoint�� �����´�.
        int l_spawnSize = m_parentMonsterPoint.childCount;
        for (int i = 0; i < l_spawnSize; ++i)
        {
            MonsterSpawnPoint l_spawnInfo = null;
            if (m_parentMonsterPoint.GetChild(i).TryGetComponent(out l_spawnInfo) == true)
            {
                l_listMonsterPoint.Add(l_spawnInfo);

                m_listPatrolInfo.Add(new List<Vector3>());
                int l_patrolSize = l_spawnInfo.transform.childCount;
                for (int j = 0; j < l_patrolSize; j++)
                {
                    MonsterPatrolPoint l_patrolInfo = null;
                    if (l_spawnInfo.transform.GetChild(j).TryGetComponent(out l_patrolInfo) == true)
                    {
                        m_listPatrolInfo[i].Add(l_patrolInfo.transform.position);
                    }
                }

            }
        }

        // ������ ���� ���� ����
        l_spawnSize = l_listMonsterPoint.Count;
        for (int i = 0; i < l_spawnSize; ++i)
        {
            m_listSpawnerInfo.Add(new SpawnerInfo { Index = l_listMonsterPoint[i].m_index, Position = l_listMonsterPoint[i].transform.position });
        }

        // Transform�� ǥ���ϴ� ������Ʈ�� ���� ������Ų��.
        l_spawnSize = m_parentMonsterPoint.childCount;
        for (int i = 0; i < l_spawnSize; ++i)
        {
            int l_patrolSize = m_parentMonsterPoint.GetChild(i).childCount;
            for (int j = 0; j < l_patrolSize; ++j)
            {
                Managers.Resource.DelPrefab(m_parentMonsterPoint.GetChild(i).GetChild(j).gameObject);
            }
            Managers.Resource.DelPrefab(m_parentMonsterPoint.GetChild(i).gameObject);
        }
        Managers.Resource.DelPrefab(m_parentMonsterPoint.gameObject);

        // �ʿ䰡 �����Ƿ� �����Ѵ�.
        l_listMonsterPoint.Clear();
    }


    // �÷��̾ Ʈ���ſ� ���������� ������ �����ϸ� ���͸� �����Ѵ�.
    public void StartSpawn()
    {
        if (m_listSpawnerInfo.Count < 0) {
            return;
        }

        // 해당 스폰에 대한 정보를 기반으로 스포너 구성
        int l_spawnSize = m_listSpawnerInfo.Count;
        for (int i = 0; i < l_spawnSize; ++i) {
            // Spawner에서 지정한 몬스터를 소환하기 위해 인덱스로 구별
            AI_Enemy l_monster = Managers.Game.Monster.Spawn(m_listSpawnerInfo[i].Index);
            l_monster.transform.position = m_listSpawnerInfo[i].Position;
            
            // Patrol에 관련되어 맵핑
            l_monster.AddPatrolPoint(m_listSpawnerInfo[i].Position);
            int l_partolSize = m_listPatrolInfo[i].Count;
            for (int j = 0; j < l_partolSize; j++) {
                l_monster.AddPatrolPoint(m_listPatrolInfo[i][j]);
            }
            l_monster.Init();
        }

        // 만약 몬스터가 이미 등록되어 있다면 MonsterManager에게 등록하는 것만 해준다.
        l_spawnSize = m_monsters.transform.childCount;
        for (int i = 0; i < l_spawnSize; ++i) {
            AI_Enemy l_monster = null;
            if (m_monsters.GetChild(i).TryGetComponent(out l_monster) == true) {
                Managers.Game.Monster.Register(l_monster);
            }
        }


    }

    
    private void RegisterMonster()
    {
        int l_size = m_monsters.childCount;
        for (int i = 0; i < l_size; ++i) {
            m_monsters.GetChild(i).gameObject.SetActive(false);
        }
    }



}
