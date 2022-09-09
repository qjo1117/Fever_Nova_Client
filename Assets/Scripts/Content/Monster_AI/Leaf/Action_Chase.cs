using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Chase : BT_Action
{
    private float m_ChaseMoveSpeed = 0.0f;

    public Action_Chase(GameObject _object, float _ChaseMoveSpeed)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<MyAnimator>();
        m_ChaseMoveSpeed = _ChaseMoveSpeed;
    }

    public override AI.State Tick()
    {
        return Chase();
    }

    private AI.State Chase()
    {
        // 다시 Chase 함수가 호출됬을시, Chase Animation이 진행중일 경우 (전 Tick에 Chase가 호출되었을경우)
        if (!m_object.m_isPlayingChaseAnimation)
        {
            m_animator.AnimationSpeedReset();
            m_animator.SetBool(AI.Enemy_AniParametar.MoveFlag, true);
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
