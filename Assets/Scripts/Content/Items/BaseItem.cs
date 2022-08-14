using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    protected int         m_id;
    protected Vector3     m_position;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (int)Define.Layer.Player)
        {
            Active(other.gameObject.transform.GetComponent<PlayerController>());
        }
    }


    public virtual void Active(PlayerController _player) { }
}
