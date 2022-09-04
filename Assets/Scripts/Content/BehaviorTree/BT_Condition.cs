using AI;


public class BT_Condition : BehaviorTree
{
    protected BehaviorTree m_node = null;

    public BT_Condition(BehaviorTree _node)
    {
        m_node = _node;
    }

    public void Attach(BehaviorTree _node)
    {
        m_node = _node;
        _node.Parent = this;
    }

    public override State Tick()
    {
        State CurrentState = Function();
        if (CurrentState == State.SUCCESS)
        {
            return m_node.Tick();
        }
        return CurrentState;
    }
}
