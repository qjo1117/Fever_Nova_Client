using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEditor;

public class Skill_Melee : Interface_Skill
{
    private int m_damage = 10;
    private float m_skillPlayTime = 0.0f;

    private float m_attackAngle = 135f;
    private ParticleSystem m_particle = null;
    private List<GameObject> m_targets = new List<GameObject>();

    private bool m_isSkillEnd = false;
    private float m_timeCheck = 0.0f;

    private string m_animationFileName;
    private string m_effectFileName;

    private List<int> m_hitId = new List<int>();

    public Skill_Melee(GameObject _object, int _id, float _coolTime, float _range, int _priority,
        int _damage, float _skillPlayTime,
        string _animationFileName = "Dagger-Attack-R3", string _effectFileName = Path.Slash_Particle)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<Animator>();
        m_id = _id;
        m_coolTime = _coolTime;
        m_coolDown = 0;
        m_rangeForCastSkill = _range;
        m_priority = _priority;

        m_damage = _damage;
        m_skillPlayTime = _skillPlayTime;

        m_isSkillEnd = false;
        m_timeCheck = 0;

        m_animationFileName = _animationFileName;
        m_effectFileName = _effectFileName;
    }

    public override void Initialize() { }
    public override void Terminate() { }

    public override AI.State Update()
    {
        return OnMeleeAttack();
    }

    private AI.State OnMeleeAttack()
    {
        if (!m_isSkillEnd)
        {


            if (m_particle == null)
            {
                m_particle = Managers.Resource.Instantiate(m_effectFileName, m_object.transform).GetComponent<ParticleSystem>();

                //위치 조정
                Vector3 vec = m_object.transform.position + m_object.transform.forward;
                //vec = new Vector3(vec.x, vec.y + 2, vec.z);
                vec.y += 2.0f;
                m_particle.transform.position = vec;

                //회전 회오리
                m_particle.transform.Rotate(Quaternion.FromToRotation(m_particle.transform.forward,
                    m_object.transform.forward).eulerAngles);

                m_particle.Stop();

                var l_particle = m_particle.GetComponent<ParticleSystem>().main;
                l_particle.duration = m_skillPlayTime;
                l_particle.startLifetime = m_skillPlayTime;

            }
            m_particle.Play();
            SetAnimation(m_animationFileName, 0.15f, m_skillPlayTime);
            m_isSkillEnd = true;

            m_hitId.Clear();
        }

        m_timeCheck += Time.deltaTime;

        if ((int)m_skillPlayTime * 5 == (int)(m_timeCheck * 10))
        {
            FindTarget();
            foreach (GameObject obj in m_targets)
            {
                PlayerController player = obj.GetComponent<PlayerController>();

                if(CheckHitID(player.StatTable.id) == true) {
                    continue;
				}

                player.Damage(m_damage);
                m_hitId.Add(player.StatTable.id);
            }
        }

        if (m_timeCheck >= m_skillPlayTime)
        {
            m_timeCheck = 0;
            m_isSkillEnd = false;


            return AI.State.SUCCESS;
        }

        return AI.State.RUNNING;
    }

    //범위 내의 타켓 탐색
    void FindTarget()
    {
        GameObject l_player = Managers.Game.Player.MainPlayer.gameObject;

        m_targets.Clear();//초기화

        float dotValue = Mathf.Cos(Mathf.Deg2Rad * (m_attackAngle / 2));
        Vector3 direction = l_player.transform.position - m_object.transform.position;
        if (direction.magnitude < m_rangeForCastSkill)
        {
            if (Vector3.Dot(direction.normalized, m_object.transform.forward) > dotValue)
            {
                m_targets.Add(l_player.gameObject);
            }
        }
    }


    public bool CheckHitID(int _id)
	{
        foreach(int id in m_hitId) {
            if(_id == id) {
                return true;
			}
		}
        return false;
	}
}


