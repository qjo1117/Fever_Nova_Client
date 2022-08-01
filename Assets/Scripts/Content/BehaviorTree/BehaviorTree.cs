public class BehaviorTree
{
    private AI.State m_state;
    private AI.NodeType m_nodeType;
    private int m_index;
    private BehaviorTree m_parent;

    public BehaviorTree() => m_state = AI.State.INVALID;

    public BehaviorTree Parent
    {
        get => m_parent;
        set => m_parent = value;
    }

    public AI.State State
    {
        get => m_state;
        set => m_state = value;
    }

    public AI.NodeType NodeType
    {
        get => m_nodeType;
        set => m_nodeType = value;
    }

    public int Index
    {
        get => m_index;
        set => m_index = value;
    }

    public virtual void Reset() => m_state = AI.State.INVALID;

    public bool IsTerminated() => m_state == AI.State.SUCCESS || m_state == AI.State.FAILURE;

    public bool IsRunning() => m_state == AI.State.RUNNING;

    public virtual AI.State Update() => AI.State.SUCCESS;

    public virtual AI.State Tick()
    {
        if (m_state == AI.State.INVALID)
        {
            Initialize();
            m_state = AI.State.RUNNING;
        }

        m_state = Update();

        if (m_state != AI.State.RUNNING)
        {
            Terminate();
        }
        return m_state;
    }

    public virtual void Initialize() { }

    public virtual void Terminate() { }
}
