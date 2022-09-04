using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Charge : Interface_Skill
{
    private int m_damage = 0;
    private float m_chargeSpeed = 0.0f;
    private float m_readyTime = 0.0f;
    private float m_currentTime = 0.0f;
    private Vector3 m_colliderSize = new Vector3();

    private float m_lineRedererHeight = 0.0f;
    private Material m_material = null;
    private LineRenderer m_line = null;

    private GameObject m_player = null;
    private Vector3 m_targetPosition = Vector3.zero;
    private bool m_isSkillStart = false;
    private List<int> m_hitId = new List<int>();

    const string AnimationFowardCharge = "Shield-Run-Forward-Charge";
    const string AnimationIdle = "Shield-Idle-Crouch";

    public Skill_Charge(GameObject _object, int _id, float _coolTime, float _range, int _priority,
         int _damage, float _chargeSpeed, float _readyTime, Vector3 _colliderSize)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<Animator>();
        m_id = _id;
        m_coolTime = _coolTime;
        m_coolDown = 0;
        m_rangeForCastSkill = _range;
        m_priority = _priority;

        m_damage = _damage;
        m_chargeSpeed = _chargeSpeed;
        m_readyTime = _readyTime;
        m_currentTime = 0f;
        m_colliderSize = _colliderSize;

        m_lineRedererHeight = 0.1f;
        m_material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        m_line = SetUpLine();

        m_player = null;
        m_targetPosition = Vector3.zero;

        m_isSkillStart = false;
    }

    protected override AI.State Function()
    {
        return ChargeAttack();
    }

    public LineRenderer SetUpLine()
    {
        Transform l_SkillRange = new GameObject("Skill_Range").transform;
        LineRenderer l_returnLine = l_SkillRange.gameObject.AddComponent<LineRenderer>();

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

        l_SkillRange.transform.SetParent(m_object.transform);

        return l_returnLine;
    }

    public void DrawLine()
    {
        Vector3 l_tail =
            new Vector3(m_object.transform.position.x, m_lineRedererHeight, m_object.transform.position.z);
        // Head와 Tail을 가져올때 TargetPosition을 같이 맵핑한다.
        Vector3 l_head = m_targetPosition =
            new Vector3(m_player.transform.position.x, m_lineRedererHeight, m_player.transform.position.z);

        m_line.SetPosition(0, l_tail);
        m_line.SetPosition(1, l_head);
        m_line.enabled = true;

        m_object.transform.LookAt(new Vector3(m_player.transform.position.x, 0, m_player.transform.position.z));

        m_hitId.Clear();
    }

    public AI.State ChargeAttack()
    {
        if (!m_isSkillStart)
        {
            m_player = Managers.Game.Player.MainPlayer.gameObject;
            DrawLine();
            m_isSkillStart = true;
            SetAnimation(AnimationIdle, 0.15f, m_readyTime);
        }

        if (m_currentTime < m_readyTime)
        {
            m_currentTime += Time.deltaTime;
            return AI.State.RUNNING;
        }

        SetAnimation(AnimationFowardCharge, 0.15f);
        m_line.enabled = false;
        m_object.transform.position = Vector3.MoveTowards(m_object.transform.position, m_targetPosition, m_chargeSpeed * Time.deltaTime);

        Collider[] target = Physics.OverlapBox(m_object.transform.position + m_object.transform.forward.normalized, m_colliderSize, m_object.transform.rotation);
        foreach (Collider collider in target)
        {
            int layer = collider.gameObject.layer;
            if (layer != (int)Define.Layer.Player)
            {
                continue;
            }
            PlayerController player = collider.GetComponent<PlayerController>();

            // 이미 피격한 상대면 피격을 피한다.
            if (CheckHitId(player.StatTable.id) == true)
            {
                continue;
            }
            player.Damage(m_damage);
            m_hitId.Add(player.StatTable.id);
        }

        if (Vector3.Distance(m_targetPosition, m_object.transform.position) <= 3.0f)
        {
            m_currentTime = 0;
            m_isSkillStart = false;
            return AI.State.SUCCESS;
        }

        return AI.State.RUNNING;
    }

    // 피격했던 캐릭터인지 확인한다.
    public bool CheckHitId(int _id)
    {
        foreach (int id in m_hitId)
        {
            if (id == _id)
            {
                return true;
            }
        }
        return false;
    }
}