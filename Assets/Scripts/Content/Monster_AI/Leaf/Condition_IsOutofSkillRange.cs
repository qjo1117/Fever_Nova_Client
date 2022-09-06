using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_IsOutofSkillRange : BT_Condition
{
    private Interface_Enemy m_object = null;

    public Condition_IsOutofSkillRange(GameObject _object, BehaviorTree _node = null) : base(_node)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
    }

    protected override AI.State Function()
    {
        if (m_object.GetComponent<Interface_Enemy>().m_isChaseComplete)
        {
            return AI.State.FAILURE;
        }

        if (Vector2.Distance(
            new Vector2(
                Managers.Game.Player.MainPlayer.gameObject.transform.position.x,
                Managers.Game.Player.MainPlayer.gameObject.transform.position.z),
            new Vector2(
                m_object.transform.position.x,
                m_object.transform.position.z))
            <= m_object.m_selectedSkill.Range)
        {
            m_object.m_isChaseComplete = true;
            m_object.m_isPlayingChaseAnimation = false;
            return AI.State.FAILURE;
        }

        return AI.State.SUCCESS;
    }
}