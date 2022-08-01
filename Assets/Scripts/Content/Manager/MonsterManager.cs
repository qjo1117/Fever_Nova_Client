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

public class MonsterManager : MonoBehaviour
{
    #region ����

    // --------- ���� ���� ���� ---------
    [SerializeField]
    private List<AI_Enemy> m_listMonster = new List<AI_Enemy>();
    private List<TargetData> m_listTargetData = new List<TargetData>();                 // ���� ���޿�

    // --------- UI Test ---------
    private UI_Goal m_goal;

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
        foreach (Spawner spawner in m_listSpawner)
        {
            spawner.TransformToPosition();
        }
    }
}