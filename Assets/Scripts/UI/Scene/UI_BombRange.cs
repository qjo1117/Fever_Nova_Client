using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BombRange : UI_Scene
{
    [SerializeField]
    private float       m_rangeRadius = 0.5f;

    public float RangeRadius
    {
        get
        {
            return m_rangeRadius;
        }
        set
        {
            m_rangeRadius = value;
            UIScaleUpdate();
        }
    }


    public override void Init()
    {
        
    }

    private void Update()
    {
        PositionUpdate();
    }

    private void PositionUpdate()
    {
        PlayerController l_player = Managers.Game.Player.MainPlayer;

        transform.position = new Vector3(l_player.transform.position.x, l_player.transform.position.y + 0.1f, l_player.transform.position.z);
    }

    private void UIScaleUpdate()
    {
        // scale 1,1,1 기준 반지름 0.5f;
        transform.localScale = Vector3.one * (m_rangeRadius / 0.5f);
    }

    private void OnDrawGizmos()
    {
        // 반지름 라인 
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + m_rangeRadius, transform.position.y, transform.position.z));
    }
}
