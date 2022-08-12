using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_IsOutofSkillRange : BT_Condition
{
    private GameObject m_object;

    public Condition_IsOutofSkillRange(GameObject _object)
    {
        m_object = _object;
    }

    public override void Initialize()
    {

    }

    public override void Terminate() { }

    public override AI.State Update()
    {
        return IsOutofSkillRange();
    }

    private AI.State IsOutofSkillRange()
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
            <= m_object.GetComponent<Interface_Enemy>().m_selectedSkill.Range)
        {
            m_object.GetComponent<Interface_Enemy>().m_isChaseComplete = true;
            m_object.GetComponent<Interface_Enemy>().m_isPlayingChaseAnimation = false;
            return AI.State.FAILURE;
        }

        return AI.State.SUCCESS;
    }
}