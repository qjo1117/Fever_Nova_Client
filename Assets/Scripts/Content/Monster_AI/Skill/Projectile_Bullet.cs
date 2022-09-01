using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bullet : MonoBehaviour
{
    private Vector3 m_attackPos = Vector3.zero;
    private float m_speed = 3f;       //투사체 속도
    private int m_damage = 10;
    private Vector3 m_attackDir = Vector3.zero;

    public void Initialized(Vector3 _pos, int _damage, float _speed)
    {
        m_attackPos = _pos;
        m_damage = _damage;
        m_speed = _speed;

        m_attackDir = m_attackPos;
        m_attackDir.y = 0.0f;
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
