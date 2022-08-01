public class BT_Selector : BT_Composite
{
    public BT_Selector() => NodeType = AI.NodeType.SELECTOR;

    public override AI.State Update()
    {
        for (int i = 0; i < ChildListCount; ++i)
        {
            AI.State CurrentState = GetChild(i).Tick();
            if (CurrentState != AI.State.FAILURE)
            {
                ClearChild(i);
                return CurrentState;
            }
        }
        return AI.State.FAILURE;
    }

    protected void ClearChild(int _index)
    {
        for (int i = 0; i < ChildListCount; ++i)
        {
            if (i != _index)
            {
                GetChild(i).Reset();
            }
        }
    }
}
