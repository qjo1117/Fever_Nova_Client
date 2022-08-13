using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Goal : UI_Scene
{
    #region UI컴포넌트_ENUM
    enum Images
    {
        Background                              // 목표 표시 UI의 뒷배경
    }

    enum Texts
    {
        GoalText                                // 목표 Text
    }
    #endregion

    #region 변수
    private int m_allMonsterCount = 0;          // 해당 스테이지에 생성될 Monster의 총 수 
    private int m_monsterKillCount = 0;         // 해당 스테이지에 죽인 Monster의 총 수 (외부에서 Monster Destroy할떄마다 같이 증가됨)
    #endregion

    #region 프로퍼티
    // MonsterManager의 AllMonsterCount프로퍼티를 통해 값을 셋팅하면
    // UI_Goal의 AllMonsterCount 프로퍼티도 호출되어 현재 목표 표시 텍스트를 갱신하도록함.
    public int AllMonsterCount
    {
        get 
        { 
            return m_allMonsterCount; 
        }
        set
        {
            m_allMonsterCount = value;
            UpdateGoalText();
        }
    }

    // MonsterManager의 MonsterKillCount프로퍼티를 통해 값을 셋팅하면
    // UI_Goal의 AllMonsterCount 프로퍼티도 호출되어 현재 목표 표시 텍스트를 갱신하도록함.
    public int MonsterKillCount
    {
        get
        {
            return m_monsterKillCount;
        }
        set
        {
            if (0 <= value && value <= m_allMonsterCount) 
            {
                m_monsterKillCount = value;
                UpdateGoalText();
            }
        }
    }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        // 임시적으로 Test위해 추가
        // (현재 몬스터 생성을 Ingame Scene의 Init에서 하는데, UI의 Image,Text 컴포넌트를 Bind하는 Init함수가 Ingame Scene Init함수가 리턴되고 나서
        // 실행되므로 지금상황에서는 UI 컴포넌트들이 Bind되기전 RemainCount에 접근하게 된다.
        //  => 해당 문제는 후에, Monster Controller에 Spawn함수를 만들어서 거기서 몹을 스폰하게 될텐데 그 경우에는 UI 컴포넌트들의 Bind가 완료되어있으니 해결될것)

        Managers.Game.Monster.AllMonsterCount = Managers.Game.MonsterCount;
    }

    // 목표 UI 상태 갱신하는 함수
    private void UpdateGoalText()
    {
        Get<Text>((int)Texts.GoalText).text = $"남은 몬스터\n{m_allMonsterCount - m_monsterKillCount} / {m_allMonsterCount}";
    }

}
