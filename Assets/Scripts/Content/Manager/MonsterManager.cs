using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 언제나 말하지만 땜빵
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

    // 현재 사용하고 있는 몬스터의 수를 알아낸다.
    private List<BehaviorTree>  m_listMonster = new List<BehaviorTree>();
    private List<TargetData>    m_listTargetData = new List<TargetData>();                 // 정보 전달용

    // 목표 표시 UI Test위해 임시로 생성
    private UI_Goal m_goal;

    private int m_allMonsterCount;     // 생성할 몬스터의 총 수
    private int m_killCount;           // 죽인 몬스터의 총 수

    public List<BehaviorTree>   ListMonster { get => m_listMonster; }

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

    void Update()
    {
        AttackUpdate();
        DieUpdate();
    }

    // InGameScene에서 호출합니다.
	public void Init()
	{
        m_goal = Managers.UI.Root.GetComponentInChildren<UI_Goal>();
    }


    [ContextMenu("TestSpawn")]
    public void TestSpawn()
	{
        for (int i = 0; i < 100; ++i)
        {
            Managers.Resource.Instantiate("Monster", transform);
        }

    }


	// TODO : Server
	public void Damege(int p_id, int p_attack, Vector3 p_force)
	{
        m_listTargetData.Add(new TargetData(p_id, p_attack, p_force));
    }

    public void Damege(List<TargetData> p_listTargetData)
    {
        foreach(TargetData data in p_listTargetData) {
            m_listTargetData.Add(data);
        }
    }

    // Attack기록이 있는 녀석들에게 대미지를 입힌다.
	private void AttackUpdate()
	{
        if(m_listTargetData.Count == 0) {
            return;
		}

		foreach(TargetData data in m_listTargetData) {
            //Debug.Log(data.id);
            //m_listMonster[data.id].Stat.Hp -= data.attack;
		}

        m_listTargetData.Clear();

    }

    // 아직은 굳이?
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
}
