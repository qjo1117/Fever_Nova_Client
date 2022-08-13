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
	private DataPrefabTable m_prefabTable = new DataPrefabTable();

	[SerializeField]
	private DataSkillTable m_skillTable = new DataSkillTable();
	[SerializeField]
	private DataMonsterStatTable m_monsterStat = new DataMonsterStatTable();
	[SerializeField]
	private DataPlayerStatTable m_playerStat = new DataPlayerStatTable();

	[SerializeField]
	private DataMeleeSkillTable m_meleeSkill = new DataMeleeSkillTable();
	[SerializeField]
	private DataFieldSkillTable m_fieldSkill = new DataFieldSkillTable();
	[SerializeField]
	private DataRushSkillTable m_rushSkill = new DataRushSkillTable();
	#endregion

	#region Property
	public DataPrefabTable PrefabTable { get => m_prefabTable; }
	public DataSkillTable SkillTable { get => m_skillTable; }
	public DataMonsterStatTable MonsterStat { get => m_monsterStat; }
	public DataPlayerStatTable PlayerStat { get => m_playerStat; }
	public DataMeleeSkillTable MeleeSkillTable { get => m_meleeSkill; }
	public DataFieldSkillTable FieldSkillTable { get => m_fieldSkill; }
	public DataRushSkillTable RushSkillTable { get => m_rushSkill; }
	#endregion

	[ContextMenu("CreateScript")]
	public void CreateScript()
	{
		DataSystem.CreateGenerateScript("Data_Table_Prefab.Ver.0.5", "PrefabTable");

		DataSystem.CreateGenerateScript("SkillTable.ver.0.3", "SkillTable");
		DataSystem.CreateGenerateScript("Data_Table_Melee.Ver.0.5", "MeleeSkillTable");
		DataSystem.CreateGenerateScript("Data_Table_Field.Ver.0.5", "FieldSkillTable");
		DataSystem.CreateGenerateScript("Data_Table_PC.Ver.0.5", "PlayerStatTable");
		DataSystem.CreateGenerateScript("Data_Table_Rush.Ver.0.5", "RushSkillTable");
		DataSystem.CreateGenerateScript("Data_Table_MonsterStat.Ver.0.5", "MonsterStatTable");

	}

	[ContextMenu("LoadData")]
	public void LoadData()
	{
		ClearData();

		m_prefabTable.DataParsing();

		m_skillTable.DataParsing();
		m_monsterStat.DataParsing();
		m_playerStat.DataParsing();
		m_meleeSkill.DataParsing();
		m_fieldSkill.DataParsing();
		m_rushSkill.DataParsing();
	}

	[ContextMenu("Clear")]
	public void ClearData()
	{
		m_prefabTable.listPrefabTable.Clear();

		m_skillTable.listSkillTable.Clear();
		m_monsterStat.listMonsterStatTable.Clear();
		m_playerStat.listPlayerStatTable.Clear();
		m_meleeSkill.listMeleeSkillTable.Clear();
		m_fieldSkill.listFieldSkillTable.Clear();
		m_rushSkill.listRushSkillTable.Clear();
	}

}
