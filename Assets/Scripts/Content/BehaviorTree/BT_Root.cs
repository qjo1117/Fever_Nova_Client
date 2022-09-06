using AI;

public class BT_Root : BehaviorTree
{
    private BehaviorTree m_node = null;

    public BT_Root(BehaviorTree _node = null)
    {
        Parent = null;
        m_node = _node;
    }

    public BehaviorTree Node
    {
        get => m_node;
        set
        {
            m_node = value;
            m_node.Parent = this;
        }
    }

    public override State Tick()
    {
        if (m_node == null)
        {
            /*디버그 - AI*/
            UnityEngine.Debug.Log("ROOT : 하위 노드 없음");
            return State.ERROR;
        }
        return m_node.Tick();
    }
}
