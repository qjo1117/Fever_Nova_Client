using System.Collections.Generic;

public class BT_Composite : BehaviorTree
{
    protected List<BehaviorTree> m_nodeList = new List<BehaviorTree>();

    public BT_Composite(params BehaviorTree[] _nodes)
    {
        for (int i = 0; i < _nodes.Length; i++)
        {
            m_nodeList.Add(_nodes[i]);
        }
    }

    public void Attach(BehaviorTree _node)
    {
        m_nodeList.Add(_node);
        _node.Parent = this;
    }
}
