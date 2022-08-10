using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Action : BehaviorTree
{
    protected GameObject m_object;
    protected Animator m_animator;

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

    public object Copy()
    {
        return this.MemberwiseClone();
    }

    public void SetAnimation(string _stateName, float _timing)
    {
        if (m_animator == null)
        {
            m_object.GetComponent<Animator>();
        }

        m_animator.CrossFade(_stateName, _timing);
    }
}