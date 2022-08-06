using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------------------------------
//반경, 범위를 나타내는 UI의경우 표시할 이미지를 변경할떄
//Pixel per unit값을 조정해서 Unity의 1Unit에 해당하는 크기로
//조정하고, SetUIScale함수의 'm_rangeRadius / 0.5f '의 0.5f값을
//조정한 이미지의 반지름으로 변경해주어야함.
//
//간단히 말해서 단위 통일화 시켜주어야함
//------------------------------------------------------------


public class UI_BombRange : UI_Scene
{
    #region 변수
    [SerializeField]
    protected float       m_rangeRadius = 0.5f;                 // 반경 반지름
    #endregion

    #region 프로퍼티
    public float RangeRadius
    {
        get
        {
            return m_rangeRadius;
        }
        set
        {
            m_rangeRadius = value;
            SetUIScale();
        }
    }
    #endregion

    public override void Init()
    {

    }

    private void FixedUpdate()
    {
        PositionUpdate();
    }

    // 반지름에 따라 UI의 크기 조정하는 함수
    public void SetUIScale()
    {
        // 현재 이미지의 Pixel per unit 조정해서 scale 1,1,1 기준 반지름 0.5f
        transform.localScale = Vector3.one * (m_rangeRadius / 0.5f);
    }

    // UI의 위치를 플레이어의 위치로 갱신해주는 함수
    private void PositionUpdate()
    {
        PlayerController l_player = Managers.Game.Player.MainPlayer;
        if (l_player != null) {
            // 바닥과 겹쳐서 UI가 안보이므로 설정할 위치 Vector의 y값에 0.1f를 더함
            transform.position = new Vector3(l_player.transform.position.x, l_player.transform.position.y + 0.2f, l_player.transform.position.z);
        }
    }


    private void OnDrawGizmos()
    {
        // 반지름 라인 
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + m_rangeRadius, transform.position.y, transform.position.z));
    }
}
