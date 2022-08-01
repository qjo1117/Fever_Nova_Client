using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Sequence : BT_Composite
{
    public BT_Sequence() => NodeType = AI.NodeType.SEQUENCE;

    public override AI.State Update()
    {
        AI.State CurrentState = AI.State.INVALID;

        for (int i = 0; i < ChildListCount; i++)
        {
            CurrentState = GetChild(i).State;

            if (GetChild(i).NodeType != AI.NodeType.ACTION
                || GetChild(i).State != AI.State.SUCCESS)
            {
                CurrentState = GetChild(i).Tick();
            }

            if (CurrentState != AI.State.SUCCESS)
            {
                return CurrentState;
            }
        }
        return AI.State.SUCCESS;
    }
}
