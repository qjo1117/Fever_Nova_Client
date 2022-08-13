using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NormalMonsterHPBar : UI_MonsterHPBar
{
    #region 변수
    [SerializeField]
    protected Camera m_mainCamera;

    public int m_verticalOffset;               // 월드위치 Y좌표 offset값
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        m_isReady = true;


        GetHpBoost();
        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }

        HpBarUpdate();
        CanvasScaleChange();
    }

    private void Update()
    {
        HpBarPositionUpdate();
    }

    // hp바의 월드 포지션을 업데이트 하는 함수
    private void HpBarPositionUpdate()
    {
        Collider l_parentCollider = transform.parent.GetComponent<Collider>();

        // hp바의 위치를 해당 hp바를 가지고있는 몬스터(hp바의 부모) 의 위치에서 수직으로 콜라이더의 ysize값 + y좌표 offset만큼 떨어진곳으로 설정
        transform.position = transform.parent.position + Vector3.up * (l_parentCollider.bounds.size.y + m_verticalOffset);

        // 해당 hp바를 가지는 몬스터 (hp바의 부모)가 회전하면 hp바도 회전하기떄문에, hp바를 항상 카메라를 향하도록함
        transform.LookAt(m_mainCamera.transform);
        // 180도 돌아간 상태를 해결하기위해 -180도 회전시켜줌
        transform.Rotate(Vector3.up, -180.0f);
    }

    // canvas의 크기를 hp바 크기에 맞게 조절해주는 함수
    private void CanvasScaleChange()
    {
        RectTransform l_hpbarRect = Get<Image>((int)Images.hpBarBackground).GetComponent<RectTransform>();
        Vector2 l_newCanvasSize = l_hpbarRect.sizeDelta * l_hpbarRect.localScale.x;

        GetComponent<RectTransform>().sizeDelta = l_newCanvasSize;
    }
}
