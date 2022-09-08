using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Range : Interface_Skill
{
    private int m_damage = 10;
    private float m_skillPlayTime;

    private float m_projectileSpeed = 3f;
    private float m_projectileLifeDuration = 5f;

    private Vector3 m_attackPos;

    private bool m_isSkillEnd;
    private float m_timeCheck;

    private Transform m_muzzle = null;

    private string m_animationFileName;
    private string m_effectFileName;

    public Skill_Range(GameObject _object, int _id, float _cooltime, float _range, int _priority,
        int _damage, float _skillPlayTime, float _projectileSpeed, float _projectileLifeDuration, Transform _muzzle,
        string _animationFileName = "Shooting-Fire-Rifle2", string _effectFileName = Path.Fire_Particle)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<MyAnimator>();
        m_id = _id;
        m_coolTime = _cooltime;
        m_coolDown = 0;
        m_rangeForCastSkill = _range;
        m_priority = _priority;

        m_damage = _damage;
        m_skillPlayTime = _skillPlayTime;
        m_projectileSpeed = _projectileSpeed;
        m_projectileLifeDuration = _projectileLifeDuration;

        m_isSkillEnd = false;
        m_timeCheck = 0;

        m_animationFileName = _animationFileName;
        m_effectFileName = _effectFileName;

        if (_muzzle)
        {
            m_muzzle = _muzzle;
        }
        else
        {
            m_muzzle = m_object.transform;
        }
    }

    protected override AI.State Function()
    {
        return OnRangeAttack();
    }

    private AI.State OnRangeAttack()
    {
        if (!m_isSkillEnd)
        {
            m_object.transform.LookAt(new Vector3(Managers.Game.Player.MainPlayer.transform.position.x, 0, Managers.Game.Player.MainPlayer.transform.position.z));
            m_animator.SetAnimationSpeed_Second(AI.Enemy_AniState.NormalAttack, m_skillPlayTime);
            m_animator.GetTransitionFromState(AI.Enemy_AniState.Idle, AI.Enemy_AniState.NormalAttack).offset = 0.15f;
            m_animator.SetBool(AI.Enemy_AniParametar.AttackFlag, true);

            //SetAnimation(m_animationFileName, 0.15f, m_skillPlayTime);

            m_attackPos = Managers.Game.Player.MainPlayer.transform.position;
            m_attackPos += new Vector3(0, 1.5f, 0);

            ParticleSystem l_effect=  Managers.Effect.CreateEffect(Define.Effect.Projectile, m_effectFileName, (m_muzzle.position + m_object.transform.forward * 2), m_object.transform.forward, 0, 0, m_projectileLifeDuration, Managers.Game.Boom.transform);
            l_effect.Play();
            l_effect.GetComponent<Projectile_Bullet>().Initialized(m_object.transform.forward, m_damage, m_projectileSpeed);
           

            //GameObject bullet = Managers.Resource.Instantiate(m_effectFileName, Managers.Game.Boom.transform);
            //bullet.transform.position = (m_muzzle.position + m_object.transform.forward * 2);
            //
            //bullet.GetComponent<Projectile_Bullet>().Initialized(m_object.transform.forward, m_damage, m_projectileSpeed);
            //Managers.Resource.Destroy(bullet, m_projectileLifeDuration);

            m_isSkillEnd = true;
        }

        m_timeCheck += Time.deltaTime;
        if (m_timeCheck >= m_skillPlayTime)
        {
            m_timeCheck = 0;
            m_isSkillEnd = false;
            return AI.State.SUCCESS;
        }
        return AI.State.RUNNING;
    }
}