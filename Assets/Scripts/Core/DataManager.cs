using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ÀÏ´Ü ÀÎ½ºÆåÅÍ¿¡¼­ º¸¿©Áà¾ßÇÑ´Ù°í ÇÏ´Ñ±ñ ³ÀµÐ´Ù.
// ³ªÁß¿¡ Manager·Î »©µÑ»ý°¢
public class DataManager : MonoBehaviour
{
	public string[] m_filePath;

	public DataSkillTable m_skillTable = new DataSkillTable();
	public DataSkillTable SkillTable { get => m_skillTable; }

	[ContextMenu("CreateScript")]
	public void CreateScript()
	{
		DataSystem.CreateGenerateScript("Stage_01_SkillTable.ver.0.1", "SkillTable");
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
