using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Action : BehaviorTree
{
    protected Interface_Enemy m_object = null;
    protected Animator m_animator = null;

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

    public void SetAnimation(string _filename, float _timing, float _playTime = 1)
    {
        AnimationClip[] l_clips = m_animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip _clip in l_clips)
        {
            if (_clip.name.Contains(_filename))
            {
                m_animator.speed = _clip.length / _playTime;
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(_filename))
                {
                    m_animator.Play(_filename, 0, 0);
                }
                else
                {
                    m_animator.CrossFade(_filename, _timing);
                }
                return;
            }
        }
    }
}