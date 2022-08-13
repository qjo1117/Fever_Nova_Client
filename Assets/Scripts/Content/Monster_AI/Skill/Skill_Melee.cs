using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class Skill_Melee : Interface_Skill
{
    private int m_damage = 10;
    private float m_skillPlayTime = 0.0f;

    private float m_attackAngle = 135f;
    private ParticleSystem m_particle = null;
    private List<PlayerController> m_targets = new List<PlayerController>();

    private bool m_isSkillEnd = false;
    private float m_timeCheck = 0.0f;

    private string m_animationFileName;
    private string m_effectFileName;

    public Skill_Melee(GameObject _object, int _id, float _coolTime, float _range, int _priority,
        int _damage, float _skillPlayTime, 
        string _animationFileName = "Dagger-Attack-R3", string _effectFileName = Path.Slash_Particle)
    {
        m_object = _object;
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
        FindTarget();

        if (!m_isSkillEnd) {
            if (m_particle == null) {
                m_particle = Managers.Resource.Instantiate(m_effectFileName, m_object.transform).GetComponent<ParticleSystem>();

                //��ġ ����
                Vector3 vec = m_object.transform.position + m_object.transform.forward;
                //vec = new Vector3(vec.x, vec.y + 2, vec.z);
                vec.y += 2.0f;
                m_particle.transform.position = vec;

                //ȸ�� ȸ����
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
        }

        m_timeCheck += Time.deltaTime;

        if (m_timeCheck >= m_skillPlayTime) {
            m_timeCheck = 0;
            m_isSkillEnd = false;
            foreach (var player in m_targets) {
                player.GetComponent<PlayerController>().Damage(m_damage);
            }
            Managers.Game.Player.MainPlayer.Damage(m_damage);
            return AI.State.SUCCESS;
        }

        return AI.State.RUNNING;
    }

    //���� ���� Ÿ�� Ž��
    void FindTarget()
    {
        //Ư�� �Ÿ� ���� player Ž��
        Collider[] l_colliders = Physics.OverlapSphere(m_object.transform.position, m_rangeForCastSkill, 1 << (int)Define.Layer.Player);

        m_targets.Clear();//�ʱ�ȭ

        float l_radianRange = Mathf.Cos(Mathf.Deg2Rad * (m_attackAngle / 2));//���� ����

        //���� ���� player Ÿ�Ͽ� �߰�
        //foreach (var item in l_colliders)
        //{
        //    float targetRadian = Vector3.Dot(m_object.transform.forward,
        //        (item.transform.position - m_object.transform.forward).normalized);

        //    if (targetRadian < l_radianRange)
        //    {
        //        m_targets.Add(item.gameObject);
        //    }
        //}

        Vector3 lookDir = AngleToDir(m_object.transform.eulerAngles.y);
        foreach (Collider collider in l_colliders) {
            Vector3 targetDir = (collider.transform.position -
                (m_object.transform.position + Vector3.up * 0.5f)).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            if (targetAngle <= m_attackAngle) {
                m_targets.Add(collider.GetComponent<PlayerController>());
            }
        }
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }
}
