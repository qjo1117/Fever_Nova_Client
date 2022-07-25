using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterManager))] 
public class MonsterManagerEditor : Editor 
{ 
	public override void OnInspectorGUI() 
	{
		MonsterManager generator = (MonsterManager)target;

		if (GUILayout.Button("Spanwers Data Save")) {
			// TODO : MonsterManager -> Spawn To Json
			generator.SpanwerSave();
		}

		base.OnInspectorGUI();

	} 
}