using AI;

public class BT_Sequence : BT_Composite
{
    public BT_Sequence(params BehaviorTree[] _nodes) : base(_nodes) { }

    protected override State Function()
    {
        State CurrentState = State.FAILURE;

        for (int i = 0; i < m_nodeList.Count; i++)
        {
            CurrentState = m_nodeList[i].Tick();
            if (CurrentState != State.SUCCESS)
            {
                return CurrentState;
            }
        }
        return State.SUCCESS;
    }
}