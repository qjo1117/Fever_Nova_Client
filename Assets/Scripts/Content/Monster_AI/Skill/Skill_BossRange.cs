using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BossRange : Interface_Skill
{
    private int m_damage = 10;
    private float m_skillPlayTime;

    private float m_projectileSpeed = 3f;
    private float m_projectileLifeDuration = 5f;

    private bool m_isSkillEnd;
    private float m_timeCheck;

    private string m_animationFileName;
    private string m_effectFileName;

    public Skill_BossRange(GameObject _object, int _id, float _cooltime, float _range, int _priority,
        int _damage, float _skillPlayTime, float _projectileSpeed, float _projectileLifeDuration,
        string _animationFileName = "Shooting-Fire-Rifle2", string _effectFileName = Path.Fire_Particle)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<Animator>();
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
    }

    public override void Initialize() { }

    public override void Terminate() { }

    public override AI.State Update()
    {
        return OnRangeAttack();
    }

    private AI.State OnRangeAttack()
    {
        if (!m_isSkillEnd) {
            m_object.transform.LookAt(new Vector3(Managers.Game.Player.MainPlayer.transform.position.x, 0, Managers.Game.Player.MainPlayer.transform.position.z));
            SetAnimation(m_animationFileName, 0.15f, m_skillPlayTime);

            CreateBullet(m_object.transform.forward);
            CreateBullet(m_object.transform.forward + m_object.transform.right);
            CreateBullet(m_object.transform.forward - m_object.transform.right);
            CreateBullet(m_object.transform.forward - m_object.transform.right / 2.0f);
            CreateBullet(m_object.transform.forward + m_object.transform.right / 2.0f);

            m_isSkillEnd = true;
        }

        m_timeCheck += Time.deltaTime;
        if (m_timeCheck >= m_skillPlayTime) {
            m_timeCheck = 0;
            m_isSkillEnd = false;
            return AI.State.SUCCESS;
        }
        return AI.State.RUNNING;
    }

    public void CreateBullet(Vector3 _forward)
	{
        GameObject bullet = Managers.Resource.Instantiate(m_effectFileName, Managers.Game.Boom.transform);
        bullet.transform.position = (m_object.transform.position + _forward * 2);

        bullet.GetComponent<Projectile_Bullet>().Initialized(_forward * 2.0f, m_damage, m_projectileSpeed);
        Managers.Resource.Destroy(bullet, m_projectileLifeDuration);
    }
}