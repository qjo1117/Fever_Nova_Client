using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Combat_Chase : BT_Action
{
    private AI_Skill m_selectedSkill;
    private float m_moveSpeed;
    private bool m_isPlayingAnimation;

    public AI_Combat_Chase(GameObject _object, AI_Skill _selectedSkill, float _moveSpeed)
    {
        m_object = _object;
        m_selectedSkill = _selectedSkill;
        m_moveSpeed = _moveSpeed;
        m_isPlayingAnimation = false;
    }

    public override void Initialize()
    {

    }

    public override void Terminate() { }

    public override AI.State Update()
    {
        return OnChase();
    }

    private AI.State OnChase()
    {
        // TODO : -박- 부탁하나 하자고 하면 Player를 제발 Find해서 실시간으로 찾지말아줘
        GameObject player = Managers.Game.Player.MainPlayer.gameObject;
        if (player)
        {
            if (!m_isPlayingAnimation)
            {
                m_isPlayingAnimation = true;
                m_animator.CrossFade("Move", 0.15f);
            }

            Vector3 Dir = player.transform.position - m_object.transform.position;
            Dir = new Vector3(Dir.x, 0, Dir.z);

            m_object.transform.rotation = Quaternion.Slerp
                (m_object.transform.rotation,
                Quaternion.LookRotation(Dir),
                Time.deltaTime * 4);

            m_object.transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);

            if (Vector3.Distance(player.transform.position, m_object.transform.position) <= m_selectedSkill.Range)
            {
                return AI.State.SUCCESS;
            }

            return AI.State.RUNNING;
        }

        return AI.State.FAILURE;
    }
}
