using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Composite : BehaviorTree
{
    protected List<BehaviorTree> m_childList;

    public BT_Composite() => m_childList = new List<BehaviorTree>();

    public override void Reset()
    {
        foreach (BehaviorTree child in m_childList)
        {
            child.Reset();
        }
    }

    public BehaviorTree GetChild(int _index) => m_childList[_index];

    public void AddChild(BehaviorTree _child)
    {
        m_childList.Add(_child);
        _child.Index = m_childList.Count - 1;
        _child.Parent = this;
    }

    public int ChildListCount => m_childList.Count;
}
