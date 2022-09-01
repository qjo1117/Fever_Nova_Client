using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Chase : BT_Action
{
    private float m_ChaseMoveSpeed = 0.0f;

    public Action_Chase(GameObject _object, float _ChaseMoveSpeed)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<Animator>();
        m_ChaseMoveSpeed = _ChaseMoveSpeed;
    }

    public override AI.State Update()
    {
        return Chase();
    }

    private AI.State Chase()
    {
        if (!m_object.m_isPlayingChaseAnimation)
        {
            m_animator.speed = 1;
            m_animator.CrossFade("Move", 0.15f);
            m_object.m_isPlayingChaseAnimation = true;
        }

        Vector3 Dir = Managers.Game.Player.MainPlayer.transform.position - m_object.transform.position;
        Dir = new Vector3(Dir.x, 0, Dir.z);

        m_object.transform.rotation = Quaternion.Slerp
            (m_object.transform.rotation,
            Quaternion.LookRotation(Dir),
            Time.deltaTime * 12.5f);

        m_object.transform.Translate(Vector3.forward * m_ChaseMoveSpeed * Time.deltaTime);
        return AI.State.RUNNING;
    }
}
