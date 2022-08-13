using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class Skill_Melee : AI_Skill
{
    List<GameObject> m_targets;
    private float m_rangeAngle = 45f;
    private int m_damage = 10;
    GameObject m_particle;

    public Skill_Melee(GameObject _object, int _id, float _coolTime, float _range, int _priority,
        int _damage)
    {
        m_object = _object;
        m_id = _id;
        m_coolTime = _coolTime;
        m_coolDown = 0;
        m_range = _range;
        m_priority = _priority;
        m_damage = _damage;

        m_targets = new List<GameObject>();
    }

    public override void Initialize() { }
    public override void Terminate() { }

    public override AI.State Update()
    {
        if (m_iscoolTime == false)
        {
            OnMeleeAttack();
        }

        m_coolDown += Time.deltaTime;
        if (m_coolTime <= m_coolDown)
        {
            m_iscoolTime = false;
            m_coolDown = 0;

            return AI.State.SUCCESS;
        }

        return AI.State.RUNNING;
    }

    private void OnMeleeAttack()
    {
        FindTarget();

        //�ִϸ��̼� ����

        //����Ʈ
        if (m_particle == null)
        {
            m_particle = Managers.Resource.Instantiate(Path.Slash_Particle, m_object.transform);

            //��ġ ����
            Vector3 vec = m_object.transform.position + m_object.transform.forward;
            vec = new Vector3(vec.x, vec.y + 1, vec.z);
            m_particle.transform.position = vec;

            //ȸ��
            m_particle.transform.Rotate(Quaternion.FromToRotation(m_particle.transform.forward,
                m_object.transform.forward).eulerAngles);

            m_particle.GetComponent<ParticleSystem>().Stop();
        }
        m_particle.GetComponent<ParticleSystem>().Play();

        m_iscoolTime = true;

        //ȿ������
        foreach (var player in m_targets)
        {
            //hp�� ���� �ȵ�
            player.GetComponent<PlayerController>().Damage(m_damage);
            Debug.Log("��������:" + System.DateTime.Now);
        }
    }

    //���� ���� Ÿ�� Ž��
    void FindTarget()
    {
        //Ư�� �Ÿ� ���� player Ž��
        Collider[] l_colliders = Physics.OverlapSphere(m_object.transform.position, m_range, 1 << (int)Define.Layer.Player);

        m_targets.Clear();//�ʱ�ȭ

        float l_radianRange = Mathf.Cos(Mathf.Deg2Rad * (m_rangeAngle / 2));//���� ����

        //���� ���� player Ÿ�Ͽ� �߰�
        foreach (var item in l_colliders)
        {
            float targetRadian = Vector3.Dot(m_object.transform.forward,
                (item.transform.position - m_object.transform.forward).normalized);

            if (targetRadian < l_radianRange)
            {
                m_targets.Add(item.gameObject);
            }
        }
    }
}
