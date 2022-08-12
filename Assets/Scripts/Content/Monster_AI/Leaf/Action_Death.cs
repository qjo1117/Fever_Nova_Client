using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Death : BT_Action
{
    private float m_corpseTime;

    public Action_Death(GameObject _object, float _corpseTime)
    {
        m_object = _object;
        m_animator = m_object.GetComponent<Animator>();
        m_corpseTime = _corpseTime;
    }

    public override AI.State Update()
    {
        return Death();
    }

    private AI.State Death()
    {
        m_animator.CrossFade("Death", 0.15f);
        m_corpseTime -= Time.deltaTime;
        if (m_corpseTime < 0)
        {
            //TODO
            //오브젝트 풀로 반환
            //필요하다면 시체 사라지는 이펙트?
        }

        return AI.State.RUNNING;
    }
}
