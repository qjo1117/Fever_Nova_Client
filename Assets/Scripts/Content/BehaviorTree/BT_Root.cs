public class BT_Root : BehaviorTree
{
    private BehaviorTree m_child;

    public BT_Root()
    {
        NodeType = AI.NodeType.ROOT;
        Parent = null;
    }

    public BehaviorTree Child
    {
        get => m_child;
        set
        {
            m_child = value;
            m_child.Parent = this;
        }
    }

    public override void Terminate()
    {
        m_child.Terminate();
        base.Terminate();
    }

    public override AI.State Tick()
    {
        if (m_child == null)
        {
            return AI.State.INVALID;
        }
        else if (m_child.State == AI.State.INVALID)
        {
            m_child.Initialize();
            m_child.State = AI.State.RUNNING;
        }

        m_child.State = State = m_child.Update();

        if (State != AI.State.RUNNING)
        {
            Terminate();
        }
        return State;
    }
}
