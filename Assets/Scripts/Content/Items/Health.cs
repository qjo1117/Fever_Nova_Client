using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : BaseItem
{
    [SerializeField]
    private                 int m_hpPlus;

    public override void Active(PlayerController _player)
    {
        if(_player.Stat.hp < _player.Stat.maxHp)
        {
            _player.Recover(m_hpPlus);
            Destroy(gameObject);
        }
    }
}
