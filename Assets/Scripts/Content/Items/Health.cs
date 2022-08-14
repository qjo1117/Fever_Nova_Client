using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : BaseItem
{
    private                 int m_hpPlus = 10;

    public override void Active(PlayerController _player)
    {
        _player.Recover(m_hpPlus);
        Managers.Resource.Destroy(gameObject);
    }
}
