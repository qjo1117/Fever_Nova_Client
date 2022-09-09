using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System;

// MyAnimator Component���� LoopId�� ���� �Ӽ��ֵ��� �ϱ����� Editor Script
[CustomEditor(typeof(MyAnimator))]
public class MyAnimatorEditor : Editor
{
    private MyAnimator m_myAni;                                                     // Custom Editor�� target�� �Ǵ� MyAnimator ������Ʈ
    private List<ChildAnimatorState> m_allState = new List<ChildAnimatorState>();   // target�� �Ǵ� MyAnimator ������Ʈ�� ��� State����

    public Define.Player_AniState m_playerAniState;                                 // Loop Time�� �����ϰ���� Player Animation State��
    public AI.Enemy_AniState m_enemyAniState;                                       // Loop Time�� �����ϰ���� Enemy Animation State��


    private void OnEnable()
    {
        m_myAni = target as MyAnimator;
    }

    public override void OnInspectorGUI()
    {   
        base.DrawDefaultInspector();

        GUIStyle l_labelStyle = new GUIStyle();
        l_labelStyle.fontStyle = FontStyle.Bold;
        l_labelStyle.normal.textColor = Color.white;

        GUILayout.Label("[Loop Time ����]", l_labelStyle);
        switch(m_myAni.SubjectType)
        {
            case Define.SpawnType.Monster:
                m_enemyAniState = (AI.Enemy_AniState)EditorGUILayout.EnumPopup("Loop Time Turn On / Off", m_enemyAniState);
                break;

            case Define.SpawnType.Player:
                m_playerAniState = (Define.Player_AniState)EditorGUILayout.EnumPopup("Loop Time Turn On / Off", m_playerAniState);
                
                break;
        }

        if(GUILayout.Button("LoopTime Turn On"))
        {
            switch (m_myAni.SubjectType)
            {
                case Define.SpawnType.Monster:
                    SetAnimationLoop(m_enemyAniState, true);
                    break;

                case Define.SpawnType.Player:
                    SetAnimationLoop(m_playerAniState,true);
                    break;
            }
        }
        if(GUILayout.Button("LoopTime Turn Off"))
        {
            switch (m_myAni.SubjectType)
            {
                case Define.SpawnType.Monster:
                    SetAnimationLoop(m_enemyAniState, false);
                    break;

                case Define.SpawnType.Player:
                    SetAnimationLoop(m_playerAniState, false);
                    break;
            }
        }
    }

    // ���ϴ� Ư�� State�� LoopTime �����ϴ� �Լ�
    // (MyAnimator�� SetAnimationLoop()�Լ��� ����)
    public void SetAnimationLoop<T>(T _animationState, bool _flag) where T : System.Enum
    {
        ChildAnimatorStatesUpdate();

        string l_stateName = _animationState.ToString();

        if (l_stateName.Contains("_"))
        {
            string[] l_splits = l_stateName.Split('_');
            l_stateName = l_splits[l_splits.Length - 1];
        }

        foreach (ChildAnimatorState childState in m_allState)
        {
            if (childState.state.name == l_stateName)
            {
                string l_assetPath = AssetDatabase.GetAssetPath(childState.state.motion.GetInstanceID());
                ModelImporter l_modelImporter = ModelImporter.GetAtPath(l_assetPath) as ModelImporter;
                ModelImporterClipAnimation[] l_clipAni = l_modelImporter.clipAnimations;

                foreach (ModelImporterClipAnimation item in l_clipAni)
                {
                    if (item.name == childState.state.motion.name)
                    {
                        item.loopTime = _flag;
                        l_modelImporter.clipAnimations = l_clipAni;
                        l_modelImporter.SaveAndReimport();
                        Debug.Log($"{item.name} loop Time {item.loopTime}");
                        break;
                    }
                }
                break;
            }
        }
    }

    // target���� �ϴ� Animator�� ��� State ������ �����ϴ� �Լ�
    // (MyAnimator�� �ִ� AllAnimationStateInfoUpdate() �� ����)
    public void ChildAnimatorStatesUpdate()
    {
        if (m_allState.Count > 0)
        {
            m_allState.Clear();
        }

        AnimatorController l_aniController;
        ChildAnimatorState[] l_states;

        l_aniController = m_myAni.FindAnimator().runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] l_layers = l_aniController.layers;


        foreach (AnimatorControllerLayer layer in l_layers)
        {
            l_states = layer.stateMachine.states;

            foreach (ChildAnimatorState state in l_states)
            {
                m_allState.Add(state);
            }
        }
    }

}
