using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bullet : MonoBehaviour
{
    private Vector3 m_attackPos;
    private float m_speed = 3f;       //투사체 속도
    private int m_damage = 10;

    Vector3 m_attackDir;

    public void Initialized(Vector3 _pos, int _damage, float _speed)
    {
        m_attackPos = _pos;
        m_damage = _damage;
        m_speed = _speed;

        m_attackDir = m_attackPos;
        transform.LookAt(transform.position + m_attackDir);
    }

    void Update()
    {
        gameObject.transform.position += m_attackDir.normalized * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case (int)Define.Layer.Ground:
                Managers.Resource.Destroy(gameObject);
                break;
            case (int)Define.Layer.Monster:
                break;
            case (int)Define.Layer.Player:
                Managers.Resource.Destroy(gameObject);
                other.GetComponent<PlayerController>().Damage(m_damage);
                break;
            case (int)Define.Layer.Block:
                break;
        }
    }
}
