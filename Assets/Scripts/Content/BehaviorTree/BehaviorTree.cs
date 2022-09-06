using AI;

public class BehaviorTree
{
    private BehaviorTree m_parent;

    public BehaviorTree Parent
    {
        get => m_parent;
        set => m_parent = value;
    }

    // 오버라이드 해서 사용할 필요한 작업
    protected virtual AI.State Function()
    {
        return AI.State.SUCCESS;
    }

    // 각 노드 진입점
    public virtual AI.State Tick()
    {
        return Function();
    }
}
