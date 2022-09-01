using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Goal : UI_Scene
{
    #region UI������Ʈ_ENUM
    enum Images
    {
        Background                              // ��ǥ ǥ�� UI�� �޹��
    }

    enum Texts
    {
        GoalText                                // ��ǥ Text
    }
    #endregion

    #region ����
    private int m_allMonsterCount = 0;          // �ش� ���������� ������ Monster�� �� �� 
    private int m_monsterKillCount = 0;         // �ش� ���������� ���� Monster�� �� �� (�ܺο��� Monster Destroy�ҋ����� ���� ������)
    #endregion

    #region ������Ƽ
    // MonsterManager�� AllMonsterCount������Ƽ�� ���� ���� �����ϸ�
    // UI_Goal�� AllMonsterCount ������Ƽ�� ȣ��Ǿ� ���� ��ǥ ǥ�� �ؽ�Ʈ�� �����ϵ�����.
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

    // MonsterManager�� MonsterKillCount������Ƽ�� ���� ���� �����ϸ�
    // UI_Goal�� AllMonsterCount ������Ƽ�� ȣ��Ǿ� ���� ��ǥ ǥ�� �ؽ�Ʈ�� �����ϵ�����.
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

        // �ӽ������� Test���� �߰�
        // (���� ���� ������ Ingame Scene�� Init���� �ϴµ�, UI�� Image,Text ������Ʈ�� Bind�ϴ� Init�Լ��� Ingame Scene Init�Լ��� ���ϵǰ� ����
        // ����ǹǷ� ���ݻ�Ȳ������ UI ������Ʈ���� Bind�Ǳ��� RemainCount�� �����ϰ� �ȴ�.
        //  => �ش� ������ �Ŀ�, Monster Controller�� Spawn�Լ��� ���� �ű⼭ ���� �����ϰ� ���ٵ� �� ��쿡�� UI ������Ʈ���� Bind�� �Ϸ�Ǿ������� �ذ�ɰ�)

        Managers.Game.Monster.AllMonsterCount = Managers.Game.MonsterCount;
    }

    // ��ǥ UI ���� �����ϴ� �Լ�
    private void UpdateGoalText()
    {
        Get<Text>((int)Texts.GoalText).text = $"���� ����\n{m_allMonsterCount - m_monsterKillCount} / {m_allMonsterCount}";
    }

}
