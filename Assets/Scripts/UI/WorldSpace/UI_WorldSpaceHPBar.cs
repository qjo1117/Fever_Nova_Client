using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorldSpaceHPBar : UI_Base
{
    #region UI컴포넌트_ENUM
    protected enum Images
    {
        hpBarBackground,                                         // hp바 뒷배경 (hp바 하얀색 배경)
        hpBar                                                    // hp바     
    }

    protected enum GameObjects
    {
        hpLine                                                  // 하얀색 사각형프레임(Image,Image(1)....) 관리 오브젝트 
    }
    #endregion

    #region 변수
    [SerializeField]
    protected Camera m_mainCamera;
    protected int m_hp;                           // Hp 바에 출력에 사용될 hp값
    protected int m_maxHp;
    protected bool m_isReady;

    public int m_verticalOffset;               // 월드위치 Y좌표 offset값
    public int m_unitHp;                       // 단위 hp 
                                               // (단위 hp가 낮을수록 최대 체력이 증가했을떄 하얀색 사각형이 촘촘하개 배열됨)
    #endregion

    #region 프로퍼티
    // 프로퍼티를 통해 hp값을 변경하면 자동으로 hpBar의 상태를 갱신하도록함
    public int HP
    {
        get
        {
            return m_hp;
        }
        set
        {
            m_hp = value;
            HpBarUpdate();
        }
    }

    public int MaxHP
    {
        get
        {
            return m_maxHp;
        }
        set
        {
            m_maxHp = value;
            GetHpBoost();
            HpBarUpdate();
        }
    }
    #endregion




    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        GetHpBoost();

        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }
        m_isReady = true;

        HpBarUpdate();
        CanvasScaleChange();
    }

    protected virtual void Update()
    {
        HpBarPositionUpdate();
    }

    // 최대 체력량에 따라 하얀색 사각 프레임 크기 조정하는 함수
    private void GetHpBoost()
    {
        if (!m_isReady)
            return;

        // (단위 hp / 최대체력) 계산하여 하얀색 사각 프레임의 스케일값 구함
        float l_scaleX = (float)m_unitHp / m_maxHp;

        // hpLine 오브젝트의 자식들 (하얀색 사각 프레임들)의 스케일값을 변경
        // (HorizontalLayoutGroup 컴포넌트가 활성화 되어있으면 Scale이 제대로 적용이 안되므로 적용전에 잠시 비활성화 시켜줌)
        GameObject l_hpLine = Get<GameObject>((int)GameObjects.hpLine);
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in l_hpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(l_scaleX, 1, 1);
        }
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
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

    // hp 바의 상태 갱신하는 함수
    private void HpBarUpdate()
    {
        if (!m_isReady)
            return;

        Get<Image>((int)Images.hpBar).fillAmount = m_hp / (float)m_maxHp;
    }

    // canvas의 크기를 hp바 크기에 맞게 조절해주는 함수
    private void CanvasScaleChange()
    {
        RectTransform l_hpbarRect = Get<Image>((int)Images.hpBarBackground).GetComponent<RectTransform>();
        Vector2 l_newCanvasSize = l_hpbarRect.sizeDelta * l_hpbarRect.localScale.x;

        GetComponent<RectTransform>().sizeDelta = l_newCanvasSize;
    }
}
