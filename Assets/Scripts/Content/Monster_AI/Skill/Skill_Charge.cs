using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1.경로 고정
//      스킬이 시작되면 경로가 고정되고 잠시 후 그 경로를 따라 움직이게 해야함
// 2.돌진 거리 고정
//      돌진 시 이동거리를 고정해야함
// 3.충돌처리방식
//      계속 콜라이더 불러서 처리하는거 비효율적


public class Skill_Charge : AI_Skill
{
    private int m_damage;
    private float m_chargeSpeed;
    private float m_readyTime;
    private float m_currentTime;
    private Vector3 m_colliderSize = new Vector3();
    private Transform m_shieldPosition;


    private float m_lineRedererHeight;
    private Material m_material;
    private LineRenderer m_line;

    private GameObject m_player;
    private Vector3 m_targetPosition;

    private bool m_isSkillStart;
    private bool m_isSkillEnd;

    public Skill_Charge(GameObject _object, int _id, float _coolTime, float _range, int _priority,
         int _damage, float _chargeSpeed, float _readyTime, Vector3 _colliderSize)
    {
        m_object = _object;
        m_id = _id;
        m_coolTime = _coolTime;
        m_coolDown = 0;
        m_range = _range;
        m_priority = _priority;


        m_damage = _damage;
        m_chargeSpeed = _chargeSpeed;
        m_readyTime = _readyTime;
        m_currentTime = 0f;
        m_colliderSize = _colliderSize;
        m_shieldPosition = m_object.transform.Find("SM_Wep_Shield_01");


        m_lineRedererHeight = 0.1f;
        m_material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        m_line = SetUpLine();


        m_player = Managers.Game.Player.MainPlayer.gameObject;
        m_targetPosition = Vector3.zero;


        m_isSkillStart = false;
        m_isSkillEnd = false;
    }

    public override AI.State Update()
    {
        return ChargeAttack();
    }

    public LineRenderer SetUpLine()
    {
        Transform l_SkillRange = new GameObject("Skill_Range").transform;

        l_SkillRange.gameObject.AddComponent<LineRenderer>();
        LineRenderer l_returnLine = l_SkillRange.GetComponent<LineRenderer>();

        l_returnLine.useWorldSpace = true;
        l_returnLine.startWidth = 1.0f;
        l_returnLine.endWidth = 1.0f;
        l_returnLine.positionCount = 2;
        l_returnLine.material = m_material;
        float l_alpha = 0.6f;

        Gradient l_gradient = new Gradient();
        l_gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(l_alpha, 0.0f), new GradientAlphaKey(l_alpha, 1.0f) });

        l_returnLine.colorGradient = l_gradient;
        l_returnLine.enabled = false;

        return l_returnLine;
    }

    public void DrawLine()
    {
        Vector3 l_tail =
            new Vector3(m_object.transform.position.x, m_lineRedererHeight, m_object.transform.position.z);
        Vector3 l_head = m_targetPosition =
            new Vector3(m_player.transform.position.x, m_lineRedererHeight, m_player.transform.position.z);

        m_line.SetPosition(0, l_tail);
        m_line.SetPosition(1, l_head);
        m_line.enabled = true;

        m_object.transform.LookAt(new Vector3(m_player.transform.position.x, m_object.transform.position.y, m_player.transform.position.z));
    }

    public AI.State ChargeAttack()
    {
        if (!m_isSkillEnd)
        {
            if (!m_isSkillStart)
            {
                DrawLine();
                m_isSkillStart = true;
            }

            if (m_currentTime < m_readyTime)
            {
                m_currentTime += Time.deltaTime;
                return AI.State.RUNNING;
            }


            m_line.enabled = false;
            m_object.transform.Translate(m_object.transform.forward * m_chargeSpeed * Time.deltaTime);


            Collider[] target = Physics.OverlapBox(m_shieldPosition.position + m_object.transform.forward.normalized, m_colliderSize, m_shieldPosition.rotation);
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i].name == "Player")
                {
                    PlayerController main_player = target[i].GetComponent<PlayerController>();
                    main_player.Damage(m_damage);
                }
            }

            if (Vector3.Distance(m_targetPosition, m_object.transform.position) <= 5.0f)
            {
                m_currentTime = 0;
                m_isSkillStart = false;
                m_isSkillEnd = true;
            }
        }
        else
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                m_isSkillEnd = false;
                return AI.State.SUCCESS;
            }
        }
        return AI.State.RUNNING;
    }

    protected void Anime()
    {
        Animator anime = m_object.GetComponent<Animator>();
        if (m_currentTime > m_readyTime)
        {
            // Charge anime()
        }
        else
        {
            // Ready anime()
        }
    }
}