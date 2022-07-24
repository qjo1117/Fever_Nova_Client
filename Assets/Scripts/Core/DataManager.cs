using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 일단 인스펙터에서 보여줘야한다고 하닌깐 냅둔다.
// 나중에 Manager로 빼둘생각
public class DataManager : MonoBehaviour
{
	public string[] m_filePath;

	#region Variable
	[SerializeField]
	private DataSkillTable m_skillTable = new DataSkillTable();
	[SerializeField]
	private List<MonsterStat> m_monsterStat = new List<MonsterStat>();
	#endregion

	#region Property
	public DataSkillTable SkillTable { get => m_skillTable; }
	public List<MonsterStat> MonsterStat { get => m_monsterStat; }
	#endregion

	[ContextMenu("CreateScript")]
	public void CreateScript()
	{
		DataSystem.CreateGenerateScript("SkillTable.ver.0.3", "SkillTable");
	}

	[ContextMenu("LoadData")]
	public void LoadData()
	{
		m_skillTable.DataParsing();
	}

	[ContextMenu("Clear")]
	public void ClearData()
	{
		m_skillTable.listSkillTable.Clear();
	}

}
