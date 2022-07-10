using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataManager))] 
public class CubeGenerateButton : Editor 
{ 
	public override void OnInspectorGUI() 
	{
		DataManager generator = (DataManager)target;
		if (GUILayout.Button("CreateScript")) {
			generator.CreateScript();
		}
		if (GUILayout.Button("LoadData")) {
			generator.LoadData();
		}
		if (GUILayout.Button("ClearData")) {
			generator.ClearData();
		}

		base.OnInspectorGUI();

	} 
}