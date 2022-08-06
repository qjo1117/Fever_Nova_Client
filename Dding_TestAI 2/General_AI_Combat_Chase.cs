using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_AI_Combat_Chase : BT_Action
{
    private GameObject m_object;
    private float m_moveSpeed;

    public General_AI_Combat_Chase(GameObject _object, float _moveSpeed)
    {
        m_object = _object;
        m_moveSpeed = _moveSpeed;
    }

    public override void Initialize()
    {
        SetStateColor();
    }

    private void SetStateColor()
    {
        m_object.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public override void Terminate() { }

    public override AI.State Update()
    {
        OnChase();
        return AI.State.RUNNING;
    }

    private void OnChase()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Vector3 Dir = player.transform.position - m_object.transform.position;

            m_object.transform.rotation = Quaternion.Slerp
                (m_object.transform.rotation,
                Quaternion.LookRotation(Dir),
                Time.deltaTime * 4);

            m_object.transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);
        }
    }
}
