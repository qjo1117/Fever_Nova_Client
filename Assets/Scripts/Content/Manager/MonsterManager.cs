using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �������� ����
public class TargetData
{
    public TargetData(int p_id, int p_attack, Vector3 p_force)
    {
        id = p_id;
        attack = p_attack;
        force = p_force;
    }
    public int id = -1;
    public int attack = 0;
    public Vector3 force = Vector3.zero;
}

// Spawner Data
[System.Serializable]
public class SpawnerJsonInfo
{
    // 스포너의 위치
    public Vector3 Position = Vector3.zero;
    // 트리거 위치, 크기
    public Vector3 TriggerPosition = Vector3.zero;
    public Vector3 TriggerSize = Vector3.zero;
    // 스포너가 가지고 있는 몬스터 리스트
    public List<SpawnerInfo> ListSpawnerInfo = new List<SpawnerInfo>();
}

[System.Serializable]
public class SpawnerJson
{
    public List<SpawnerJsonInfo> ListJson = new List<SpawnerJsonInfo>();
}

public class MonsterManager : MonoBehaviour
{
	#region 변수

	// --------- Monster Data ---------
	[SerializeField]
    private List<BehaviorTree>  m_listMonster = new List<BehaviorTree>();
    private List<TargetData>    m_listTargetData = new List<TargetData>();                 // 
    #region ����

    // --------- ���� ���� ���� ---------
    [SerializeField]
    private List<AI_Enemy> m_listMonster = new List<AI_Enemy>();
    private List<TargetData> m_listTargetData = new List<TargetData>();                 // ���� ���޿�

    // --------- UI Test ---------
    private UI_Goal m_goal;

    private int m_allMonsterCount;     // 
    private int m_killCount;           // 
    private int m_allMonsterCount;     // ������ ������ �� ��
    private int m_killCount;           // ���� ������ �� ��


    public int AllMonsterCount
    {
        get
        {
            return m_allMonsterCount;
        }
        set
        {
            m_allMonsterCount = value;
            m_goal.AllMonsterCount = m_allMonsterCount;
        }
    }

    public int MonsterKillCount
    {
        get
        {
            return m_killCount;
        }
        set
        {
            m_killCount = value;
            m_goal.MonsterKillCount = m_killCount;
        }
    }

    // --------- Spawner ���� ���� ---------
    public Transform            m_parentSpawner = null;
	[SerializeField]
    private List<Spawner>       m_listSpawner = new List<Spawner>();
    public Transform m_parentSpawner = null;
    [SerializeField]
    private List<Spawner> m_listSpawner = new List<Spawner>();

    #endregion

    void Update()
    {
        AttackUpdate();
        DieUpdate();
    }

    // InGameScene에서 호출하도록 한다.
    public void Init()
    {
        m_goal = Managers.UI.Root.GetComponentInChildren<UI_Goal>();
        // BehaviorTree 구성


        // 시작할때 스포너에 있는 데이터를 가져온다.
        InitSpawner();
    }




    [ContextMenu("TestSpawn")]
    public void TestSpawn()
    {
        for (int i = 0; i < 100; ++i)
        {
            Managers.Resource.Instantiate("Monster", transform);
        }

    }


    public AI_Enemy Spawn(int _index)
    {
        // -------------------
        m_listMonster.Add(Managers.Resource.Instantiate("Monster", transform).GetOrAddComponent<AI_Enemy_01>());
        return m_listMonster[m_listMonster.Count - 1];
    }

    public void Register(AI_Enemy _monster)
    {
        m_listMonster.Add(_monster);
    }


    // TODO : Server
    public void Damege(int p_id, int p_attack, Vector3 p_force)
    {
        m_listTargetData.Add(new TargetData(p_id, p_attack, p_force));
    }

    public void Damege(List<TargetData> p_listTargetData)
    {
        foreach (TargetData data in p_listTargetData)
        {
            m_listTargetData.Add(data);
        }
    }


    private void AttackUpdate()
    {
        if (m_listTargetData.Count == 0)
        {
            return;
        }

        foreach (TargetData data in m_listTargetData)
        {
            //Debug.Log(data.id);
            //m_listMonster[data.id].Stat.Hp -= data.attack;
        }

        m_listTargetData.Clear();

    }

    private void DieUpdate()
    {
        //     List<MonsterController> listDie = new List<MonsterController>();
        //     foreach (MonsterController monster in m_listMonster) {
        //         if(monster.Hp <= 0) {
        //             listDie.Add(monster);
        //}
        //     }

        //     foreach (MonsterController monster in listDie) {
        //         m_listMonster.Remove(monster);
        //     }
    }

    private void InitSpawner()
    {
        int l_size = m_parentSpawner.childCount;
        for (int i = 0; i < l_size; ++i)
        {
            Spawner l_spawner = null;
            if (m_parentSpawner.GetChild(i).TryGetComponent(out l_spawner) == true)
            {
                m_listSpawner.Add(l_spawner);
            }
        }

        // Spanwer Info -> Transform To Position
        foreach (Spawner spawner in m_listSpawner) {
            spawner.TransformToPosition();
        }
    }

    #region SpanwerData / Save Load

    public void SpanwerSave()
	{
        SpawnerJson l_spawnerJson = new SpawnerJson();    

        // 현재 들고 있는 스포너의 데이터를 저장한다.
        foreach (Spawner spawner in m_listSpawner) {
            SpawnerJsonInfo info = new SpawnerJsonInfo();
            // 스포너의 위치
            info.Position = spawner.transform.position;

            // 트리거의 정보
            info.TriggerPosition = spawner.Trigger.transform.position;
            info.TriggerSize = spawner.Trigger.GetComponent<BoxCollider>().size;

            // 위치랑 인덱스만 있는 몬스터의 정보
            int l_size = spawner.ListSpawnerInfo.Count;
            List<SpawnerInfo> l_listSpawner = spawner.ListSpawnerInfo;
            for (int i = 0; i < l_size; ++i) {
                info.ListSpawnerInfo.Add(l_listSpawner[i]);
            }
            l_spawnerJson.ListJson.Add(info);
        }

        string l_json = JsonUtility.ToJson(l_spawnerJson);
        Debug.Log(l_json);
	}

	#endregion

}
}
