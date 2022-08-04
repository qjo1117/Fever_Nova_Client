using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ÀÏ´Ü ÀÎ½ºÆåÅÍ¿¡¼­ º¸¿©Áà¾ßÇÑ´Ù°í ÇÏ´Ñ±ñ ³ÀµÐ´Ù.
// ³ªÁß¿¡ Manager·Î »©µÑ»ý°¢
public class DataManager : MonoBehaviour
{

	#region Variable
	[SerializeField]
	private DataSkillTable m_skillTable = new DataSkillTable();
	[SerializeField]
	private DataMonsterStatTable m_monsterStat = new DataMonsterStatTable();
	#endregion

	#region Property
	public DataSkillTable SkillTable { get => m_skillTable; }
	public DataMonsterStatTable MonsterStat { get => m_monsterStat; }
	#endregion

	[ContextMenu("CreateScript")]
	public void CreateScript()
	{
		DataSystem.CreateGenerateScript("SkillTable.ver.0.3", "SkillTable");
		DataSystem.CreateGenerateScript("Data_Table.Ver0.4", "MonsterStatTable");
	}

	[ContextMenu("LoadData")]
	public void LoadData()
	{
		ClearData();

		m_skillTable.DataParsing();
		m_monsterStat.DataParsing();
	}

	[ContextMenu("Clear")]
	public void ClearData()
	{
		m_skillTable.listSkillTable.Clear();
		m_monsterStat.listMonsterStatTable.Clear();
	}

}
