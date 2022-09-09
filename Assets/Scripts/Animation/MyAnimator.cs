using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Define;
using AI;

public class MyAnimator : MonoBehaviour
{
    [Header("[�⺻ �ִϸ��̼� �ӵ�]")]
    public float DefaultAnimationSpeed = 1.0f;                                  // �⺻ �ִϸ��̼� �ӵ��� (�ӵ� ����, �ʱ� ������ ����)
    private Animator m_animator = null;                                         // Unity Animator
    private MyAnimatorInfo_Storage m_infoStorage = null;                        // ���� ��ü���� Animation Info (Animation Clip FileName,
                                                                                // Animator Controller Parametar Name)�� ��� �̱��� Ŭ����

    // (���� ����� Animatio Clip FileName, Animation Controller Parametar Name�� ���� ������Ʈ���� ������ �ְ� �ߴµ�, ������Ʈ Ǯ���ҋ�
    // �Ȱ��� ��ü�� �����ҋ����� ��� �ʱ�ȭ�� ������ؼ� �ڿ��Ҹ� ���߱⋚���� ����ó�� Animation Info�� �������ִ� �̱��� Ŭ������ �����ؼ�
    // �Ȱ��� ��ü�� Animation Info �ߺ� �ʱ�ȭ�� �������Ͽ� ���� ����� �����ߴ�.)

    private List<ChildAnimatorState> m_allState;                                // ���� Controller�� ��� State ����

    [Header("[�÷��̾� id]")]
    [Tooltip("�� ������Ʈ�� ����ϴ� ������Ʈ�� Player�ϋ� ����� Player���� �˷��ִ� �ĺ��ڷ� ����")]
    [SerializeField]
    private int m_playerId = -1;                                                // �÷��̾� id (��ü�� Player�� ��쿡 ���)

    [Header("[������Ʈ�� ����ϴ� ��ü]")]
    [SerializeField]
    private SpawnType m_subjectType;                                            // �� ������Ʈ�� ����ϴ� ��ü Ÿ��

    [Header("[������Ʈ�� ����ϴ� ���� Ÿ��]")]
    [Tooltip("����ϴ� ��ü�� Player�ϋ� �� ������ ������ص� �������")]
    [SerializeField]
    private EnemyType m_enemysubjectType;                                       // �� ������Ʈ�� ����ϴ� �� Ÿ�� (�� ��ü�� �� ��ü �϶�)

    private Player_AniParametar m_currentPlayerFlag = Player_AniParametar.None; // ���� Ȱ��ȭ�� Bool Parametar (Player)
    private Enemy_AniParametar m_currentEnemyFlag = Enemy_AniParametar.None;    // ���� Ȱ��ȭ�� Bool Parametar (Enemy)

    #region Property
    public Animator Animator { get => m_animator; }
    public int PlayerId { get => m_playerId; set => m_playerId = value; }
    public SpawnType SubjectType { get => m_subjectType; }
    public EnemyType EnemySubjectType { get => m_enemysubjectType; }
    #endregion

    private void Awake()
    {
        Initialize();
    }

    #region Initilize Function
    virtual protected void Initialize()
    {
        m_animator = FindAnimator();
        m_animator.speed = DefaultAnimationSpeed;

        m_infoStorage = MyAnimatorInfo_Storage.Instance;

        // ������Ʈ ��� ��ü�� ���� Dictonary ����
        if (m_subjectType == SpawnType.Player)
        {
            // �ߺ� �ʱ�ȭ�� ���� ����
            if(!m_infoStorage.PlayerAniFileName.ContainsKey(m_playerId))
            {
                m_infoStorage.PlayerAniFileName.Add(m_playerId, new Dictionary<Player_AniState, string>());
            }
            if(!m_infoStorage.PlayerAniParametar.ContainsKey(m_playerId))
            {
                m_infoStorage.PlayerAniParametar.Add(m_playerId, new Dictionary<Player_AniParametar, string>());
            }
        }
        else if (m_subjectType == SpawnType.Monster)
        {
            // �ߺ� �ʱ�ȭ�� ���� ����
            if (!m_infoStorage.EnemyAniFileName.ContainsKey(m_enemysubjectType))
            {
                m_infoStorage.EnemyAniFileName.Add(m_enemysubjectType, new Dictionary<Enemy_AniState, string>());
            }
            if(!m_infoStorage.EnemyAniParametar.ContainsKey(m_enemysubjectType))
            {
                m_infoStorage.EnemyAniParametar.Add(m_enemysubjectType, new Dictionary<Enemy_AniParametar, string>());
            }
        }
        
        m_allState = new List<ChildAnimatorState>();

        AllAnimationStateInfoUpdate();
        DictonaryInitialize();
    }


    // MyAnimatorInfo_Storage Ŭ������ �����ϴ� Animation Info (Animation Clip FileName, Animation Controller Parametar Name)��
    // ������ ������ Dictionary���� �ʱ�ȭ �ϴ� �Լ�
    private void DictonaryInitialize()
    {
        AutoAniFileNameInitialize();
        AutoParametarInitialize();
    }

    // �ִϸ��̼� Ŭ������(Motion ����)�� �̸��� Dictonary�� �ڵ����� �ʱ�ȭ���ִ� �Լ�
    private void AutoAniFileNameInitialize()
    {
        bool[] l_isCompletes = new bool[m_allState.Count];      // Dictonary�� �߰� �Ǿ������� ������ bool �迭
        int l_enumCount = 0;                                    // state�� ��Ÿ���� Enum�� ��� ����
        Motion l_stateMotion;                                   // state�� �������ִ� Motion(Animation Clip)

        switch (m_subjectType)
        {
            case SpawnType.Player:
                {
                    // �ѹ��̶� �ʱ�ȭ�� �̷������� �Լ� ����
                    if (m_infoStorage.PlayerAniFileName[m_playerId].Count > 0) 
                    {
                        return;
                    }

                    Player_AniState l_stateEnum = Player_AniState.Player_Run;
                    l_enumCount = System.Enum.GetValues(typeof(Player_AniState)).Length;

                    for (int i = 0; i < l_enumCount; i++)
                    {
                        for (int j = 0; j < m_allState.Count; j++)
                        {
                            l_stateMotion = m_allState[j].state.motion;

                            // �̹� ���ϸ� �߰� �Ǿ��ų�, State�� Motion������ �������� �ʴ� ���
                            if (l_isCompletes[j] || l_stateMotion == null)
                            {
                                continue;
                            }

                            // blend tree�� ���ϸ� �ʱ�ȭ X
                            if (l_stateMotion.GetType() != typeof(BlendTree))
                            {
                                if (l_stateEnum.ToString().Contains(m_allState[j].state.name))
                                {
                                    m_infoStorage.PlayerAniFileName[m_playerId].Add(l_stateEnum, l_stateMotion.name);
                                    l_isCompletes[j] = true;
                                    break;
                                }
                            }
                        }
                        l_stateEnum++;
                    }
                }
                break;

            case SpawnType.Monster:
                {
                    // �ѹ��̶� �ʱ�ȭ�� �̷������� �Լ� ����
                    if (m_infoStorage.EnemyAniFileName[m_enemysubjectType].Count > 0) 
                    {
                        return;
                    }

                    Enemy_AniState l_stateEnum = Enemy_AniState.Idle;
                    l_enumCount = System.Enum.GetValues(typeof(Enemy_AniState)).Length;

                    string l_stateName;                                             // state �̸� (string��)
                    string l_enemySubjectType = m_enemysubjectType.ToString();      // �� ������Ʈ�� ����ϴ� ��������Ʈ�� Ÿ�Ը� (String��)

                    for (int i = 0; i < l_enumCount; i++)
                    {
                        // �ڽ��� Ÿ�԰� �����ʴ� enum���� �ѱ������ �ڵ�
                        // ex) MyAnimator ��� ��ü�� Boss �ε�, Melee State�� Ȯ���Ұ��
                        l_stateName = l_stateEnum.ToString();
                        if (l_stateName.Contains('_') && !l_stateName.Contains(l_enemySubjectType))
                        {
                            l_stateEnum++;
                            continue;
                        }

                        for (int j = 0; j < m_allState.Count; j++)
                        {
                            l_stateMotion = m_allState[j].state.motion;

                            // �̹� ���ϸ� �߰� �Ǿ��ų�, State�� Motion������ �������� �ʴ� ���
                            if (l_isCompletes[j] || l_stateMotion == null)
                            {
                                continue;
                            }

                            // blend tree�� ���ϸ� �ʱ�ȭ X
                            if (l_stateMotion.GetType() != typeof(BlendTree))
                            {
                                if (l_stateEnum.ToString().Contains(m_allState[j].state.name))
                                {
                                    m_infoStorage.EnemyAniFileName[m_enemysubjectType].Add(l_stateEnum, l_stateMotion.name);
                                    l_isCompletes[j] = true;
                                    break;
                                }
                            }
                        }
                        l_stateEnum++;
                    }
                }
                break;
        }
    }

    // Animator Parametar�� �̸��� Dictonary�� �ڵ����� �ʱ�ȭ���ִ� �Լ�
    private void AutoParametarInitialize()
    {
        AnimatorControllerParameter[] l_parametars = m_animator.parameters;         // ���� Ainmator Controller���� ���Ǵ� Parametar��
        bool[] l_isCompletes = new bool[l_parametars.Length];                       // Dictonary�� �߰� �Ǿ������� ������ bool �迭
        int l_enumCount = 0;

        switch (m_subjectType)
        {
            case SpawnType.Player:
                {
                    // �ѹ��̶� �ʱ�ȭ�� �̷������� �Լ� ����
                    if (m_infoStorage.PlayerAniParametar[m_playerId].Count > 0)
                    {
                        return;
                    }

                    Player_AniParametar l_parametarEnum;
                    l_enumCount = System.Enum.GetValues(typeof(Player_AniParametar)).Length;

                    l_parametarEnum = Player_AniParametar.JumpTrigger;
                    for (int i = 0; i < l_enumCount; i++)
                    {
                        for (int j = 0; j < l_parametars.Length; j++)
                        {
                            // �̹� Parametar�� �߰��� ���
                            if (l_isCompletes[j])
                            {
                                continue;
                            }

                            // enum�� (string��)�� Parametar���� �����ϰ��ִ� ���
                            if (l_parametarEnum.ToString().Contains(l_parametars[j].name))
                            {
                                m_infoStorage.PlayerAniParametar[m_playerId].Add(l_parametarEnum, l_parametars[j].name);
                                l_isCompletes[j] = true;
                                break;
                            }
                        }
                        l_parametarEnum++;
                    }
                }
                break;

            case SpawnType.Monster:
                {
                    // �ѹ��̶� �ʱ�ȭ�� �̷������� �Լ� ����
                    if (m_infoStorage.EnemyAniParametar[m_enemysubjectType].Count > 0)
                    {
                        return;
                    }

                    Enemy_AniParametar l_parametarEnum = Enemy_AniParametar.DeathFlag;
                    l_enumCount = System.Enum.GetValues(typeof(Enemy_AniParametar)).Length;
                    string l_parametarName;                                         // parametar �̸�
                    string l_enemySubjectType = m_enemysubjectType.ToString();      // �� ������Ʈ�� ����ϴ� ��������Ʈ�� Ÿ�Ը� (String��)

                    for (int i = 0; i < l_enumCount; i++)
                    {
                        // �ڽ��� Ÿ�԰� �����ʴ� enum���� �ѱ������ �ڵ�
                        l_parametarName = l_parametarEnum.ToString();
                        if (l_parametarName.Contains('_') && !l_parametarName.Contains(l_enemySubjectType))
                        {
                            l_parametarEnum++;
                            continue;
                        }

                        for (int j = 0; j < l_parametars.Length; j++)
                        {
                            // �̹� Parametar�� �߰��� ���
                            if (l_isCompletes[j])
                            {
                                continue;
                            }

                            // enum�� (string��)�� Parametar���� �����ϰ��ִ� ���
                            if (l_parametarName.Contains(l_parametars[j].name))
                            {
                                m_infoStorage.EnemyAniParametar[m_enemysubjectType].Add(l_parametarEnum, l_parametars[j].name);
                                l_isCompletes[j] = true;
                                break;
                            }
                        }
                        l_parametarEnum++;
                    }
                }
                break;
        }
    }
    #endregion

    #region Get Transition Function
    // Transition ����� �ִϸ��̼��� �ִϸ��̼� ��ȯ�� Transition ����ϱ⋚����,
    // ���� Ÿ�̹� �����ϱ����ؼ��� �Ʒ��� GetTransitionFromState �Լ��� ���� Transition�� �˾Ƴ���,
    // Offset���� �����ϸ� �ȴ�.

    // �Է¹��� State���� ����Ǿ��ִ� ��� Transition ���ϴ� �Լ�
    private AnimatorStateTransition[] GetAllTransitionFromState<T>(T _animationState) where T : System.Enum
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return null;
        }

        // enum state�� string������ state�� ��ȯ �۾�
        string l_stateName = _animationState.ToString();
        if (l_stateName.Contains("_"))
        {
            string[] l_splits = l_stateName.Split('_');
            l_stateName = l_splits[l_splits.Length - 1];
        }

        // State List������ ���ϴ� state�� ã��, �ش� state�� transition���� ��ȯ
        foreach (ChildAnimatorState childState in m_allState)
        {
            if (childState.state.name == l_stateName)
            {
                return childState.state.transitions;
            }
        }
        return null;
    }
    private AnimatorStateTransition[] GetAllTransitionFromState(string _animationState)
    {
        // State List������ ���ϴ� state�� ã��, �ش� state�� transition���� ��ȯ
        foreach (ChildAnimatorState childState in m_allState)
        {
            if (childState.state.name == _animationState)
            {
                return childState.state.transitions;
            }
        }
        return null;
    }


    // ���� State, ���� State�� ������ Transition�� ã�� �Լ�
    public AnimatorStateTransition GetTransitionFromState<T>(T _startState, T _endState) where T : System.Enum
    {
        AnimatorStateTransition[] l_transitions = null;

        // ���� State�� ����� ��� Transition�� ���Ѵ�.
        if ((l_transitions = GetAllTransitionFromState(_startState)) == null)
        {
            return null;
        }

        // enum state�� string������ state�� ��ȯ �۾�
        string l_endStateName = _endState.ToString();
        if (l_endStateName.Contains("_"))
        {
            string[] l_splits = l_endStateName.Split('_');
            l_endStateName = l_splits[l_splits.Length - 1];
        }

        // ������ ���� Transition�� ���� State�� ������ Transition�� ã�Ƽ� ����
        foreach (AnimatorStateTransition transition in l_transitions)
        {
            if(transition.destinationState.name == l_endStateName)
            {
                return transition;
            }
        }
        return null;
    }
    public AnimatorStateTransition GetTransitionFromState(string _startState, string _endState)
    {
        AnimatorStateTransition[] l_transitions = null;

        // ���� State�� ����� ��� Transition�� ���Ѵ�.
        if ((l_transitions = GetAllTransitionFromState(_startState)) == null)
        {
            return null;
        }

        // ������ ���� Transition�� ���� State�� ������ Transition�� ã�Ƽ� ����
        foreach (AnimatorStateTransition transition in l_transitions)
        {
            if (transition.destinationState.name == _endState)
            {
                return transition;
            }
        }
        return null;
    }


    // Any State�� ����Ǿ� �ִ�, Ư�� Transition�� ã�� �Լ�
    // (AnyState�� layer���� �����ϱ� ������, _layer �Ű������� ���� �ִϸ��̼� ���̾������ AnyState�� ����� Transition ã�� �� �ֵ�����)
    public AnimatorStateTransition GetTransitionFromAnyState<T>(T _endState, string _layer = "Base Layer") where T : System.Enum
    {
        AnimatorStateTransition[] l_transitions = null;

        // enum state�� string������ state�� ��ȯ �۾�
        string l_endStateName = _endState.ToString();
        if (l_endStateName.Contains("_"))
        {
            string[] l_splits = l_endStateName.Split('_');
            l_endStateName = l_splits[l_splits.Length - 1];
        }

        // ���� Animator Controller�� layer�� ����
        AnimatorController l_aniController = m_animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] l_layers = l_aniController.layers;

        // ���ϴ� layer�� ã��, �ش� layer�� anyState�� ����� Transition����
        foreach (AnimatorControllerLayer layer in l_layers)
        {
            if(layer.name == _layer)
            {
                l_transitions = layer.stateMachine.anyStateTransitions;
                break;
            }
        }
        // anyState�� ����� Trnasition�� �߿� �츮�� ���ϴ� ���� State�� ������ Transition�� ã�Ƽ� ����
        foreach (AnimatorStateTransition transition in l_transitions)
        {
            if (transition.destinationState.name == l_endStateName)
            {
                return transition;
            }
        }
        return null;
    }
    public AnimatorStateTransition GetTransitionFromAnyState(string _endState, string _layer = "Base Layer")
    {
        AnimatorStateTransition[] l_transitions = null;

        // ���� Animator Controller�� layer�� ����
        AnimatorController l_aniController = m_animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] l_layers = l_aniController.layers;

        // ���ϴ� layer�� ã��, �ش� layer�� anyState�� ����� Transition����
        foreach (AnimatorControllerLayer layer in l_layers)
        {
            if (layer.name == _layer)
            {
                l_transitions = layer.stateMachine.anyStateTransitions;
                break;
            }
        }
        // anyState�� ����� Trnasition�� �߿� �츮�� ���ϴ� ���� State�� ������ Transition�� ã�Ƽ� ����
        foreach (AnimatorStateTransition transition in l_transitions)
        {
            if (transition.destinationState.name == _endState)
            {
                return transition;
            }
        }
        return null;
    }
    #endregion

    #region AnimationController Parametar Get/Set

    public bool GetBool(Player_AniParametar _parametar)
    {
        if(!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return false;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        return m_animator.GetBool(l_parametarName);
    }
    public bool GetBool(Enemy_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return false;
        }
        string l_parametarName = m_infoStorage.EnemyAniParametar[m_enemysubjectType][_parametar];
        return m_animator.GetBool(l_parametarName);
    }
    public bool GetBool(string _parametar)
    {
        return m_animator.GetBool(_parametar);
    }


    public int GetInteger(Player_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return -1;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        return m_animator.GetInteger(l_parametarName);
    }
    public int GetInteger(Enemy_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return -1;
        }
        string l_parametarName = m_infoStorage.EnemyAniParametar[m_enemysubjectType][_parametar];
        return m_animator.GetInteger(l_parametarName);
    }
    public int GetInteger(string _parametar)
    {
        return m_animator.GetInteger(_parametar);
    }


    public float GetFloat(Player_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return -1f;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        return m_animator.GetFloat(l_parametarName);
    }
    public float GetFloat(Enemy_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return -1f;
        }
        string l_parametarName = m_infoStorage.EnemyAniParametar[m_enemysubjectType][_parametar];
        return m_animator.GetFloat(l_parametarName);
    }
    public float GetFloat(string _parametar)
    {
        return m_animator.GetFloat(_parametar);
    }

    public void SetBool(Player_AniParametar _parametar, bool _boolValue)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        m_currentPlayerFlag = _parametar;
        m_animator.SetBool(l_parametarName, _boolValue);
    }
    public void SetBool(Enemy_AniParametar _parametar, bool _boolValue)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_parametarName = m_infoStorage.EnemyAniParametar[m_enemysubjectType][_parametar];
        m_currentEnemyFlag = _parametar;
        m_animator.SetBool(l_parametarName, _boolValue);
    }
    public void SetBool(string _parametar, bool _boolValue)
    {
        m_animator.SetBool(_parametar, _boolValue);
    }

    public void SetTrigger(Player_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        m_currentPlayerFlag = _parametar;
        m_animator.SetTrigger(l_parametarName);
    }
    public void SetTrigger(Enemy_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_parametarName = m_infoStorage.EnemyAniParametar[m_enemysubjectType][_parametar];
        m_currentEnemyFlag = _parametar;
        m_animator.SetTrigger(l_parametarName);
    }
    public void SetTrigger(string _parametar)
    {
        m_animator.SetTrigger(_parametar);
    }

    public void SetInteger(Player_AniParametar _parametar, int _value)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        m_currentPlayerFlag = _parametar;
        m_animator.SetInteger(l_parametarName, _value);
    }
    public void SetInteger(Enemy_AniParametar _parametar, int _value)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_parametarName = m_infoStorage.EnemyAniParametar[m_enemysubjectType][_parametar];
        m_currentEnemyFlag = _parametar;
        m_animator.SetInteger(l_parametarName, _value);
    }
    public void SetInteger(string _parametar, int _value)
    {
        m_animator.SetInteger(_parametar, _value);
    }

    public void SetFloat(Player_AniParametar _parametar, float _value)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        m_currentPlayerFlag = _parametar;
        m_animator.SetFloat(l_parametarName, _value);
    }
    public void SetFloat(Enemy_AniParametar _parametar, float _value)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_parametarName = m_infoStorage.EnemyAniParametar[m_enemysubjectType][_parametar];
        m_currentEnemyFlag = _parametar;
        m_animator.SetFloat(l_parametarName, _value);
    }
    public void SetFloat(string _parametar, float _value)
    {
        m_animator.SetFloat(_parametar, _value);
    }

    #endregion

    // BlendTree�� ��� �������ִ� �ִϸ��̼��� Ÿ�Ӷ����� ���󰡴°��� �ƴϱ⋚���� Animation Speed,Animation Loop ������ �ʿ������ ����.

    #region Animation Speed Change
    // _playTime ���� : miliSecond    
    // ���ϴ� State�� Player Animation �ӵ��� milisecond������ ����
    public void SetAnimationSpeed_MiliSecond(Player_AniState _animationState, float _miliSecPlayTime = 1000.0f)
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_animationFileName = m_infoStorage.PlayerAniFileName[m_playerId][_animationState];
        AnimationClip[] l_clips = m_animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip _clip in l_clips)
        {
            if (_clip.name.Contains(l_animationFileName))
            {
                m_animator.speed = (1000.0f * _clip.length) / _miliSecPlayTime;
                return;
            }
        }
    }
    // ���ϴ� State�� Enemy Animation �ӵ��� milisecond������ ����
    public void SetAnimationSpeed_MiliSecond(Enemy_AniState _animationState, float _miliSecPlayTime = 1000.0f)
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_animationFileName = m_infoStorage.EnemyAniFileName[m_enemysubjectType][_animationState];
        AnimationClip[] l_clips = m_animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip _clip in l_clips)
        {
            if (_clip.name.Contains(l_animationFileName))
            {
                m_animator.speed = (1000.0f * _clip.length) / _miliSecPlayTime;
                return;
            }
        }
    }

    // ���ϴ� State�� Player Animation �ӵ��� second������ ����
    public void SetAnimationSpeed_Second(Player_AniState _animationState, float _SecPlayTime = 1)
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_animationFileName = m_infoStorage.PlayerAniFileName[m_playerId][_animationState];
        AnimationClip[] l_clips = m_animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip _clip in l_clips)
        {
            if (_clip.name.Contains(l_animationFileName))
            {
                m_animator.speed = _clip.length / _SecPlayTime;
                return;
            }
        }
    }
    // ���ϴ� State�� Enemy Animation �ӵ��� second������ ����
    public void SetAnimationSpeed_Second(Enemy_AniState _animationState, float _SecPlayTime = 1)
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }
        string l_animationFileName = m_infoStorage.EnemyAniFileName[m_enemysubjectType][_animationState];
        AnimationClip[] l_clips = m_animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip _clip in l_clips)
        {
            if (_clip.name.Contains(l_animationFileName))
            {
                m_animator.speed = _clip.length / _SecPlayTime;
                return;
            }
        }
    }

    // �ִϸ��̼� �ӵ��� �⺻������ �缳��
    public void AnimationSpeedReset()
    {
        m_animator.speed = DefaultAnimationSpeed;
    }

    #endregion

    #region Animation Loop Setting
    // ���ϴ� State�� CycleOffset�� �����ϴ� �Լ� (���� �Ⱦ��� �𸣰�����, ����� �������� �ϴ� ����)
    public void SetCycleOffset<T>(T _animationState, float _cycleOffset) where T : System.Enum
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }

        string l_stateName = _animationState.ToString();

        if (l_stateName.Contains("_"))
        {
            string[] l_splits = l_stateName.Split('_');
            l_stateName = l_splits[l_splits.Length - 1];
        }

        foreach(ChildAnimatorState childState in m_allState)
        {
            if (childState.state.name == l_stateName)
            {
                childState.state.cycleOffset = _cycleOffset;
                break;
            }
        }
    }
    public void SetCycleOffset(string _animationState, float _cycleOffset)
    {
        foreach (ChildAnimatorState childState in m_allState)
        {
            if (childState.state.name == _animationState)
            {
                childState.state.cycleOffset = _cycleOffset;
                break;
            }
        }
    }

    // ���ϴ� state�� �ִϸ��̼��� Loop Time�� �����ϴ� �Լ�
    public void SetAnimationLoop<T>(T _animationState, bool _flag) where T : System.Enum
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("�߸��� ������ �Ķ���� ����");
            return;
        }

        // enum������ Animation State�� �̸� ���ϴ� �۾�
        string l_stateName = _animationState.ToString();

        if (l_stateName.Contains("_"))
        {
            string[] l_splits = l_stateName.Split('_');
            l_stateName = l_splits[l_splits.Length - 1];
        }

        
        foreach (ChildAnimatorState childState in m_allState)
        {
            // �����ϰ���� State�� ��ȸ�ϸ鼭 ã��
            if (childState.state.name == l_stateName)
            {
                // ã�� State�� Animation Clip�� Path����, �� .FBX������ ��θ� ������ �µ� �̸� �̿��Ͽ� FBX Model Importer
                // �� ������.
                string l_assetPath = UnityEditor.AssetDatabase.GetAssetPath(childState.state.motion.GetInstanceID());
                UnityEditor.ModelImporter l_modelImporter = UnityEditor.AssetImporter.GetAtPath(l_assetPath) as UnityEditor.ModelImporter;
                UnityEditor.ModelImporterClipAnimation[] l_clipAni = l_modelImporter.clipAnimations;

                // Model Importer�� ��ϵǾ��ִ� Animation Clip�� ��, �츮�� �����ϱ� ���ϴ� Clip�� ã��,
                // �ش� Clip�� loopTime�� �����Ѵ�.
                foreach (UnityEditor.ModelImporterClipAnimation item in l_clipAni)
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
    public void SetAnimationLoop(string _animationState, bool _flag)
    {
        foreach (ChildAnimatorState childState in m_allState)
        {
            // �����ϰ���� State�� ��ȸ�ϸ鼭 ã��
            if (childState.state.name == _animationState)
            {
                // ã�� State�� Animation Clip�� Path����, �� .FBX������ ��θ� ������ �µ� �̸� �̿��Ͽ� FBX Model Importer
                // �� ������.
                string l_assetPath = UnityEditor.AssetDatabase.GetAssetPath(childState.state.motion.GetInstanceID());
                UnityEditor.ModelImporter l_modelImporter = UnityEditor.AssetImporter.GetAtPath(l_assetPath) as UnityEditor.ModelImporter;
                UnityEditor.ModelImporterClipAnimation[] l_clipAni = l_modelImporter.clipAnimations;

                // Model Importer�� ��ϵǾ��ִ� Animation Clip�� ��, �츮�� �����ϱ� ���ϴ� Clip�� ã��,
                // �ش� Clip�� loopTime�� �����Ѵ�.
                foreach (UnityEditor.ModelImporterClipAnimation item in l_clipAni)
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
    #endregion

    #region Another Class Function
    // �ڽŰ�, ���� ������Ʈ���� Animator ������Ʈ�� ã�Ƽ� �����ϴ� �Լ�
    public Animator FindAnimator()
    {
        Animator l_animator = null;
        if (!TryGetComponent<Animator>(out l_animator))
        {
            l_animator = transform.GetComponentInChildren<Animator>();
        }

        return l_animator;
    }

    // ���޹��� State Enum���� ����� ���޵Ǿ����� üũ�ϴ� �Լ�
    // ex) Enemy ������Ʈ�ε�, Player_AniState, Player_AniParametar�� �����ϴ°� ����
    private bool FunctionParametarCheck<T>(T _parametar)
    {
        System.Type l_parametarType = _parametar.GetType();

        if(l_parametarType == typeof(Player_AniParametar) || l_parametarType == typeof(Player_AniState))
        {
            if(m_subjectType == SpawnType.Monster)
            {
                return false;
            }
            return true;
        }
        else if(l_parametarType == typeof(Enemy_AniParametar) || l_parametarType == typeof(Enemy_AniState))
        {
            if(m_subjectType == SpawnType.Player)
            {
                return false;
            }
            return true;
        }

        return false;
    }

    // ���� Animation Controller�� ��ϵ� State���� ������ m_allState ������ �����ϴ� �Լ�
    public void AllAnimationStateInfoUpdate()
    {
        // �̹� ���ŵ� ������ ������� Clear�Լ��� �ʱ�ȭ
        if(m_allState.Count >0)
        {
            m_allState.Clear();
        }

        AnimatorController l_aniController;
        ChildAnimatorState[] l_states;

        // ���� Animator Controller�� Layer �������� �޾ƿ�
        l_aniController = m_animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] l_layers = l_aniController.layers;

        // Layer�� ��ȸ�ϸ鼭 ��ϵ� State������ m_allState ����Ʈ�� �߰�
        foreach (AnimatorControllerLayer layer in l_layers)
        {
            l_states = layer.stateMachine.states;

            foreach (ChildAnimatorState state in l_states)
            {
                m_allState.Add(state);
            }
        }     
    }
    // ���� Ȱ��ȭ �Ǿ��ִ� Bool Parametar�� false�� �����ϴ� �Լ�
    public void FlagClear()
    {
        switch (m_subjectType)
        {
            case SpawnType.Player:
                if (m_currentPlayerFlag == Player_AniParametar.None)
                {
                    return;
                }
                SetBool(m_currentPlayerFlag, false);
                break;

            case SpawnType.Monster:
                if (m_currentEnemyFlag == Enemy_AniParametar.None)
                {
                    return;
                }
                SetBool(m_currentEnemyFlag, false);
                break;
        }
    }

    // Animation Layer���� ���� State������ �޾ƿ��� �Լ�
    public AnimatorStateInfo GetCurrentStateInfo(string _layer = "Base Layer")
    {
        return m_animator.GetCurrentAnimatorStateInfo(m_animator.GetLayerIndex(_layer));
    }
    #endregion

}