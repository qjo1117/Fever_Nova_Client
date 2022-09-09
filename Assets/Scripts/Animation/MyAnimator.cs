using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Define;
using AI;

public class MyAnimator : MonoBehaviour
{
    [Header("[기본 애니메이션 속도]")]
    public float DefaultAnimationSpeed = 1.0f;                                  // 기본 애니메이션 속도값 (속도 리셋, 초기 생성시 적용)
    private Animator m_animator = null;                                         // Unity Animator
    private MyAnimatorInfo_Storage m_infoStorage = null;                        // 각각 객체들의 Animation Info (Animation Clip FileName,
                                                                                // Animator Controller Parametar Name)이 담긴 싱글톤 클래스

    // (기존 방식은 Animatio Clip FileName, Animation Controller Parametar Name을 각각 컴포넌트마다 가지고 있게 했는데, 오브젝트 풀링할떄
    // 똑같은 객체를 생성할떄마다 계속 초기화를 해줘야해서 자원소모가 심했기떄문에 지금처럼 Animation Info를 가지고있는 싱글톤 클래스를 생성해서
    // 똑같은 객체의 Animation Info 중복 초기화를 막기위하여 현재 방식을 선택했다.)

    private List<ChildAnimatorState> m_allState;                                // 현재 Controller의 모든 State 정보

    [Header("[플레이어 id]")]
    [Tooltip("이 컴포넌트를 사용하는 오브젝트가 Player일떄 몇번쨰 Player인지 알려주는 식별자로 사용됨")]
    [SerializeField]
    private int m_playerId = -1;                                                // 플레이어 id (주체가 Player일 경우에 사용)

    [Header("[컴포넌트를 사용하는 주체]")]
    [SerializeField]
    private SpawnType m_subjectType;                                            // 이 컴포넌트를 사용하는 객체 타입

    [Header("[컴포넌트를 사용하는 적의 타입]")]
    [Tooltip("사용하는 주체가 Player일떄 이 변수는 어떤값을해도 상관없음")]
    [SerializeField]
    private EnemyType m_enemysubjectType;                                       // 이 컴포넌트를 사용하는 적 타입 (이 객체가 적 객체 일때)

    private Player_AniParametar m_currentPlayerFlag = Player_AniParametar.None; // 현재 활성화된 Bool Parametar (Player)
    private Enemy_AniParametar m_currentEnemyFlag = Enemy_AniParametar.None;    // 현재 활성화된 Bool Parametar (Enemy)

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

        // 컴포넌트 사용 주체에 따라 Dictonary 생성
        if (m_subjectType == SpawnType.Player)
        {
            // 중복 초기화를 막기 위함
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
            // 중복 초기화를 막기 위함
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


    // MyAnimatorInfo_Storage 클래스에 존재하는 Animation Info (Animation Clip FileName, Animation Controller Parametar Name)의
    // 정보를 가지는 Dictionary들을 초기화 하는 함수
    private void DictonaryInitialize()
    {
        AutoAniFileNameInitialize();
        AutoParametarInitialize();
    }

    // 애니메이션 클립파일(Motion 파일)의 이름을 Dictonary에 자동으로 초기화해주는 함수
    private void AutoAniFileNameInitialize()
    {
        bool[] l_isCompletes = new bool[m_allState.Count];      // Dictonary에 추가 되었는지를 저장할 bool 배열
        int l_enumCount = 0;                                    // state를 나타내는 Enum의 요소 갯수
        Motion l_stateMotion;                                   // state가 가지고있는 Motion(Animation Clip)

        switch (m_subjectType)
        {
            case SpawnType.Player:
                {
                    // 한번이라도 초기화가 이루어진경우 함수 종료
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

                            // 이미 파일명 추가 되었거나, State에 Motion파일이 존재하지 않는 경우
                            if (l_isCompletes[j] || l_stateMotion == null)
                            {
                                continue;
                            }

                            // blend tree는 파일명 초기화 X
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
                    // 한번이라도 초기화가 이루어진경우 함수 종료
                    if (m_infoStorage.EnemyAniFileName[m_enemysubjectType].Count > 0) 
                    {
                        return;
                    }

                    Enemy_AniState l_stateEnum = Enemy_AniState.Idle;
                    l_enumCount = System.Enum.GetValues(typeof(Enemy_AniState)).Length;

                    string l_stateName;                                             // state 이름 (string형)
                    string l_enemySubjectType = m_enemysubjectType.ToString();      // 이 컴포넌트를 사용하는 적오브젝트의 타입명 (String형)

                    for (int i = 0; i < l_enumCount; i++)
                    {
                        // 자신의 타입과 맞지않는 enum값은 넘기기위한 코드
                        // ex) MyAnimator 사용 주체는 Boss 인데, Melee State를 확인할경우
                        l_stateName = l_stateEnum.ToString();
                        if (l_stateName.Contains('_') && !l_stateName.Contains(l_enemySubjectType))
                        {
                            l_stateEnum++;
                            continue;
                        }

                        for (int j = 0; j < m_allState.Count; j++)
                        {
                            l_stateMotion = m_allState[j].state.motion;

                            // 이미 파일명 추가 되었거나, State에 Motion파일이 존재하지 않는 경우
                            if (l_isCompletes[j] || l_stateMotion == null)
                            {
                                continue;
                            }

                            // blend tree는 파일명 초기화 X
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

    // Animator Parametar의 이름을 Dictonary에 자동으로 초기화해주는 함수
    private void AutoParametarInitialize()
    {
        AnimatorControllerParameter[] l_parametars = m_animator.parameters;         // 현재 Ainmator Controller에서 사용되는 Parametar들
        bool[] l_isCompletes = new bool[l_parametars.Length];                       // Dictonary에 추가 되었는지를 저장할 bool 배열
        int l_enumCount = 0;

        switch (m_subjectType)
        {
            case SpawnType.Player:
                {
                    // 한번이라도 초기화가 이루어진경우 함수 종료
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
                            // 이미 Parametar명 추가된 경우
                            if (l_isCompletes[j])
                            {
                                continue;
                            }

                            // enum값 (string형)이 Parametar명을 포함하고있는 경우
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
                    // 한번이라도 초기화가 이루어진경우 함수 종료
                    if (m_infoStorage.EnemyAniParametar[m_enemysubjectType].Count > 0)
                    {
                        return;
                    }

                    Enemy_AniParametar l_parametarEnum = Enemy_AniParametar.DeathFlag;
                    l_enumCount = System.Enum.GetValues(typeof(Enemy_AniParametar)).Length;
                    string l_parametarName;                                         // parametar 이름
                    string l_enemySubjectType = m_enemysubjectType.ToString();      // 이 컴포넌트를 사용하는 적오브젝트의 타입명 (String형)

                    for (int i = 0; i < l_enumCount; i++)
                    {
                        // 자신의 타입과 맞지않는 enum값은 넘기기위한 코드
                        l_parametarName = l_parametarEnum.ToString();
                        if (l_parametarName.Contains('_') && !l_parametarName.Contains(l_enemySubjectType))
                        {
                            l_parametarEnum++;
                            continue;
                        }

                        for (int j = 0; j < l_parametars.Length; j++)
                        {
                            // 이미 Parametar명 추가된 경우
                            if (l_isCompletes[j])
                            {
                                continue;
                            }

                            // enum값 (string형)이 Parametar명을 포함하고있는 경우
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
    // Transition 사용한 애니메이션은 애니메이션 전환시 Transition 사용하기떄문에,
    // 시작 타이밍 조절하기위해서는 아래의 GetTransitionFromState 함수를 통해 Transition을 알아낸후,
    // Offset값을 조절하면 된다.

    // 입력받은 State값과 연결되어있는 모든 Transition 구하는 함수
    private AnimatorStateTransition[] GetAllTransitionFromState<T>(T _animationState) where T : System.Enum
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
            return null;
        }

        // enum state를 string형태의 state로 변환 작업
        string l_stateName = _animationState.ToString();
        if (l_stateName.Contains("_"))
        {
            string[] l_splits = l_stateName.Split('_');
            l_stateName = l_splits[l_splits.Length - 1];
        }

        // State List에서서 원하는 state를 찾고, 해당 state의 transition들을 반환
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
        // State List에서서 원하는 state를 찾고, 해당 state의 transition들을 반환
        foreach (ChildAnimatorState childState in m_allState)
        {
            if (childState.state.name == _animationState)
            {
                return childState.state.transitions;
            }
        }
        return null;
    }


    // 시작 State, 종료 State를 가지는 Transition을 찾는 함수
    public AnimatorStateTransition GetTransitionFromState<T>(T _startState, T _endState) where T : System.Enum
    {
        AnimatorStateTransition[] l_transitions = null;

        // 시작 State와 연결된 모든 Transition을 구한다.
        if ((l_transitions = GetAllTransitionFromState(_startState)) == null)
        {
            return null;
        }

        // enum state를 string형태의 state로 변환 작업
        string l_endStateName = _endState.ToString();
        if (l_endStateName.Contains("_"))
        {
            string[] l_splits = l_endStateName.Split('_');
            l_endStateName = l_splits[l_splits.Length - 1];
        }

        // 위에서 구한 Transition중 종료 State를 가지는 Transition을 찾아서 리턴
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

        // 시작 State와 연결된 모든 Transition을 구한다.
        if ((l_transitions = GetAllTransitionFromState(_startState)) == null)
        {
            return null;
        }

        // 위에서 구한 Transition중 종료 State를 가지는 Transition을 찾아서 리턴
        foreach (AnimatorStateTransition transition in l_transitions)
        {
            if (transition.destinationState.name == _endState)
            {
                return transition;
            }
        }
        return null;
    }


    // Any State와 연결되어 있는, 특정 Transition을 찾는 함수
    // (AnyState는 layer별로 존재하기 떄문에, _layer 매개변수를 통해 애니메이션 레이어값에따라 AnyState에 연결된 Transition 찾을 수 있도록함)
    public AnimatorStateTransition GetTransitionFromAnyState<T>(T _endState, string _layer = "Base Layer") where T : System.Enum
    {
        AnimatorStateTransition[] l_transitions = null;

        // enum state를 string형태의 state로 변환 작업
        string l_endStateName = _endState.ToString();
        if (l_endStateName.Contains("_"))
        {
            string[] l_splits = l_endStateName.Split('_');
            l_endStateName = l_splits[l_splits.Length - 1];
        }

        // 현재 Animator Controller의 layer들 구함
        AnimatorController l_aniController = m_animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] l_layers = l_aniController.layers;

        // 원하는 layer를 찾고, 해당 layer의 anyState와 연결된 Transition구함
        foreach (AnimatorControllerLayer layer in l_layers)
        {
            if(layer.name == _layer)
            {
                l_transitions = layer.stateMachine.anyStateTransitions;
                break;
            }
        }
        // anyState와 연결된 Trnasition들 중에 우리가 원하는 종료 State를 가지는 Transition을 찾아서 리턴
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

        // 현재 Animator Controller의 layer들 구함
        AnimatorController l_aniController = m_animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] l_layers = l_aniController.layers;

        // 원하는 layer를 찾고, 해당 layer의 anyState와 연결된 Transition구함
        foreach (AnimatorControllerLayer layer in l_layers)
        {
            if (layer.name == _layer)
            {
                l_transitions = layer.stateMachine.anyStateTransitions;
                break;
            }
        }
        // anyState와 연결된 Trnasition들 중에 우리가 원하는 종료 State를 가지는 Transition을 찾아서 리턴
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
            return false;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        return m_animator.GetBool(l_parametarName);
    }
    public bool GetBool(Enemy_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
            return -1;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        return m_animator.GetInteger(l_parametarName);
    }
    public int GetInteger(Enemy_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
            return -1f;
        }
        string l_parametarName = m_infoStorage.PlayerAniParametar[m_playerId][_parametar];
        return m_animator.GetFloat(l_parametarName);
    }
    public float GetFloat(Enemy_AniParametar _parametar)
    {
        if (!FunctionParametarCheck(_parametar))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
            Debug.LogError("잘못된 형식의 파라메터 전달");
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

    // BlendTree의 경우 정해져있는 애니메이션의 타임라인을 따라가는것이 아니기떄문에 Animation Speed,Animation Loop 설정이 필요없을것 같다.

    #region Animation Speed Change
    // _playTime 단위 : miliSecond    
    // 원하는 State의 Player Animation 속도를 milisecond단위로 설정
    public void SetAnimationSpeed_MiliSecond(Player_AniState _animationState, float _miliSecPlayTime = 1000.0f)
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
    // 원하는 State의 Enemy Animation 속도를 milisecond단위로 설정
    public void SetAnimationSpeed_MiliSecond(Enemy_AniState _animationState, float _miliSecPlayTime = 1000.0f)
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
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

    // 원하는 State의 Player Animation 속도를 second단위로 설정
    public void SetAnimationSpeed_Second(Player_AniState _animationState, float _SecPlayTime = 1)
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
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
    // 원하는 State의 Enemy Animation 속도를 second단위로 설정
    public void SetAnimationSpeed_Second(Enemy_AniState _animationState, float _SecPlayTime = 1)
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
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

    // 애니메이션 속도를 기본값으로 재설정
    public void AnimationSpeedReset()
    {
        m_animator.speed = DefaultAnimationSpeed;
    }

    #endregion

    #region Animation Loop Setting
    // 원하는 State의 CycleOffset값 조정하는 함수 (쓸지 안쓸지 모르겠으나, 만들어 놓았으니 일단 남김)
    public void SetCycleOffset<T>(T _animationState, float _cycleOffset) where T : System.Enum
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
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

    // 원하는 state의 애니메이션의 Loop Time을 설정하는 함수
    public void SetAnimationLoop<T>(T _animationState, bool _flag) where T : System.Enum
    {
        if (!FunctionParametarCheck(_animationState))
        {
            Debug.LogError("잘못된 형식의 파라메터 전달");
            return;
        }

        // enum값에서 Animation State의 이름 구하는 작업
        string l_stateName = _animationState.ToString();

        if (l_stateName.Contains("_"))
        {
            string[] l_splits = l_stateName.Split('_');
            l_stateName = l_splits[l_splits.Length - 1];
        }

        
        foreach (ChildAnimatorState childState in m_allState)
        {
            // 설정하고싶은 State를 순회하면서 찾음
            if (childState.state.name == l_stateName)
            {
                // 찾은 State의 Animation Clip의 Path정보, 즉 .FBX파일의 경로를 가지고 온뒤 이를 이용하여 FBX Model Importer
                // 를 가져옴.
                string l_assetPath = UnityEditor.AssetDatabase.GetAssetPath(childState.state.motion.GetInstanceID());
                UnityEditor.ModelImporter l_modelImporter = UnityEditor.AssetImporter.GetAtPath(l_assetPath) as UnityEditor.ModelImporter;
                UnityEditor.ModelImporterClipAnimation[] l_clipAni = l_modelImporter.clipAnimations;

                // Model Importer에 등록되어있는 Animation Clip들 중, 우리가 설정하기 원하는 Clip을 찾고,
                // 해당 Clip의 loopTime을 변경한다.
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
            // 설정하고싶은 State를 순회하면서 찾음
            if (childState.state.name == _animationState)
            {
                // 찾은 State의 Animation Clip의 Path정보, 즉 .FBX파일의 경로를 가지고 온뒤 이를 이용하여 FBX Model Importer
                // 를 가져옴.
                string l_assetPath = UnityEditor.AssetDatabase.GetAssetPath(childState.state.motion.GetInstanceID());
                UnityEditor.ModelImporter l_modelImporter = UnityEditor.AssetImporter.GetAtPath(l_assetPath) as UnityEditor.ModelImporter;
                UnityEditor.ModelImporterClipAnimation[] l_clipAni = l_modelImporter.clipAnimations;

                // Model Importer에 등록되어있는 Animation Clip들 중, 우리가 설정하기 원하는 Clip을 찾고,
                // 해당 Clip의 loopTime을 변경한다.
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
    // 자신과, 하위 오브젝트들중 Animator 컴포넌트를 찾아서 리턴하는 함수
    public Animator FindAnimator()
    {
        Animator l_animator = null;
        if (!TryGetComponent<Animator>(out l_animator))
        {
            l_animator = transform.GetComponentInChildren<Animator>();
        }

        return l_animator;
    }

    // 전달받은 State Enum값이 제대로 전달되었는지 체크하는 함수
    // ex) Enemy 오브젝트인데, Player_AniState, Player_AniParametar값 전달하는것 방지
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

    // 현재 Animation Controller에 등록된 State들의 정보를 m_allState 변수에 갱신하는 함수
    public void AllAnimationStateInfoUpdate()
    {
        // 이미 갱신된 값들이 있을경우 Clear함수로 초기화
        if(m_allState.Count >0)
        {
            m_allState.Clear();
        }

        AnimatorController l_aniController;
        ChildAnimatorState[] l_states;

        // 현재 Animator Controller의 Layer 정보들을 받아옴
        l_aniController = m_animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] l_layers = l_aniController.layers;

        // Layer를 순회하면서 등록된 State정보를 m_allState 리스트에 추가
        foreach (AnimatorControllerLayer layer in l_layers)
        {
            l_states = layer.stateMachine.states;

            foreach (ChildAnimatorState state in l_states)
            {
                m_allState.Add(state);
            }
        }     
    }
    // 현재 활성화 되어있는 Bool Parametar를 false로 변경하는 함수
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

    // Animation Layer별로 현재 State정보를 받아오는 함수
    public AnimatorStateInfo GetCurrentStateInfo(string _layer = "Base Layer")
    {
        return m_animator.GetCurrentAnimatorStateInfo(m_animator.GetLayerIndex(_layer));
    }
    #endregion

}