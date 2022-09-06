using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Action : BehaviorTree
{
    protected Interface_Enemy m_object = null;
    protected Animator m_animator = null;

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