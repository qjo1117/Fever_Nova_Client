using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 땜빵
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

public class MonsterGroup 
{
    private Spawn m_spawn = null;
    private List<int> m_listID = new List<int>();

    public Spawn Spawn { get => m_spawn; }
    public List<int> MonsterID { get => m_listID; }
}

public class MonsterManager : MonoBehaviour
{
    private Dictionary<string, MonsterGroup> m_dicGroup = new Dictionary<string, MonsterGroup>();

    private List<MonsterController> m_listMonster = new List<MonsterController>();

    private List<TargetData> m_listTargetData = new List<TargetData>();             // 정보 전달용

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        AttackUpdate();
        DieUpdate();
    }

	public void Init()
	{
        MonsterController monster = Managers.Resource.NewPrefab("Monster").GetComponent<MonsterController>();
        monster.ID = 0;
        m_listMonster.Add(monster);
    }


	// TODO : Server
	public void Attack(int p_id, int p_attack, Vector3 p_force)
	{
        m_listTargetData.Add(new TargetData(p_id, p_attack, p_force));
    }

    public void Attack(List<TargetData> p_listTargetData)
    {
        foreach(TargetData data in p_listTargetData) {
            m_listTargetData.Add(data);
        }
    }

    // Attack기록이 있는 녀석들에게 대미지를 입힌다.
	private void AttackUpdate()
	{
		foreach(TargetData data in m_listTargetData) {
            Debug.Log(data.id);
            m_listMonster[data.id].Hp -= data.attack;

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
