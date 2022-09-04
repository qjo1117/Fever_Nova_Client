using AI;

public class BT_Selector : BT_Composite
{
    public BT_Selector(params BehaviorTree[] _nodes) : base(_nodes) { }

    protected override State Function()
    {
        State CurrentState;
        for (int i = 0; i < m_nodeList.Count; ++i)
        {
            CurrentState = m_nodeList[i].Tick();
            if (CurrentState != State.FAILURE)
            {
                return CurrentState;
            }
        }
        return State.FAILURE;
    }
}