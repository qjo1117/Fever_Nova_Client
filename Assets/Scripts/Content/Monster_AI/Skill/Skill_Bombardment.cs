using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Bombarment : Interface_Skill
{
    private int m_damage;
    private int m_totalSkillCount;
    private int m_currentSkillCount;
    private float m_readyTime;
    private float m_skillRange;

    private GameObject m_particle;
    private GameObject m_circleForTotalRadius;
    private GameObject m_circleForCurrentRadius;

    private bool m_isSkillStart;


    public Skill_Bombarment(GameObject _object, int _id, float _coolTime, float _range, int _priority,
       int _damage, float _skillRange, float _readyTime,
        int _totalSkillCount)
    {
        m_object = _object;
        m_animator = m_object.GetComponent<Animator>();
        m_id = _id;
        m_coolTime = _coolTime;
        m_coolDown = 0;
        m_rangeForCastSkill = _range;
        m_priority = _priority;

        m_damage = _damage;
        m_skillRange = _skillRange;
        m_isSkillStart = false;
        m_readyTime = _readyTime;
        m_totalSkillCount = _totalSkillCount;
        m_currentSkillCount = 0;

        if (m_circleForTotalRadius == null)
        {
            m_circleForTotalRadius = Managers.Resource.Instantiate(Path.Bombardment_IndiCator);
            m_circleForTotalRadius.transform.SetParent(m_object.transform);
            m_circleForTotalRadius.SetActive(false);
        }
        if (m_circleForCurrentRadius == null)
        {
            m_circleForCurrentRadius = Managers.Resource.Instantiate(Path.Bombardment_Charge_Indicator);
            m_circleForCurrentRadius.transform.SetParent(m_object.transform);
            m_circleForTotalRadius.SetActive(false);
        }
    }


    public override AI.State Update()
    {
        if (!m_isSkillStart)
        {
            DrawCircle();
        }
        else
        {
            return On_BomBardment();
        }
        return AI.State.RUNNING;
    }

    public AI.State On_BomBardment()
    {
        Vector3 l_speed = new Vector3();
        l_speed.x = l_speed.z = m_skillRange * 1 / m_readyTime * Time.deltaTime;
        l_speed.y = 0.0f;
        //서브 인디케이터의 반지름 증가
        m_circleForCurrentRadius.transform.localScale += l_speed;
        if (CheckRadius())
        {
            //스킬을 발동
            Do_Skill();
            m_currentSkillCount++;
        }
        if (m_currentSkillCount == m_totalSkillCount)
        {
            //인디케이터 삭제
            DeleteIndicaotr();
            return AI.State.SUCCESS;
        }
        return AI.State.RUNNING;
    }

    public void DrawCircle()
    {
        //위치 세팅
        m_circleForTotalRadius.transform.position =
            new Vector3(m_object.transform.position.x, 0.1f, m_object.transform.position.z);
        m_circleForCurrentRadius.transform.position =
            new Vector3(m_object.transform.position.x, 0.2f, m_object.transform.position.z);
        //스케일 설정
        m_circleForTotalRadius.transform.localScale =
            new Vector3(m_skillRange, 0.1f, m_skillRange);
        m_circleForCurrentRadius.transform.localScale =
            new Vector3(0f, 0.1f, 0f);
        //트랜스폼 부모 설정
        m_circleForTotalRadius.SetActive(true);
        m_circleForCurrentRadius.SetActive(true);
        m_isSkillStart = true;
        SetAnimation("Shield-Idle-Crouch", 0.15f, 3);
    }

    public bool CheckRadius()
    {
        //현재 서브 인디케이터가 메인 인디케이터를 넘어설때
        if (m_circleForCurrentRadius.transform.localScale.x > m_skillRange)
        {
            //Sub_Skill_Radius_Reset
            m_circleForCurrentRadius.transform.localScale = new Vector3(0f, 0.2f, 0f);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Do_Skill()
    {
        //파티클을 불러옴
        m_particle = Managers.Resource.Instantiate(Path.Bombardment_Effect, m_object.transform);
        //위치 세팅
        m_particle.transform.position = new Vector3(m_object.transform.position.x, 0f, m_object.transform.position.z);
        //부모 설정
        m_particle.transform.SetParent(m_object.transform);

        //Distance Calc
        Vector3 l_Dir = Managers.Game.Player.MainPlayer.transform.position - m_object.transform.position;
        //Distance
        float l_Dis = l_Dir.magnitude;
        if (l_Dis < m_skillRange)
        {
            //컨트롤러 받아서 데미지 부여
            PlayerController l_Con = Managers.Game.Player.MainPlayer.GetComponent<PlayerController>();
            l_Con.Damage(m_damage);
        }
        //파티클 삭제
        Managers.Resource.Destroy(m_particle, 5.0f);
    }

    public void DeleteIndicaotr()
    {
        m_circleForTotalRadius.SetActive(false);
        m_circleForCurrentRadius.SetActive(false);
        m_isSkillStart = false;
        m_currentSkillCount = 0;
    }
}
