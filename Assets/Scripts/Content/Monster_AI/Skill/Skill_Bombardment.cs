using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Bombarment : Interface_Skill
{
    private int m_damage = 0;
    private int m_totalSkillCount = 0;
    private int m_currentSkillCount = 0;
    private float m_readyTime = 0.0f;
    private float m_skillRange = 0.0f;
    private float m_localScaleRatio = 0.0f;

    private GameObject m_particle = null;
    private GameObject m_circleForTotalRadius = null;
    private GameObject m_circleForCurrentRadius = null;
    private bool m_isSkillStart = false;

    const string AnimationCrouch = "Shield-Idle-Crouch";


    public Skill_Bombarment(GameObject _object, int _id, float _coolTime, float _range, int _priority,
       int _damage, float _skillRange, float _readyTime,
        int _totalSkillCount)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<Animator>();
        m_id = _id;
        m_coolTime = _coolTime;
        m_coolDown = 0;
        m_rangeForCastSkill = _range;
        m_priority = _priority;
        m_localScaleRatio = m_object.transform.localScale.x;

        m_damage = _damage;
        m_skillRange = _skillRange;
        m_isSkillStart = false;
        m_readyTime = _readyTime;
        m_totalSkillCount = _totalSkillCount;
        m_currentSkillCount = 0;
    }


    protected override AI.State Function()
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
        l_speed.x = l_speed.z = (m_skillRange * m_localScaleRatio) / m_readyTime * Time.deltaTime;
        l_speed.y = 0.0f;
        //서브 인디케이터의 반지름 증가
        m_circleForCurrentRadius.transform.localScale += l_speed;

        if (CheckRadius())
        {
            //스킬을 발동
            Do_Skill();
            m_currentSkillCount++;
            m_object.transform.LookAt(Managers.Game.Player.MainPlayer.transform.position);
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
        // 가져옴
        m_circleForTotalRadius = Managers.Resource.Instantiate(Path.Bombardment_IndiCator);
        m_circleForCurrentRadius = Managers.Resource.Instantiate(Path.Bombardment_Charge_Indicator);

        //위치 세팅
        m_circleForTotalRadius.transform.position =
            new Vector3(m_object.transform.position.x, 0.1f, m_object.transform.position.z);
        m_circleForCurrentRadius.transform.position =
            new Vector3(m_object.transform.position.x, 0.2f, m_object.transform.position.z);
        //스케일 설정
        m_circleForTotalRadius.transform.localScale =
            new Vector3(m_skillRange * m_localScaleRatio, 0.1f, m_skillRange * m_localScaleRatio);
        m_circleForCurrentRadius.transform.localScale =
            new Vector3(0f, 0.1f, 0f);

        //트랜스폼 부모 설정
        m_circleForTotalRadius.SetActive(true);
        m_circleForCurrentRadius.SetActive(true);
        m_isSkillStart = true;
        SetAnimation(AnimationCrouch, 0.15f, 3);

        Managers.Resource.Destroy(m_circleForTotalRadius.gameObject, 3.0f);
        Managers.Resource.Destroy(m_circleForCurrentRadius.gameObject, 3.0f);
    }

    public bool CheckRadius()
    {
        //현재 서브 인디케이터가 메인 인디케이터를 넘어설때
        if (m_circleForCurrentRadius.transform.localScale.x > m_skillRange * m_localScaleRatio)
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

    // 피격판정이 들어가는 시점
    public void Do_Skill()
    {
        //파티클을 불러옴
        ParticleSystem particle = Managers.Resource.Instantiate(Path.Bombardment_Effect, m_object.transform).GetComponent<ParticleSystem>();
        //위치 세팅
        particle.transform.position = new Vector3(m_object.transform.position.x, 0f, m_object.transform.position.z);

        //Distance Calc
        foreach (PlayerController player in Managers.Game.Player.List)
        {
            Vector3 direction = player.transform.position - m_object.transform.position;
            direction.y = 0.0f;
            if (direction.sqrMagnitude < (m_skillRange + m_skillRange) * (m_skillRange + m_skillRange))
            {
                player.Damage(m_damage);
            }
        }

        //파티클 삭제
        Managers.Resource.Destroy(particle.gameObject, 5.0f);
    }



    public void DeleteIndicaotr()
    {


        m_isSkillStart = false;
        m_currentSkillCount = 0;
    }
}
