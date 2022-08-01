using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Condition : BehaviorTree
{
    public BT_Condition() => NodeType = AI.NodeType.CONDITION;

    public override AI.State Tick()
    {
        State = Update();

        if (State == AI.State.RUNNING)
        {
            //error function
        }

        if (State == AI.State.SUCCESS)
        {
            TerminateRunningStateByOtherAction();
        }

        return State;
    }

    public void TerminateRunningStateByOtherAction()
    {
        BehaviorTree FindRoot = Parent;
        int ErrorCount = 0;

        if (FindRoot != null)
        {
            while (ErrorCount < 100)
            {
                FindRoot = FindRoot.Parent;
                if (FindRoot.Parent == null)
                {
                    break;
                }
                ++ErrorCount;
            }
        }

        if (FindRoot != null)
        {
            if (FindRoot.State == AI.State.RUNNING)
            {
                BehaviorTree RunningAction = FindRunningAction(((BT_Root)FindRoot).Child);
                if (RunningAction != null)
                {
                    if (Parent != RunningAction.Parent
                        || Parent.NodeType != AI.NodeType.SEQUENCE)
                    {
                        RunningAction.Terminate();
                    }
                }
            }
        }
    }

    public BehaviorTree FindRunningAction(BehaviorTree _child)
    {
        BehaviorTree RunningAction = null;

        if (_child != null)
        {
            if (_child.NodeType == AI.NodeType.SELECTOR
                || _child.NodeType == AI.NodeType.SEQUENCE)
            {
                for (int i = 0; i < ((BT_Composite)_child).ChildListCount; ++i)
                {
                    RunningAction = FindRunningAction(((BT_Composite)_child).GetChild(i));
                    if (RunningAction != null)
                    {
                        return RunningAction;
                    }
                }
            }
            if (_child.NodeType == AI.NodeType.ACTION && _child.State == AI.State.RUNNING)
            {
                return _child;
            }
        }
        return RunningAction;
    }
}
