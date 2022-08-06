using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class General_AI_Action_Attack : BT_Action
{
    private GameObject m_object;
    private Transform attack_Point;
    private float m_rotate_speed = 3f;
    private float m_skill_cooltime = 0f;
    private float m_current_time = 0f;
    
    public General_AI_Action_Attack(GameObject _object,float skill_cooltime,Transform transform)
    {
        attack_Point = transform;
        m_object = _object;
        m_skill_cooltime = skill_cooltime;
    }
    public override void Initialize()
    {
        SetStateColor();
    }

    private void SetStateColor()
    {
        m_object.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public override void Terminate() { }

    public override AI.State Update()
    {
       return OnAttack();
    }

    private AI.State OnAttack()
    {
        m_current_time += Time.deltaTime;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Vector3 Dir = player.transform.position - m_object.transform.position;
            Dir = new Vector3(Dir.x, m_object.transform.position.y, Dir.z);

            m_object.transform.rotation = Quaternion.Slerp
                (m_object.transform.rotation,
                Quaternion.LookRotation(Dir),
                Time.deltaTime * 4);

            //현재시간이 쿨타임값을 넘어서면
            if (m_current_time < m_skill_cooltime)
            {
                return AI.State.RUNNING;
            }
            //공격 시키고
            //현재 타입값 초기화
            //성공을 리턴
            m_current_time = 0f;
                return AI.State.FAILURE;
        }
        return AI.State.FAILURE;
    }
    IEnumerable Attack()
    {
        yield return 1;
    }
}
    
