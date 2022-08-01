using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Action : BehaviorTree
{
    public BT_Action() => NodeType = AI.NodeType.ACTION;

    public override void Initialize() { }

    public override void Terminate() { }

    public override void Reset() => State = AI.State.INVALID;

    public override AI.State Tick()
    {
        if (State == AI.State.INVALID)
        {
            Initialize();
            State = AI.State.RUNNING;
        }
        State = Update();
        if (State != AI.State.RUNNING)
        {
            Terminate();
        }
        return State;
    }
}
