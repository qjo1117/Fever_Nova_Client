using AI;

public class BehaviorTree
{
    private BehaviorTree m_parent;

    public BehaviorTree Parent
    {
        get => m_parent;
        set => m_parent = value;
    }

    // �������̵� �ؼ� ����� �ʿ��� �۾�
    protected virtual AI.State Function()
    {
        return AI.State.SUCCESS;
    }

    // �� ��� ������
    public virtual AI.State Tick()
    {
        return Function();
    }
}
